#load "internal/runner.csx"

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public static class SimpleTargets
{
    public static void Run(IList<string> args, IDictionary<string, SimpleTargets.Target> targets) =>
        SimpleTargetsRunner.Run(args, targets, Console.Out);

    public class Target
    {
        public Target(IEnumerable<string> dependencies)
            : this(dependencies, null)
        {
        }

        public Target(Action action)
            : this(null, action)
        {
        }

        public Target(IEnumerable<string> dependencies, Action action)
        {
            this.Dependencies = new ReadOnlyCollection<string>(dependencies?.ToList() ?? new List<string>());
            this.Action = action;
        }

        public IReadOnlyList<string> Dependencies { get; }

        public Action Action { get; }
    }
}
