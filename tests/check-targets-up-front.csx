#load "../artifacts/files/simple-targets.csx"
#load "helpers/assert.csx"
#load "helpers/record.csx"

using static SimpleTargets;

// arrange
// this script should be run with targets "build" and "notarealtarget", in that order.
var targets = new TargetDictionary{ { "build", () => { } } };
var expectedMessage = $@"The following target was not found: ""notarealtarget"".";

// act
var exception = Record.Exception(() => Run(Args, targets));

// assert
Assert.HasMessage(exception, expectedMessage);
