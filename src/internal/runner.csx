#load "target-runner.csx"

using System;
using System.Collections.Generic;
using System.Linq;

public static class SimpleTargetsCSharpRunner
{
    public static void Run(IList<string> args, IDictionary<string, Target> targets)
    {
        foreach (var option in args.Where(arg => arg.StartsWith("-", StringComparison.Ordinal)))
        {
            switch (option)
            {
                case "-H":
                case "-h":
                case "-?":
                    Console.WriteLine("Usage: <script-runner> <script-file> [<options>] [<targets>]");
                    Console.WriteLine();
                    Console.WriteLine("script-runner: A C# script runner. E.g. csi.exe.");
                    Console.WriteLine();
                    Console.WriteLine("script-file: Path to a script.");
                    Console.WriteLine();
                    Console.WriteLine("options:");
                    Console.WriteLine(" -T      Display the targets, then exit");
                    Console.WriteLine();
                    Console.WriteLine("targets: A list of targets to run. If not specified, 'default' target will be run.");
                    Console.WriteLine();
                    Console.WriteLine("Examples:");
                    Console.WriteLine("  csi.exe build.csx");
                    Console.WriteLine("  csi.exe build.csx -T");
                    Console.WriteLine("  csi.exe build.csx test package");
                    return;
                case "-T":
                    foreach (var target in targets)
                    {
                        Console.WriteLine(target.Key);
                    }

                    return;
                default:
                    Console.WriteLine($"Unknown option '{option}'.");
                    return;
            }
        }

        var targetNames = args.Where(arg => !arg.StartsWith("-", StringComparison.Ordinal)).ToList();
        if (!targetNames.Any())
        {
            targetNames.Add("default");
        }

        SimpleTargetsCSharpTargetRunner.Run(targetNames, targets);

        Console.WriteLine(
            $"Target{(targetNames.Count > 1 ? "s" : "")} {string.Join(", ", targetNames.Select(name => $"'{name}'"))} succeeded.");
    }
}
