using System.Collections.Generic;
using System.IO;

public static class SimpleTargetsCSharpTargets
{
    public static void Display(IDictionary<string, Target> targets, TextWriter output)
    {
        foreach (var target in targets)
        {
            output.WriteLine(target.Key);
        }
    }
}
