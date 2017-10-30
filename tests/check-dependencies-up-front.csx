#load "../artifacts/files/simple-targets.csx"
#load "helpers/assert.csx"
#load "helpers/record.csx"

using static SimpleTargets;

// arrange
var targets = new TargetDictionary();
targets.Add("default", DependsOn("pack"), () => { });
targets.Add("pack", DependsOn("build", "notarealdependency"), () => { });
targets.Add("build", () => { });
var expectedMessage = @"Missing dependency detected: ""notarealdependency"", required by ""pack""";

// act
var exception = Record.Exception(() => Run(Args, targets));

// assert
Assert.HasMessage(exception, expectedMessage);
