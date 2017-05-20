#load "../artifacts/files/simple-targets.csx"

using static SimpleTargets;

var targets = new TargetDictionary();
targets.Add("build", () =>{});

using (var output = new StringWriter())
{
    try
    {
        SimpleTargetsRunner.Run(Args, targets, output);
    }
    catch (Exception exception)
    {
        if (!exception.Message.Contains($@"Target ""notarealtarget"" not found."))
        {
            throw new Exception($@"Expected exception to warn of unknown target ""notarealtarget"", but instead it said '{exception.Message}'.");
        }

        if (output.ToString().Contains("simple-targets/build"))
        {
            // relies on the script being run with targets "build" and "notarealtarget", in that order.
            // and with the --no-color flag
            throw new Exception(@"The ""build"" target was run before discovering that the ""notarealtarget"" target does not exist.");
        }

        return;
    }

    throw new Exception("Expected an exception, but none was found.");
}
