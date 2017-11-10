#load "../artifacts/files/contentFiles/csx/any/simple-targets.csx"
#load "helpers/assert.csx"
#load "helpers/record.csx"

using static SimpleTargets;

// arrange
var targets = new TargetDictionary
{
    { "default", DependsOn("pack"), () => { } },
    { "pack", DependsOn("build", "notarealdependency1", "notarealdependency2"), () => { } },
    { "build", () => { } },
};

var expectedMessage = @"Missing dependencies detected: ""notarealdependency1"", required by ""pack""; ""notarealdependency2"", required by ""pack""";

// act
var exception = Record.Exception(() => Run(Args, targets));

// assert
Assert.HasMessage(exception, expectedMessage);
