# Evil Forest 
    Final Fantasy IX Event Engine Tools

## Project Overview

This project is a toolkit for editing `.eb` files from the game Final Fantasy IX, which contain bytecode that describes events in battles, on game locations, and the global map.

**Final Goal**: The ultimate goal of this project is to decompile all events into understandable C# code, operating with abstractions sufficient to replay the events in the game without altering the original logic.

**Immediate Goals**: Our immediate aims are to achieve maximally readable code for all events and to learn how to convert it back into bytecode. This won't allow for writing arbitrary C# code, as you are still constrained by the capabilities of the bytecode. However, it will make editing game scripts much more convenient.

**Core Concept**: A primary idea of the project is the optimization and reorganization of the code in a way that facilitates the writing of new game events. Therefore, the decompiled code may significantly differ from a line-by-line interpretation of the bytecode, but it must functionally perform the same actions.

## Build and edit an event

The CLI and Windows editor target .NET 10. Exported source projects and the test suite target .NET 8.

```powershell
dotnet build -c Release
dotnet run --project EvilForest -- export EVT_ALEX1_TS_CARGO_0.eb.bytes Cargo
dotnet build Cargo/Event.csproj
dotnet run --project EvilForest -- compile Cargo Cargo.edited.eb.bytes
dotnet run --project EvilForest.Editor -- Cargo.edited.eb.bytes
```

For a Cargo scale edit, change `scale: 936` to `scale: 500` in `00_Object_00.cs`, in the script with ID 10. The editor exposes the same value under **SID 00 â†’ Script 10 â†’ SetScale â†’ Scale**. It saves through the same instruction encoder and EV serializer as the CLI. Immediate instruction operands are editable in the editor. Expressions, argument masks, and branch targets are editable in exported C#.

`export-all <input-directory> <output-directory>` creates one project per event with a shared compiled dependency. Both export commands stage their output and require a fresh or empty destination. The CLI uses `EventEngine.CSharp` as its export directory when run without arguments. The EB compiler validates the project before writing and uses a temporary file beside the destination for replacement.

`compile-managed <project-directory> <output.dll>` validates the same event source and compiles managed handlers for the scheduler. It binds auto-properties for allocated and pinned storage to the same addresses as EB.

## Source format

- `Event.cs` declares the format version and internal location storage. The compiler writes the signature and derives the slot count from the object declarations. `Event.Header.bin` carries the 124 opaque header bytes. `Event.Trailer.bin` is present when the input has trailing data. Object slots and script IDs come from `ObjectSlot` and `Script` attributes.
- The compiled `EvilForest.EventSource` dependency provides `Globals`, resource IDs, service APIs, and the instruction declarations consumed by the event compiler. Keep the DLL and the relative project reference when moving an export. A single export includes the DLL under `lib`. Batch exports share one DLL under the batch root. `generate-api <fixtures-directory> <dependency-source-directory>` refreshes the corpus-wide globals and other catalog sources before rebuilding the dependency.
- Typed storage declarations expose named global, location, and object data. `Location` and `LocalMemory` wrap explicit-layout structs whose field types and offsets define both managed and EB access. Global accessors use their `StorageView` declarations. Overlapping views share the same bytes. Current Memoria sign-extends both 24-bit encodings on reads. The `index-storage` command produces a list of views and a reference report with script IDs and nearby operations.
- You can declare a new location or object property with `[StorageField(StorageScope.Map, StorageType.Byte)]` or the corresponding `Instance` scope. The compiler allocates the first free slot after accounting for the existing declared views, including bit overlaps. Use a field with `FieldOffset` to pin either kind of address. Re-exporting the binary emits pinned addresses.
- Scripts use native assignments, conditions and operators where the expression can be lifted safely. The exporter recovers `if`, `else`, `while`, `do` and `switch` for supported layouts. Other regions use native C# labels and jumps where their targets can be represented. The compiler relocates the targets after lowering. Use `disassemble <input.eb.bytes>` for opcode bytes and destinations.
- Suspensions appear as explicit calls such as `await Engine.Wait(frames: 25);`. Dialogue and music annotations come from game resource IDs. The source compiler validates the use of `await` and emits the corresponding event instruction. To execute an exported event in Memoria, compile it to event bytecode.
- Comments are informational. Re-export older projects from the original event when an encoded operand exists only in a comment.

The optional `--simplify` export represents an adjacent `WAIT` and `Return` as `WaitThenReturn` when no branch targets the return. The expansion emits the same two instructions, retains the wait's mask and operand, and keeps the frame boundary. Request scheduling operations remain explicit.

The exporter puts extracted actions in `Cutscenes/`. `TryStart`, `StartWhenReady`, and `RunAndWait` preserve the original request mode and priority. A scene helper called directly from its owning handler runs on that handler and can span waits. A get-only `[SceneCutscene]` binding on the event context supplies the instance for both managed registration and EB linking. Re-export projects from older versions to get this source layout.

`Together` groups actor tracks already scheduled by an existing field phase. Each original actor loop enters the same scene method and runs its own `Track` on the acting instance. The shared completion method preserves the original coordinator's polling and yields.

`CutsceneOutline.tsv` lists original entries, source paths, and unresolved dispatch. `diagnose-cutscene-memory <input.eb.bytes> <report-directory>` writes byte masks for all handlers and discoverable phase fragments, including coordinators. Both compilers work directly from the project source without these reports.

Both export commands accept `--no-cutscene-catalog` for deterministic fallback names and `--cutscene-catalog <path>` for a local JSON catalog. Each row contains `event`, `slot`, and `tag`, plus `name`, `group`, or `parallel`. To name a phase or its shared entry, supply its `phase` and the corresponding `fragment` key from `CutsceneOutline.tsv`. The exported C# project carries the resulting declarations and compiles independently of the catalog.

In the managed scheduler, an accepted `Inventory.TetraMaster` command suspends the field. The host calls `ResumeExternalMode()` when control returns to the field. A host `Retry` keeps the activation waiting for admission.

The shared dependency exposes an event scheduler for managed tests and host integrations. Its host supplies field, actors, audio, dialogue, and expression behavior. The scheduler runs immediate operations within a visit and resumes the routines on later ticks as their conditions become ready. Use the compiled event bytecode for execution in Memoria.

## Verification

```powershell
dotnet test -c Release --filter 'Category!=FullCorpus&Category!=MemoriaRuntime'

$env:EVILFOREST_FULL_CORPUS = '1'
$env:EVILFOREST_CORPUS_REPORT = Join-Path $PWD 'corpus.tsv'
dotnet test -c Release --filter Category=FullCorpus

$env:EVILFOREST_MEMORIA_MANAGED = 'C:\Path\To\FINAL FANTASY IX\x64\FF9_Data\Managed'
dotnet test -c Release --filter Category=MemoriaRuntime
```

The corpus run reports container and instruction identity, export, SDK build, source import, and byte comparison for all 818 fixtures. It builds a second clean copy with C# comments stripped, including comments in the support source. The optional Memoria checks load the installed managed assembly and exercise variable access, request acceptance and return, and instruction pointer reads. They cover managed VM behavior without a running Unity scene.

## Testing and Contibutions

You can use [this](https://github.com/Albeoris/EvilForest/releases/tag/v2024.05.01) archive to test the program. Feel free to make PR if you want to help us.
