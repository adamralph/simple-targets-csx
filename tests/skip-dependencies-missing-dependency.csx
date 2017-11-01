#load "../artifacts/files/simple-targets.csx"
#load "helpers/assert.csx"

using static SimpleTargets;

// arrange
// this script should be run with the -s flag and no targets
var ranDefault = false;

var targets = new TargetDictionary
{
    { "default", DependsOn("test", "makedocs"), () => ranDefault = true },
    { "test", () => throw new Exception("test failed") },
};

// act
Run(Args, targets);

// assert
Assert.IsTrue(ranDefault, "ran default");
