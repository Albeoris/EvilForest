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

`export-all <input-directory> <output-directory>` creates one project per event and reports failures individually. The compiler validates the project before writing and uses a temporary file beside the destination for replacement.

## Source format

- `Event.cs` carries the event container metadata and internal location storage. Object slots and script IDs come from `ObjectSlot` and `Script` attributes. Names and filenames can change independently of IDs.
- The compiled `EvilForest.EventSource` dependency provides `Globals`, resource IDs, service APIs, and the instruction declarations consumed by the event compiler. Keep the DLL and the relative project reference when moving an export. A single export includes the DLL under `lib`. Batch exports share one DLL under the batch root. `generate-api <fixtures-directory> <dependency-source-directory>` refreshes the corpus-wide globals and other catalog sources before rebuilding the dependency.
- Typed storage declarations expose named global, location, and object data. For example, `Globals.FieldEntrance = 10000;` writes the signed 16-bit view of global bytes 2 and 3. `StorageView` preserves the physical view and explicit overlap. Current Memoria sign-extends both 24-bit encodings on reads. The `index-storage` command produces a list of views and a reference report with script IDs and nearby operations. `index-memoria-storage` records engine references, including unresolved dynamic expressions.
- You can declare a new location or object property with `[StorageField(StorageScope.Map, StorageType.Byte)]` or the corresponding `Instance` scope. The compiler allocates the first free slot after accounting for the existing declared views, including bit overlaps. Use `StorageView` to pin a public address. Re-exporting the binary emits its allocated views with pinned addresses.
- Scripts use native assignments, conditions and operators where the expression can be lifted safely. The exporter recovers `if`, `else`, `while`, `do` and `switch` for supported layouts. Other regions use native C# labels and jumps where their targets can be represented. The compiler relocates the targets after lowering. Use `disassemble <input.eb.bytes>` for opcode bytes and destinations.
- Suspensions appear as explicit calls such as `await Engine.Wait(frames: 25);`. Dialogue and music annotations come from game resource IDs. The source compiler validates the use of `await` and emits the corresponding event instruction. To execute an exported event in Memoria, compile it to event bytecode.
- Comments are informational. Re-export older projects from the original event when an encoded operand exists only in a comment.

The optional `--simplify` export represents an adjacent `WAIT` and `Return` as `WaitThenReturn` when no branch targets the return. The expansion emits the same two instructions, retains the wait's mask and operand, and keeps the frame boundary. Request scheduling operations remain explicit.

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
