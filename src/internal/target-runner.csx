#load "util.csx"

using System;
using System.Collections.Generic;
using System.Linq;
using static SimpleTargets;
using static SimpleTargetsUtil;

public static class SimpleTargetsTargetRunner
{
    public static void Run(IList<string> targetNames, bool dryRun, IDictionary<string, Target> targets, TextWriter output)
    {
        var targetsRan = new HashSet<string>();
        foreach (var name in targetNames)
        {
            if (!targetsRan.Contains(name))
            {
                RunTarget(name, dryRun, targets, targetsRan, output);
            }
        }
    }

    private static void RunTarget(
        string name, bool dryRun, IDictionary<string, Target> targets, ISet<string> targetsRan, TextWriter output)
    {
        Target target;
        if (!targets.TryGetValue(name, out target))
        {
            throw new Exception($"Target \"{(name.Replace("\"", "\"\""))}\" not found.");
        }

        targetsRan.Add(name);

        foreach (var dependency in target.Dependencies.Except(targetsRan))
        {
            RunTarget(dependency, dryRun, targets, targetsRan, output);
        }

        if (target.Action != null)
        {
            output.WriteLine(Message("\x1b[37mStarting...\x1b[0m", dryRun, name));

            if (!dryRun)
            {
                try
                {
                    target.Action.Invoke();
                }
                catch (Exception ex)
                {
                    output.WriteLine(Message($"\x1b[31mFailed! {ex.Message}\x1b[0m", dryRun, name));
                    throw new Exception($"Target \"{(name.Replace("\"", "\"\""))}\" failed.", ex);
                }
            }

            output.WriteLine(Message("\x1b[32mSucceeded.\x1b[0m", dryRun, name));
        }
    }
}
