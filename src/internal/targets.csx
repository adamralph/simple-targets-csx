using System.Collections.Generic;
using System.IO;
using System.Linq;
using static SimpleTargets;

public static class SimpleTargetsTargets
{
    public static void Display(IDictionary<string, Target> targets, TextWriter output)
    {
        foreach (var target in targets.OrderBy(pair => pair.Key))
        {
            output.WriteLine(target.Key);
        }
    }

    public static void DisplayWithDependencies(IDictionary<string, Target> targets, TextWriter output)
    {
        foreach (var target in targets.OrderBy(pair => pair.Key))
        {
            output.WriteLine(target.Key);
            foreach (var dependency in target.Value.Dependencies)
            {
                output.WriteLine("  " + dependency);
            }
        }
    }
}
