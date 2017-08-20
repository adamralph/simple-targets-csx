#load "../artifacts/files/simple-targets.csx"
#load "helpers/assertions.csx"

using static SimpleTargets;

var wasBuildRun = false;

var targets = new TargetDictionary();
targets.Add("build", () => {});

// relies on the script being run with targets "what2", "build", and "what1".
var expectedMessage = $@"The following targets were not found: ""what1"", ""what2"".";
AssertThrowsWithMessage(
    expectedMessage,
    () => Run(Args, targets));
