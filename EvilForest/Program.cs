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
                return args switch
                {
                    ["export", String input, String destination] => Export(input, destination),
                    ["export", String input, String destination, "--simplify"] => Export(input, destination, simplify: true),
                    ["export", String input, String destination, "--flat"] => Export(input, destination, structured: false),
                    ["index-memoria-storage", String source, String report] => IndexMemoriaStorage(source, report),
                    ["index-storage", String input, String summary, String references] => IndexStorage(input, summary, references),
                    ["generate-api", String fixtures, String output] => GenerateApi(fixtures, output),
                    ["disassemble", String input] => Disassemble(input),
                    ["compile", String project, String output] => Compile(project, output),
                    ["export-all", String input, String destination] => ExportAll(input, destination),
                    [] => ExportAll("EventEngine", "EventEngine"),
                    _ => Usage()
                };
            }
            catch (Exception ex) when (ex is IOException || ex is InvalidDataException || ex is UnauthorizedAccessException || ex is ArgumentException)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
        }

        private static Int32 Export(String input, String destination, Boolean simplify = false, Boolean structured = true, String dependency = null)
        {
            String name = EventName(input);
            using var stream = File.OpenRead(input);
            EVContainer container = EVContainer.Read(stream);
            EventProjectExporter.Export(container, name, destination, simplify, structured, dependency);
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

        private static Int32 ExportAll(String input, String destination)
        {
            String dependency = EventProjectExporter.CopyDependencyTo(Path.Combine(destination, "lib"));
            Int32 succeeded = 0;
            Int32 failed = 0;
            foreach (String file in Directory.EnumerateFiles(input, "*" + EventExtension).OrderBy(path => path, StringComparer.OrdinalIgnoreCase))
            {
                try
                {
                    Export(file, Path.Combine(destination, EventName(file)), dependency: dependency);
                    succeeded++;
                }
                catch (Exception ex) when (ex is IOException || ex is InvalidDataException || ex is UnauthorizedAccessException || ex is ArgumentException)
                {
                    Console.Error.WriteLine($"{Path.GetFileName(file)}: {ex.Message}");
                    failed++;
                }
            }
            Console.WriteLine($"Exported {succeeded} event projects. {failed} failed.");
            return failed == 0 ? 0 : 1;
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
            Console.Error.WriteLine("EvilForest export <input.eb.bytes> <project-directory> [--simplify]");
            Console.Error.WriteLine("EvilForest index-memoria-storage <source-directory> <references.tsv>");
            Console.Error.WriteLine("EvilForest index-storage <input-directory> <summary.tsv> <references.tsv>");
            Console.Error.WriteLine("EvilForest generate-api <fixtures-directory> <dependency-source-directory>");
            Console.Error.WriteLine("EvilForest disassemble <input.eb.bytes>");
            Console.Error.WriteLine("EvilForest compile <project-directory> <output.eb.bytes>");
            Console.Error.WriteLine("EvilForest export-all <input-directory> <output-directory>");
            return 2;
        }
    }
}
