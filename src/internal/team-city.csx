public static class SimpleTargetsTeamCity
{
    public static string BlockOpened(string name, string description) =>
        $"##teamcity[blockOpened name='{Encode(name)}' description='{Encode(description)}']";

    public static string BlockClosed(string name) =>
        $"##teamcity[blockClosed name='{Encode(name)}']";

    public static string ErrorMessage(string message) =>
        Message(message, "ERROR");

    public static string Message(string message) =>
        Message(message, "NORMAL");

    private static string Message(string message, string status) =>
        $"##teamcity[message text='{Encode(message)}' status='{status}']";

    private static string Encode(string value) => value?
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
