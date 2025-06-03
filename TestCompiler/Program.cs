using System;
using System.IO;
using EveilForest.CSharp;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Testing C# to bytecode compilation...");
        
        // Create test file
        string testDir = "/tmp/evilforest_test";
        Directory.CreateDirectory(testDir);
        
        string testCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @mes.ShowAndWait(windowId: 2, ui: 128, text: 35);
    }
}";
        
        File.WriteAllText(Path.Combine(testDir, "01_TestObject.cs"), testCode);

        try
        {
            var compiler = new CSharpEventCompiler();
            var objects = compiler.CompileDirectory(testDir);
            
            Console.WriteLine($"✓ Successfully compiled {objects.Length} objects");
            
            if (objects.Length > 0)
            {
                var obj = objects[0];
                Console.WriteLine($"  Object ID: {obj.Id}");
                Console.WriteLine($"  Variable Count: {obj.VariableCount}");
                Console.WriteLine($"  Script Count: {obj.Scripts.Length}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
        finally
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
        }
    }
}