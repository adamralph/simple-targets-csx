#load "target-runner.csx"
#load "target-runner-options.csx"
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

        var options = new SimpleTargetsTargetRunnerOptions();

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
                    options.DryRun = true;
                    break;
                case "--no-color":
                    options.Color = false;
                    break;
                case "-s":
                    options.SkipDependencies = true;
                    break;
                default:
                    throw new Exception($"Unknown option '{option}'.");
            }
        }

        if (showUsage)
        {
            output.Write(GetUsage(options.Color));
            return;
        }

        if (showDependencies)
        {
            output.Write(GetDependencies(targets, options.Color));
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

        output.WriteLine(StartMessage(targetNames, options.DryRun, options.Color));

        try
        {
            SimpleTargetsTargetRunner.Run(targetNames, targets, output, options);
        }
        catch (Exception)
        {
            output.WriteLine(FailureMessage(targetNames, options.DryRun, options.Color));
            throw;
        }

        output.WriteLine(SuccessMessage(targetNames, options.DryRun, options.Color));
    }
}
