using System;
using System.IO;
using System.Linq;
using EveilForest.CSharp;
using Memoria.EventEngine.EV;

namespace EvilForest
{
    internal static class Program
    {
        private const String EventExtension = ".eb.bytes";

        private static Int32 Main(String[] args)
        {
            try
            {
                Boolean allowNative = args.Length >= 4 && args[0] is "export" or "export-all" && args[^1] == "--allow-native-phase-dispatch";
                if (allowNative)
                    args = args[..^1];
                return args switch
                {
                    ["export", String input, String destination] => Export(input, destination, allowNativePhaseDispatch: allowNative),
                    ["export", String input, String destination, "--simplify"] => Export(input, destination, simplify: true),
                    ["export", String input, String destination, "--flat"] => Export(input, destination, structured: false),
                    ["export", String input, String destination, "--no-cutscene-catalog"] => Export(input, destination, useCutsceneNames: false, allowNativePhaseDispatch: allowNative),
                    ["export", String input, String destination, "--cutscene-catalog", String catalog] => Export(input, destination, useCutsceneNames: false, cutsceneCatalog: CutsceneCatalog.Read(catalog), allowNativePhaseDispatch: allowNative),
                    ["index-memoria-storage", String source, String report] => IndexMemoriaStorage(source, report),
                    ["index-storage", String input, String summary, String references] => IndexStorage(input, summary, references),
                    ["diagnose-cutscene-memory", String input, String directory] => DiagnoseCutsceneMemory(input, directory),
                    ["generate-api", String fixtures, String output] => GenerateApi(fixtures, output),
                    ["disassemble", String input] => Disassemble(input),
                    ["compile", String project, String output] => Compile(project, output),
                    ["compile-managed", String project, String output] => CompileManaged(project, output),
                    ["export-all", String input, String destination] => ExportAll(input, destination, allowNativePhaseDispatch: allowNative),
                    ["export-all", String input, String destination, "--no-cutscene-catalog"] => ExportAll(input, destination, useCutsceneNames: false, allowNativePhaseDispatch: allowNative),
                    ["export-all", String input, String destination, "--cutscene-catalog", String catalog] => ExportAll(input, destination, useCutsceneNames: false, cutsceneCatalog: CutsceneCatalog.Read(catalog), allowNativePhaseDispatch: allowNative),
                    [] => ExportAll("EventEngine", "EventEngine.CSharp"),
                    _ => Usage()
                };
            }
            catch (Exception ex) when (ex is IOException || ex is InvalidDataException || ex is UnauthorizedAccessException || ex is ArgumentException)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
        }

        private static Int32 Export(String input, String destination, Boolean simplify = false, Boolean structured = true, String dependency = null, Boolean useCutsceneNames = true, CutsceneCatalog cutsceneCatalog = null, Boolean allowNativePhaseDispatch = false)
        {
            String name = EventName(input);
            using var stream = File.OpenRead(input);
            EVContainer container = EVContainer.Read(stream);
            EventProjectExporter.Export(container, name, destination, simplify, structured, dependency, useCutsceneNames, cutsceneCatalog, allowNativePhaseDispatch: allowNativePhaseDispatch);
            return 0;
        }

        private static Int32 IndexMemoriaStorage(String source, String report)
        {
            Int32 count = EngineStorageReport.Write(source, report);
            Console.WriteLine($"Indexed {count} engine storage references.");
            return 0;
        }

        private static Int32 IndexStorage(String input, String summary, String references)
        {
            Int32 count = StorageUsageReport.Write(input, summary, references);
            Console.WriteLine($"Indexed storage usage in {count} events.");
            return 0;
        }

        private static Int32 GenerateApi(String fixtures, String output)
        {
            EventProjectExporter.GenerateDependencySources(fixtures, output);
            return 0;
        }

        private static Int32 DiagnoseCutsceneMemory(String input, String directory)
        {
            using var stream = File.OpenRead(input);
            EventProjectExporter.DiagnoseCutsceneMemory(EVContainer.Read(stream), EventName(input), directory);
            return 0;
        }

        private static Int32 Disassemble(String input)
        {
            using var stream = File.OpenRead(input);
            EVContainer container = EVContainer.Read(stream);
            Console.WriteLine($"{EventName(input)} ({container.Objects.Length} objects)");
            foreach (EVContainerObject obj in container.Objects)
            {
                foreach (EVContainerScript script in obj.Scripts)
                {
                    Console.WriteLine($"Object {obj.Id}, script {script.Id}");
                    foreach (EVInstruction instruction in EVInstructionBody.Decode(script.Body).Instructions)
                    {
                        String bytes = Convert.ToHexString(script.Body.AsSpan(instruction.Label, instruction.OriginalLength));
                        String targets = String.Join(", ", instruction.Targets.Select(target => $"0x{target:X4}"));
                        String edges = targets.Length == 0 ? String.Empty : $" -> {targets}";
                        Console.WriteLine($"  {instruction.Label:X4}  {instruction.Opcode,-16} {bytes}{edges}");
                    }
                }
            }
            return 0;
        }

        private static Int32 Compile(String project, String output)
        {
            EVContainer container = EventProjectCompiler.Compile(project);
            EVFileWriter.Write(output, container);
            foreach (String change in EVAddressCheckpoint.GetChanges(container))
                Console.Error.WriteLine(change);
            return 0;
        }

        private static Int32 ExportAll(String input, String destination, Boolean useCutsceneNames = true, CutsceneCatalog cutsceneCatalog = null, Boolean allowNativePhaseDispatch = false)
        {
            Int32 count = EventProjectExporter.ExportAll(input, destination, useCutsceneNames, cutsceneCatalog, allowNativePhaseDispatch);
            Console.WriteLine($"Exported {count} event projects.");
            return 0;
        }

        private static Int32 CompileManaged(String project, String output)
        {
            Byte[] assembly = EventProjectCompiler.CompileManaged(project);
            File.WriteAllBytes(output, assembly);
            return 0;
        }

        private static String EventName(String file)
        {
            String name = Path.GetFileName(file);
            return name.EndsWith(EventExtension, StringComparison.OrdinalIgnoreCase)
                ? name.Substring(0, name.Length - EventExtension.Length)
                : Path.GetFileNameWithoutExtension(name);
        }

        private static Int32 Usage()
        {
            Console.Error.WriteLine("EvilForest export <input.eb.bytes> <project-directory> [--simplify | --flat | --no-cutscene-catalog | --cutscene-catalog <path>]");
            Console.Error.WriteLine("EvilForest index-memoria-storage <source-directory> <references.tsv>");
            Console.Error.WriteLine("EvilForest index-storage <input-directory> <summary.tsv> <references.tsv>");
            Console.Error.WriteLine("EvilForest diagnose-cutscene-memory <input.eb.bytes> <report-directory>");
            Console.Error.WriteLine("EvilForest generate-api <fixtures-directory> <dependency-source-directory>");
            Console.Error.WriteLine("EvilForest disassemble <input.eb.bytes>");
            Console.Error.WriteLine("EvilForest compile <project-directory> <output.eb.bytes>");
            Console.Error.WriteLine("EvilForest compile-managed <project-directory> <output.dll>");
            Console.Error.WriteLine("EvilForest export-all <input-directory> <output-directory> [--no-cutscene-catalog | --cutscene-catalog <path>]");
            Console.Error.WriteLine("Append --allow-native-phase-dispatch to export or export-all to retain unsupported native phase machines for inspection.");
            return 2;
        }
    }
}
