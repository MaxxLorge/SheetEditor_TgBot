using System.Text.RegularExpressions;

namespace SheetEditor.StaticData;

public static partial class RegularExpressions
{
    [GeneratedRegex("(?<=\").*(?=\")")]
    public static partial Regex CharactersBetweenQuotes();
}