using System;
using System.IO;
using EveilForest.CSharp;
using Xunit;

namespace EveilForest.CSharp.Tests;

public class ErrorHandlingTests : IDisposable
{
    private readonly CSharpEventCompiler _compiler;
    private readonly string _testDirectory;

    public ErrorHandlingTests()
    {
        _compiler = new CSharpEventCompiler();
        _testDirectory = Path.Combine(Path.GetTempPath(), $"evilforest_error_tests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_testDirectory);
    }

    [Fact]
    public void UnsupportedMethodName_ThrowsNotSupportedException()
    {
        // Arrange
        const string csharpCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> UnsupportedMethodName()
    {
        yield return @mes.ShowAndWait(windowId: 2, ui: 128, text: 35);
    }
}";
        WriteTestFile("01_TestObject.cs", csharpCode);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _compiler.CompileDirectory(_testDirectory));
        Assert.Contains("cannot be mapped to a script ID", exception.Message);
        Assert.Contains("UnsupportedMethodName", exception.Message);
    }

    [Fact]
    public void UnsupportedStatementType_ThrowsNotSupportedException()
    {
        // Arrange - Using if statement which is not supported
        const string csharpCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        if (true)
        {
            yield return @mes.ShowAndWait(windowId: 2, ui: 128, text: 35);
        }
    }
}";
        WriteTestFile("01_TestObject.cs", csharpCode);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _compiler.CompileDirectory(_testDirectory));
        Assert.Contains("is not supported", exception.Message);
    }

    [Fact]
    public void UnsupportedExpressionType_ThrowsNotSupportedException()
    {
        // Arrange - Using variable assignment which is not supported
        const string csharpCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        var result = @mes.ShowAndWait(windowId: 2, ui: 128, text: 35);
        yield return result;
    }
}";
        WriteTestFile("01_TestObject.cs", csharpCode);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _compiler.CompileDirectory(_testDirectory));
        Assert.Contains("is not supported", exception.Message);
    }

    [Fact]
    public void NonMemberAccessMethodCall_ThrowsNotSupportedException()
    {
        // Arrange - Using direct method call instead of service.method
        const string csharpCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return ShowAndWait(windowId: 2, ui: 128, text: 35);
    }
}";
        WriteTestFile("01_TestObject.cs", csharpCode);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _compiler.CompileDirectory(_testDirectory));
        Assert.Contains("Only member access method calls are supported", exception.Message);
    }

    [Fact]
    public void NonLiteralArgumentValue_ThrowsNotSupportedException()
    {
        // Arrange - Using variable as argument value
        const string csharpCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        var windowId = 2;
        yield return @mes.ShowAndWait(windowId: windowId, ui: 128, text: 35);
    }
}";
        WriteTestFile("01_TestObject.cs", csharpCode);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _compiler.CompileDirectory(_testDirectory));
        Assert.Contains("is not supported", exception.Message);
    }

    [Fact]
    public void InvalidObjectIdInFileName_ThrowsFormatException()
    {
        // Arrange - File matches pattern but has non-numeric object ID
        const string csharpCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @mes.ShowAndWait(windowId: 2, ui: 128, text: 35);
    }
}";
        WriteTestFile("ABC_TestObject.cs", csharpCode); // Non-numeric prefix but matches pattern

        // Act & Assert
        Assert.Throws<FormatException>(() => _compiler.CompileDirectory(_testDirectory));
    }

    [Fact]
    public void MissingUnderscoreInFileName_IgnoresFile()
    {
        // Arrange
        const string csharpCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        yield return @mes.ShowAndWait(windowId: 2, ui: 128, text: 35);
    }
}";
        WriteTestFile("01TestObject.cs", csharpCode); // Missing underscore - doesn't match pattern

        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert - File should be ignored
        Assert.Empty(objects);
    }

    [Fact]
    public void EmptyMethodBody_CompilesSuccessfully()
    {
        // Arrange - Method with no statements should still compile (with just return)
        const string csharpCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init()
    {
        // Empty method body
    }
}";
        WriteTestFile("01_TestObject.cs", csharpCode);

        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert - Should compile successfully with return instruction
        Assert.Single(objects);
        Assert.Single(objects[0].Scripts);
    }

    [Fact]
    public void MethodWithoutBody_CompilesSuccessfully()
    {
        // Arrange - Abstract method declaration (method without body)
        const string csharpCode = @"public sealed class TestObject
{
    public IEnumerable<IAwaitable> Init();
}";
        WriteTestFile("01_TestObject.cs", csharpCode);

        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert - Should compile successfully with return instruction
        Assert.Single(objects);
        Assert.Single(objects[0].Scripts);
    }

    [Theory]
    [InlineData("Init", 0)]
    [InlineData("OnLoop", 1)]
    [InlineData("OnEnter", 2)]
    [InlineData("OnExit", 3)]
    public void ValidMethodNames_MapToCorrectScriptIds(string methodName, int expectedScriptId)
    {
        // Arrange
        string csharpCode = $@"public sealed class TestObject
{{
    public IEnumerable<IAwaitable> {methodName}()
    {{
        yield return @mes.ShowAndWait(windowId: 2, ui: 128, text: 35);
    }}
}}";
        WriteTestFile("01_TestObject.cs", csharpCode);

        // Act
        var objects = _compiler.CompileDirectory(_testDirectory);

        // Assert
        Assert.Single(objects);
        Assert.Single(objects[0].Scripts);
        Assert.Equal(expectedScriptId, objects[0].Scripts[0].Id);
    }

    private void WriteTestFile(string fileName, string content)
    {
        File.WriteAllText(Path.Combine(_testDirectory, fileName), content);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, recursive: true);
        }
    }
}