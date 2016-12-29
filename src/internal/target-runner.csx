#load "text-writer.csx"

using System;
using System.Collections.Generic;
using System.Linq;
using static SimpleTargets;

public static class SimpleTargetsTargetRunner
{
    public static void Run(IList<string> targetNames, bool dryRun, IDictionary<string, Target> targets, TextWriter output, TextWriter error)
    {
        var targetsRan = new HashSet<string>();
        foreach (var name in targetNames)
        {
            if (!targetsRan.Contains(name))
            {
                RunTarget(name, dryRun, targets, targetsRan, output, error);
            }
        }
    }

    private static void RunTarget(
        string name, bool dryRun, IDictionary<string, Target> targets, ISet<string> targetsRan, TextWriter output, TextWriter error)
    {
        Target target;
        if (!targets.TryGetValue(name, out target))
        {
            throw new Exception($"Target '{name}' not found.");
        }

        targetsRan.Add(name);

        foreach (var dependency in target.Dependencies.Except(targetsRan))
        {
            RunTarget(dependency, dryRun, targets, targetsRan, output, error);
        }

        if (target.Action != null)
        {
            var targetOutput = TextWriter.Synchronized(new SimpleTargetsTextWriter(output, name));

            targetOutput.WriteLine($"Starting...{(dryRun ? " (dry run)" : "")}");

            if (!dryRun)
            {
                var originalOut = Console.Out;
                var originalError = Console.Error;

                Console.SetOut(targetOutput);
                Console.SetError(TextWriter.Synchronized(new SimpleTargetsTextWriter(error, name)));

                try
                {
                    target.Action.Invoke();
                }
                catch (Exception ex)
                {
                    targetOutput.WriteLine($"Failed! {ex.Message}");
                    throw new Exception($"Target '{name}' failed.", ex);
                }
                finally
                {
                    Console.SetOut(originalOut);
                    Console.SetError(originalError);
                }
            }

            targetOutput.WriteLine($"Succeeded.{(dryRun ? " (dry run)" : "")}");
        }
    }
}
