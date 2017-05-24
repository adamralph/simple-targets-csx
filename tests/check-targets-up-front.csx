#load "../artifacts/files/simple-targets.csx"
#load "helpers/assertions.csx"

using static SimpleTargets;

var wasBuildRun = false;

var targets = new TargetDictionary();
targets.Add("build", () => { wasBuildRun = true; });

// relies on the script being run with targets "build" and "notarealtarget", in that order.
var expectedMessage = $@"The following target was not found: ""notarealtarget"".";
AssertThrowsWithMessage(
    expectedMessage,
    () => Run(Args, targets));
