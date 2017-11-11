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
    public static void Run(IList<string> targetNames, IDictionary<string, Target> targets, TextWriter output, SimpleTargetsTargetRunnerOptions options)
    {
        if (!options.SkipDependencies)
        {
            ValidateDependencies(targets);
        }

        ValidateTargets(targetNames, targets);

        var targetsRan = new HashSet<string>();
        foreach (var name in targetNames)
        {
            RunTarget(name, targets, targetsRan, output, options);
        }
    }

    private static void RunTarget(
        string name, IDictionary<string, Target> targets, ISet<string> targetsRan, TextWriter output, SimpleTargetsTargetRunnerOptions options)
    {
        var target = targets[name];

        if (!targetsRan.Add(name))
        {
            return;
        }

        if (!options.SkipDependencies)
        {
            foreach (var dependency in target.Dependencies)
            {
                RunTarget(dependency, targets, targetsRan, output, options);
            }
        }

        if (target.Action != null)
        {
            output.WriteLine(StartMessage(name, options.DryRun, options.Color));
            var stopWatch = Stopwatch.StartNew();

            if (!options.DryRun)
            {
                try
                {
                    target.Action.Invoke();
                }
                catch (Exception ex)
                {
                    output.WriteLine(FailureMessage(name, ex, options.DryRun, options.Color, stopWatch.Elapsed.TotalMilliseconds));
                    throw;
                }
            }

            output.WriteLine(SuccessMessage(name, options.DryRun, options.Color, stopWatch.Elapsed.TotalMilliseconds));
        }
    }

    private static void ValidateDependencies(IDictionary<string, Target> targets)
    {
        var missingDependencies = new SortedDictionary<string, SortedSet<string>>();

        foreach (var targetEntry in targets)
        {
            foreach (var dependencyName in targetEntry.Value.Dependencies
                .Where(dependencyName => !targets.ContainsKey(dependencyName)))
            {
                (missingDependencies.TryGetValue(dependencyName, out var set)
                        ? set
                        : missingDependencies[dependencyName] = new SortedSet<string>())
                    .Add(targetEntry.Key);
            }
        }

        if (!missingDependencies.Any())
        {
            return;
        }

        var message = $"Missing {(missingDependencies.Count > 1 ? "dependencies" : "dependency")} detected: " +
            string.Join(
                "; ",
                missingDependencies.Select(missingDependency =>
                    $@"{Quote(missingDependency.Key)}, required by {Quote(missingDependency.Value.ToList())}"));

        throw new Exception(message);
    }

    private static void ValidateTargets(IEnumerable<string> targetNames, IDictionary<string, Target> targets)
    {
        var unknownTargets = new SortedSet<string>(targetNames.Except(targets.Keys));
        if (!unknownTargets.Any())
        {
            return;
        }

        var message = $"The following target{(unknownTargets.Count() > 1 ? "s were" : " was")} not found: {Quote(unknownTargets)}.";
        throw new Exception(message);
    }
}
