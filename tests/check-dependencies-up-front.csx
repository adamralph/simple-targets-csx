#load "../artifacts/files/simple-targets.csx"

using static SimpleTargets;

var targets = new TargetDictionary();
targets.Add("default", DependsOn("pack"), () =>{});
targets.Add("pack", DependsOn("build", "notarealdependency"), () =>{});
targets.Add("build", () =>{});

using (var output = new StringWriter())
{
    try
    {
        SimpleTargetsRunner.Run(Args, targets, output);
    }
    catch (Exception exception)
    {
        if (!exception.Message.Contains($@"Target ""notarealdependency"" not found."))
        {
            throw new Exception($@"Expected exception to warn of unknown target ""notarealdependency"", but instead it said '{exception.Message}'.");
        }

        if (output.ToString().Contains("simple-targets/build"))
        {
            // relies on the script being run with targets "build" and "notarealtarget", in that order.
            // and with the --no-color flag
            throw new Exception(@"The ""build"" target was run before discovering that the ""notarealdependency"" target does not exist.");
        }

        return;
    }

    throw new Exception("expected an exception, but none was found");
}
