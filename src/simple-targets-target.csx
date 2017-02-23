using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public static partial class SimpleTargets
{
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
