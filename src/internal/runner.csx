#load "target-runner.csx"
#load "util.csx"
#load "../simple-targets-target.csx"

using System.Collections.Generic;
using System.IO;
using System.Linq;
using static SimpleTargets;
using static SimpleTargetsUtil;

public static class SimpleTargetsRunner
{
    public static void Run(IList<string> args, IDictionary<string, Target> targets, TextWriter output)
    {
        var showUsage = false;
        var showDependencies = false;
        var showList = false;
        var dryRun = false;
        var color = true;

        foreach (var option in args.Where(arg => arg.StartsWith("-", StringComparison.Ordinal)))
        {
            switch (option)
            {
                case "-H":
                case "-h":
                case "-?":
                    showUsage = true;
                    break;
                case "-D":
                    showDependencies = true;
                    break;
                case "-T":
                    showList = true;
                    break;
                case "-n":
                    dryRun = true;
                    break;
                case "--no-color":
                    color = false;
                    break;
                default:
                    throw new Exception($"Unknown option '{option}'.");
            }
        }

        if (showUsage)
        {
            output.Write(GetUsage(color));
            return;
        }

        if (showDependencies)
        {
            output.Write(GetDependencies(targets, color));
            return;
        }

        if (showList)
        {
            output.Write(GetList(targets));
            return;
        }

        var targetNames = args.Where(arg => !arg.StartsWith("-", StringComparison.Ordinal)).ToList();
        if (!targetNames.Any())
        {
            targetNames.Add("default");
        }

        output.WriteLine(StartMessage(targetNames, dryRun, color));

        SimpleTargetsTargetRunner.Run(targetNames, dryRun, targets, output, color);

        output.WriteLine(SuccessMessage(targetNames, dryRun, color));
    }
}
