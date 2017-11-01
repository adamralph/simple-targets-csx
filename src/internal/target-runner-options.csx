public class SimpleTargetsTargetRunnerOptions
{
    public SimpleTargetsTargetRunnerOptions()
    {
        this.DryRun = false;
        this.Color = true;
        this.SkipDependencies = false;
    }

    public bool DryRun { get; set; }

    public bool Color { get; set; }

    public bool SkipDependencies { get; set; }
}
