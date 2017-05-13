using System;
using System.Collections.Generic;
using System.Linq;
using static SimpleTargets;
using static SimpleTargetsUtil;

using System.Collections.Generic;

public class SimpleTargetsTargetRunnerOptions
{
    public SimpleTargetsTargetRunnerOptions(IDictionary<string, Target>  targets, TextWriter output)
    {
        this.Targets = targets;
        this.Output = output;
        this.DryRun = false;
        this.Color = true;
        this.RunDependencies = true;
    }

    public IDictionary<string, Target> Targets { get; }

    public TextWriter Output { get; }

    public bool DryRun { get; set; }

    public bool Color { get; set; }

    public bool RunDependencies { get; set; }
}
