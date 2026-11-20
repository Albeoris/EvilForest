using Memoria.EventEngine.EV;
using System.Globalization;

namespace EveilForest.CSharp;

internal static class SwitchLayout
{
    public static string Key(IEnumerable<EVExpressionToken> selector, IEnumerable<string> sections, bool directDispatch = false)
    {
        string expression = string.Join(";", selector.Select(token => $"{token.Opcode:X2}:{token.Value.ToString(CultureInfo.InvariantCulture)}"));
        return $"{(directDispatch ? "D|" : "")}{expression}|{string.Join("|", sections)}";
    }

    public static string Section(IEnumerable<int> cases, bool fallback, bool returns)
    {
        string values = string.Join(",", cases.Select(value => value.ToString(CultureInfo.InvariantCulture)));
        return $"{values}{(fallback ? "D" : "")}{(returns ? "R" : "B")}";
    }
}
