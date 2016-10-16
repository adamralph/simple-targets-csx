using System;
using System.Collections.Generic;
using System.Linq;
using static SimpleTargets;

public static class SimpleTargetsTargetRunner
{
    public static void Run(IList<string> targetNames, bool dryRun, IDictionary<string, Target> targets, TextWriter output)
    {
        var targetsRan = new HashSet<string>();
        foreach (var name in targetNames)
        {
            RunTarget(name, dryRun, targets, targetsRan, output);
        }
    }

    private static void RunTarget(
        string name, bool dryRun, IDictionary<string, Target> targets, ISet<string> targetsRan, TextWriter output)
    {
        Target target;
        if (!targets.TryGetValue(name, out target))
        {
            throw new InvalidOperationException($"Target '{name}' not found.");
        }

        targetsRan.Add(name);

        foreach (var dependency in target.Dependencies.Except(targetsRan))
        {
            RunTarget(dependency, dryRun, targets, targetsRan, output);
        }

        if (target.Action != null)
        {
            output.WriteLine($"Running target '{name}'...{(dryRun ? " (dry run)" : "")}");
            if (!dryRun)
            {
                try
                {
                    target.Action.Invoke();
                }
                catch (Exception)
                {
                    output.WriteLine($"Target '{name}' failed!");
                    throw;
                }
            }
        }
    }
}
