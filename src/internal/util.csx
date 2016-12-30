using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SimpleTargets;

public static class SimpleTargetsUtil
{
    private const string Default    = "\x1b[0m";
    private const string Red        = "\x1b[31m";
    private const string Green      = "\x1b[32m";
    private const string Yellow     = "\x1b[33m";
    private const string Cyan       = "\x1b[36m";
    private const string White      = "\x1b[37m";

    private static readonly Dictionary<MessageType, string> Colors = new Dictionary<MessageType, string>
    {
        { MessageType.Start,    White },
        { MessageType.Success,  Green },
        { MessageType.Failure,  Red },
    };

    public static string Usage =>
@"Usage: <script-runner> <script-file> [<options>] [<targets>]

script-runner: A C# script runner. E.g. csi.exe.

script-file: Path to a script.

options:
 -D      Display the targets and dependencies, then exit
 -T      Display the targets, then exit
 -n      Do a dry run without executing actions

targets: A list of targets to run. If not specified, 'default' target will be run.

Examples:
  csi.exe build.csx
  csi.exe build.csx -T
  csi.exe build.csx test package
";

    public static string GetList(IDictionary<string, Target> targets)
    {
        var value = new StringBuilder();
        foreach (var target in targets.OrderBy(pair => pair.Key))
        {
            value.AppendLine(target.Key);
        }

        return value.ToString();
    }

    public static string GetDependencies(IDictionary<string, Target> targets)
    {
        var value = new StringBuilder();
        foreach (var target in targets.OrderBy(pair => pair.Key))
        {
            value.AppendLine(target.Key);
            foreach (var dependency in target.Value.Dependencies)
            {
                value.AppendLine("  " + dependency);
            }
        }

        return value.ToString();
    }

    public enum MessageType
    {
        Start,
        Success,
        Failure,
    }

    public static string Message(MessageType messageType, string text, bool dryRun) =>
        $"{GetPrefix()}{Colors[messageType]}{text}{GetSuffix(dryRun)}{Default}";

    public static string Message(MessageType messageType, string text, bool dryRun, string targetName) =>
        $"{GetPrefix(targetName)}{Colors[messageType]}{text}{GetSuffix(dryRun)}{Default}";

    private static string GetPrefix() =>
        $"{Cyan}simple-targets{White}: ";

    private static string GetPrefix(string targetName) =>
        $"{Cyan}simple-targets{White}/{Cyan}{targetName.Replace(": ", ":: ")}{White}: ";

    private static string GetSuffix(bool dryRun) => dryRun ? $"{Yellow} (dry run)" : "";
}
