#load "internal/runner.csx"

using System;
using System.Collections.Generic;

public static void Run(IList<string> args, IDictionary<string, Target> targets)
{
    SimpleTargetsCSharpRunner.Run(args, targets);
}

public class Target
{
    private string[] dependOn = new string[0];

    public string[] DependOn
    {
        get { return this.dependOn; }
        set { this.dependOn = value ?? new string[0]; }
    }

    public Action Do { get; set; }
}
