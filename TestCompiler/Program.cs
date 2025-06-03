using System;
using System.IO;
using EveilForest.CSharp;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== C# to Bytecode Compilation Test ===\n");
        
        // Test 1: Valid compilation
        Console.WriteLine("Test 1: Valid C# compilation");
        TestValidCompilation();
        
        Console.WriteLine("\nTest 2: Error handling for unsupported methods");
        TestErrorHandling();
        
        Console.WriteLine("\nTest 3: Multiple scripts compilation");
        TestMultipleScripts();
        
        Console.WriteLine("\n=== All Tests Completed ===");
    }
    
    private static void TestValidCompilation()
    {
        string testCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @mes.ShowAndWait(windowId: 2, ui: 128, text: 35);
    }
}";
        
        TestCompilationScenario("Valid compilation", testCode, expectSuccess: true);
    }
    
    private static void TestErrorHandling()
    {
        string testCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @unsupported.UnsupportedMethod(param: 123);
    }
}";
        
        TestCompilationScenario("Unsupported method", testCode, expectSuccess: false);
    }
    
    private static void TestMultipleScripts()
    {
        string testCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @mes.ShowAndWait(windowId: 2, ui: 128, text: 35);
    }
    
    public IEnumerable<IAwaitable> OnLoop()
    {
        yield return @actor.Wait(frames: 60);
    }
}";
        
        TestCompilationScenario("Multiple scripts", testCode, expectSuccess: true);
    }
    
    private static void TestCompilationScenario(string testName, string testCode, bool expectSuccess)
    {
        string testDir = $"/tmp/evilforest_test_{Guid.NewGuid():N}";
        
        try
        {
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "01_TestObject.cs"), testCode);

            var compiler = new CSharpEventCompiler();
            var objects = compiler.CompileDirectory(testDir);
            
            if (expectSuccess)
            {
                Console.WriteLine($"  ✓ {testName}: Successfully compiled {objects.Length} objects");
                
                if (objects.Length > 0)
                {
                    var obj = objects[0];
                    Console.WriteLine($"    - Object ID: {obj.Id}");
                    Console.WriteLine($"    - Variable Count: {obj.VariableCount}");
                    Console.WriteLine($"    - Script Count: {obj.Scripts.Length}");
                    
                    foreach (var script in obj.Scripts)
                    {
                        Console.WriteLine($"    - Script {script.Id}: OK");
                    }
                }
            }
            else
            {
                Console.WriteLine($"  ✗ {testName}: Expected error but compilation succeeded");
            }
        }
        catch (Exception ex) when (!expectSuccess)
        {
            Console.WriteLine($"  ✓ {testName}: Expected error caught - {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ✗ {testName}: Unexpected error - {ex.Message}");
        }
        finally
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, true);
        }
    }
}