#load "target-runner.csx"
#load "util.csx"

using System.Collections.Generic;
using System.IO;
using System.Linq;
using static SimpleTargets;
using static SimpleTargetsUtil;

public static class SimpleTargetsRunner
{
    public static void Run(IList<string> args, IDictionary<string, Target> targets, TextWriter output)
    {
        var dryRun = false;

        foreach (var option in args.Where(arg => arg.StartsWith("-", StringComparison.Ordinal)))
        {
            switch (option)
            {
                case "-H":
                case "-h":
                case "-?":
                    output.Write(Usage);
                    return;
                case "-D":
                    output.Write(GetDependencies(targets));
                    return;
                case "-T":
                    output.Write(GetList(targets));
                    return;
                case "-n":
                    dryRun = true;
                    break;
                default:
                    throw new Exception($"Unknown option '{option}'.");
            }
        }

        var targetNames = args.Where(arg => !arg.StartsWith("-", StringComparison.Ordinal)).ToList();
        if (!targetNames.Any())
        {
            targetNames.Add("default");
        }

        var prefix = $"\x1b[36msimple-targets\x1b[37m: ";
        var targetNamesFragment = string.Join(", ", targetNames.Select(name => $"\"{(name.Replace("\"", "\\\""))}\""));
        var dryRunFragment = dryRun ? "\x1b[33m (dry run)\x1b[0m" : "";

        output.WriteLine($"{prefix}\x1b[37mRunning {targetNamesFragment}...\x1b[0m{dryRunFragment}");

        SimpleTargetsTargetRunner.Run(targetNames, dryRun, targets, output);

        output.WriteLine($"{prefix}\x1b[32m{targetNamesFragment} succeeded.\x1b[0m{dryRunFragment}");
    }
}
