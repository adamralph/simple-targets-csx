public static class SimpleTargetsTeamCity
{
    public static string Encode(string value) => value?
        .Replace("|", "||")
        .Replace("'", "|'")
        .Replace("\r", "|r")
        .Replace("\n", "|n")
        .Replace("]", "|]")
        .Replace("[", "|[")
        .Replace("\u0085", "|x")
        .Replace("\u2028", "|l")
        .Replace("\u2029", "|p");
}
