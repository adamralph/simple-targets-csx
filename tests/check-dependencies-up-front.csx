#load "../artifacts/files/simple-targets.csx"
#load "helpers/assert.csx"
#load "helpers/record.csx"

using static SimpleTargets;

// arrange
var targets = new TargetDictionary
{
    { "default", DependsOn("pack"), () => { } },
    { "pack", DependsOn("build", "notarealdependency"), () => { } },
    { "build", () => { } },
};

var expectedMessage = @"Missing dependency detected: ""notarealdependency"", required by ""pack""";

// act
var exception = Record.Exception(() => Run(Args, targets));

// assert
Assert.HasMessage(exception, expectedMessage);
