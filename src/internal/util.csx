using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SimpleTargets;

public static class SimpleTargetsUtil
{
    private const string Default        = "\x1b[0m";
    private const string Green          = "\x1b[32m";
    private const string Cyan           = "\x1b[36m";
    private const string White          = "\x1b[37m";
    private const string BrightRed      = "\x1b[91m";
    private const string BrightYellow   = "\x1b[93m";
    private const string BrightMagenta  = "\x1b[95m";

    private static readonly Dictionary<MessageType, string> Colors = new Dictionary<MessageType, string>
    {
        { MessageType.Start,    White },
        { MessageType.Success,  Green },
        { MessageType.Failure,  BrightRed },
    };

    public static string Usage =>
$@"{Cyan}Usage: {BrightYellow}<script-runner> {Default}<script-file> {White}[<options>] {Default}[<targets>]

{Cyan}script-runner: {Default}A C# script runner. E.g. {BrightYellow}csi.exe{Default}.

{Cyan}script-file: {Default}Path to a script. E.g. build.csx.

{Cyan}options:{Default}
 {White}-D      {Default}Display the targets and dependencies, then exit
 {White}-T      {Default}Display the targets, then exit
 {White}-n      {Default}Do a dry run without executing actions

{Cyan}targets: {Default}A list of targets to run. If not specified, 'default' target will be run.

{Cyan}Examples:{Default}
  {BrightYellow}csi.exe {Default}build.csx
  {BrightYellow}csi.exe {Default}build.csx {White}-T{Default}
  {BrightYellow}csi.exe {Default}build.csx test pack
  {BrightYellow}csi.exe {Default}build.csx {White}-n {Default}build
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
                value.AppendLine($"  {White}{dependency}{Default}");
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

    public static string Message(MessageType messageType, string text, string targetName) =>
        $"{GetPrefix(targetName)}{Colors[messageType]}{text}{Default}";

    private static string GetPrefix() =>
        $"{Cyan}simple-targets{White}: ";

    private static string GetPrefix(string targetName) =>
        $"{Cyan}simple-targets{White}/{Cyan}{targetName.Replace(": ", ":: ")}{White}: ";

    private static string GetSuffix(bool dryRun) => dryRun ? $"{BrightMagenta} (dry run)" : "";
}
