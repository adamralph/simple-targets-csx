#load "util.csx"
#load "../simple-targets-target.csx"

using System;
using System.Collections.Generic;
using System.Linq;
using static SimpleTargets;
using static SimpleTargetsUtil;

public static class SimpleTargetsTargetRunner
{
    public static void Run(IList<string> targetNames, bool dryRun, IDictionary<string, Target> targets, TextWriter output, bool color)
    {
        var targetsRan = new HashSet<string>();
        foreach (var name in targetNames)
        {
            RunTarget(name, dryRun, targets, targetsRan, output, color);
        }
    }

    private static void RunTarget(
        string name, bool dryRun, IDictionary<string, Target> targets, ISet<string> targetsRan, TextWriter output, bool color)
    {
        Target target = targets[name];

        if (!targetsRan.Add(name))
        {
            return;
        }

        foreach (var dependency in target.Dependencies)
        {
            RunTarget(dependency, dryRun, targets, targetsRan, output, color);
        }

        if (target.Action != null)
        {
            output.WriteLine(StartMessage(name, color));

            if (!dryRun)
            {
                try
                {
                    target.Action.Invoke();
                }
                catch (Exception ex)
                {
                    output.WriteLine(FailureMessage(name, ex, color));
                    throw new Exception($"Target \"{(name.Replace("\"", "\"\""))}\" failed.", ex);
                }
            }

            output.WriteLine(SuccessMessage(name, color));
        }
    }
}
