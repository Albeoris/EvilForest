using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

public interface IEventCompiler
{
    EVObject[] CompileDirectory(String directoryPath);
}