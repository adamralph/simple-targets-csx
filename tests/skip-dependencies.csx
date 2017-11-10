#load "../artifacts/files/contentFiles/csx/any/simple-targets.csx"
#load "helpers/assert.csx"

using static SimpleTargets;

// arrange
// this script should be run with the -s flag and no targets
var ranDefault = false;

var targets = new TargetDictionary
{
    { "default", DependsOn("test", "makedocs"), () => ranDefault = true },
    { "test", DependsOn("build"), () => throw new Exception("test failed") },
    { "build", () => throw new Exception("build failed") },
    { "makedocs", () => throw new Exception("makedocs failed") },
};

// act
Run(Args, targets);

// assert
Assert.IsTrue(ranDefault, "ran default");
