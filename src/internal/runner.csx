#load "target-runner.csx"

using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class SimpleTargetsCSharpRunner
{
    public static void Run(IList<string> args, IDictionary<string, Target> targets, TextWriter output)
    {
        foreach (var option in args.Where(arg => arg.StartsWith("-", StringComparison.Ordinal)))
        {
            switch (option)
            {
                case "-H":
                case "-h":
                case "-?":
                    output.WriteLine("Usage: <script-runner> <script-file> [<options>] [<targets>]");
                    output.WriteLine();
                    output.WriteLine("script-runner: A C# script runner. E.g. csi.exe.");
                    output.WriteLine();
                    output.WriteLine("script-file: Path to a script.");
                    output.WriteLine();
                    output.WriteLine("options:");
                    output.WriteLine(" -T      Display the targets, then exit");
                    output.WriteLine();
                    output.WriteLine("targets: A list of targets to run. If not specified, 'default' target will be run.");
                    output.WriteLine();
                    output.WriteLine("Examples:");
                    output.WriteLine("  csi.exe build.csx");
                    output.WriteLine("  csi.exe build.csx -T");
                    output.WriteLine("  csi.exe build.csx test package");
                    return;
                case "-T":
                    foreach (var target in targets)
                    {
                        output.WriteLine(target.Key);
                    }

                    return;
                default:
                    output.WriteLine($"Unknown option '{option}'.");
                    return;
            }
        }

        var targetNames = args.Where(arg => !arg.StartsWith("-", StringComparison.Ordinal)).ToList();
        if (!targetNames.Any())
        {
            targetNames.Add("default");
        }

        SimpleTargetsCSharpTargetRunner.Run(targetNames, targets, output);

        output.WriteLine(
            $"Target{(targetNames.Count > 1 ? "s" : "")} {string.Join(", ", targetNames.Select(name => $"'{name}'"))} succeeded.");
    }
}
