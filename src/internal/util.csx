using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SimpleTargets;

public static class SimpleTargetsUtil
{
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
}
