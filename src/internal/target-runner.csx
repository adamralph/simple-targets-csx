using System;
using System.Collections.Generic;
using System.Linq;

public static class SimpleTargetsCSharpTargetRunner
{
    public static void Run(IList<string> targetNames, IDictionary<string, Target> targets)
    {
        var targetsRan = new HashSet<string>();
        foreach (var name in targetNames)
        {
            RunTarget(name, targets, targetsRan);
        }
    }

    private static void RunTarget(string name, IDictionary<string, Target> targets, ISet<string> targetsRan)
    {
        Target target;
        if (!targets.TryGetValue(name, out target))
        {
            throw new InvalidOperationException($"Target '{name}' not found.");
        }

        targetsRan.Add(name);

        foreach (var dependency in target.DependOn.Except(targetsRan))
        {
            RunTarget(dependency, targets, targetsRan);
        }

        if (target.Do != null)
        {
            Console.WriteLine($"Running target '{name}'...");
            try
            {
                target.Do.Invoke();
            }
            catch (Exception)
            {
                Console.WriteLine($"Target '{name}' failed!");
                throw;
            }
        }
    }
}
