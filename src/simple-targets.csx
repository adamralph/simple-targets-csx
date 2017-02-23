#load "internal/runner.csx"
#load "simple-targets-target.csx"

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public static partial class SimpleTargets
{
    public static string[] DependsOn(params string[] dependencies) => dependencies;

    public static void Run(IList<string> args, IDictionary<string, SimpleTargets.Target> targets) =>
        SimpleTargetsRunner.Run(args, targets, Console.Out);

    public class TargetDictionary : Dictionary<string, Target>
    {
        public void Add(string name, IEnumerable<string> dependencies, Action action) =>
            this.Add(name, new Target(dependencies, action));

        public void Add(string name, IEnumerable<string> dependencies) =>
            this.Add(name, new Target(dependencies, null));

        public void Add(string name, Action action) =>
            this.Add(name, new Target(null, action));
    }
}
