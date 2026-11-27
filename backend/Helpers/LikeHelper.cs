namespace CgbSteps.Api.Helpers;

public static class LikeHelper
{
    public const string EscapeCharacter = "\\";

    /// <summary>Escapes \, % and _ so user text is matched literally in a LIKE pattern.</summary>
    public static string Escape(string value) => value
        .Replace("\\", "\\\\", StringComparison.Ordinal)
        .Replace("%", "\\%", StringComparison.Ordinal)
        .Replace("_", "\\_", StringComparison.Ordinal);
}
