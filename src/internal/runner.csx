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

        var targetNamesFragment = string.Join(", ", targetNames.Select(name => $"\"{(name.Replace("\"", "\"\""))}\""));

        output.WriteLine(Message(MessageType.Start, $"Running {targetNamesFragment}...", dryRun));

        SimpleTargetsTargetRunner.Run(targetNames, dryRun, targets, output);

        output.WriteLine(Message(MessageType.Success, $"{targetNamesFragment} succeeded.", dryRun));
    }
}
