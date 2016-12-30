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
            throw new Exception($@"Target ""{(name.Replace(@"""", @"\"""))}"" not found.");
        }

        targetsRan.Add(name);

        foreach (var dependency in target.Dependencies.Except(targetsRan))
        {
            RunTarget(dependency, dryRun, targets, targetsRan, output);
        }

        if (target.Action != null)
        {
            var prefix = $"\x1b[36msimple-targets\x1b[37m/\x1b[36m{name.Replace(":", "\\:")}\x1b[37m: ";

            output.WriteLine($"{prefix}\x1b[37mStarting...\x1b[0m{(dryRun ? "\x1b[33m (dry run)\x1b[0m" : "")}");

            if (!dryRun)
            {
                try
                {
                    target.Action.Invoke();
                }
                catch (Exception ex)
                {
                    output.WriteLine($"{prefix}\x1b[31mFailed! {ex.Message}\x1b[0m");
                    throw new Exception($@"Target ""{(name.Replace(@"""", @"\"""))}"" failed.", ex);
                }
            }

            output.WriteLine($"{prefix}\x1b[32mSucceeded.\x1b[0m{(dryRun ? "\x1b[33m (dry run)\x1b[0m" : "")}");
        }
    }
}
