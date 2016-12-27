#load "team-city.csx"

using System;
using System.Collections.Generic;
using System.Linq;
using static SimpleTargets;
using static SimpleTargetsTeamCity;

public static class SimpleTargetsTargetRunner
{
    public static void Run(IList<string> targetNames, bool dryRun, IDictionary<string, Target> targets, TextWriter output, bool isTeamCity)
    {
        var targetsRan = new HashSet<string>();
        foreach (var name in targetNames)
        {
            RunTarget(name, dryRun, targets, targetsRan, output, isTeamCity);
        }
    }

    private static void RunTarget(
        string name, bool dryRun, IDictionary<string, Target> targets, ISet<string> targetsRan, TextWriter output, bool isTeamCity)
    {
        Target target;
        if (!targets.TryGetValue(name, out target))
        {
            throw new InvalidOperationException($"Target '{name}' not found.");
        }

        targetsRan.Add(name);

        foreach (var dependency in target.Dependencies.Except(targetsRan))
        {
            RunTarget(dependency, dryRun, targets, targetsRan, output, isTeamCity);
        }

        if (target.Action != null)
        {
            var message = $"Running target '{name}'...{(dryRun ? " (dry run)" : "")}";
            if (isTeamCity)
            {
                message = $"##teamcity[blockOpened name='{name}' description='{Encode(message)}']";
            }
            
            output.WriteLine(message);

            try
            {
                if (!dryRun)
                {
                    try
                    {
                        target.Action.Invoke();
                    }
                    catch (Exception)
                    {
                        message = $"Target '{name}' failed!";

                        if (isTeamCity)
                        {
                            message = $"##teamcity[message text='{Encode(message)}' status='ERROR']";
                        }

                        output.WriteLine(message);
                        throw;
                    }
                }
            }
            finally
            {
                if (isTeamCity)
                {
                    output.WriteLine($"##teamcity[blockClosed name='{name}']");
                }
            }
        }
    }
}
