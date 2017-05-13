#load "target-runner-options.csx"
#load "util.csx"
#load "../simple-targets-target.csx"

using System;
using System.Collections.Generic;
using System.Linq;
using static SimpleTargets;
using static SimpleTargetsUtil;

public static class SimpleTargetsTargetRunner
{
    public static void Run(IList<string> targetNames, SimpleTargetsTargetRunnerOptions options)
    {
        var targetsRan = new HashSet<string>();
        foreach (var name in targetNames)
        {
            RunTarget(name, targetsRan, options);
        }
    }

    private static void RunTarget(
        string name, ISet<string> targetsRan, SimpleTargetsTargetRunnerOptions options)
    {
        Target target;
        if (!options.Targets.TryGetValue(name, out target))
        {
            throw new Exception($"Target \"{(name.Replace("\"", "\"\""))}\" not found.");
        }

        if (!targetsRan.Add(name))
        {
            return;
        }

        foreach (var dependency in target.Dependencies)
        {
            RunTarget(dependency, targetsRan, options);
        }

        if (target.Action != null)
        {
            options.Output.WriteLine(StartMessage(name, options.Color));

            if (!options.DryRun)
            {
                try
                {
                    target.Action.Invoke();
                }
                catch (Exception ex)
                {
                    options.Output.WriteLine(FailureMessage(name, ex, options.Color));
                    throw new Exception($"Target \"{(name.Replace("\"", "\"\""))}\" failed.", ex);
                }
            }

            options.Output.WriteLine(SuccessMessage(name, options.Color));
        }
    }
}
