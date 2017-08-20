#load "../artifacts/files/simple-targets.csx"
#load "helpers/assert.csx"
#load "helpers/record.csx"

using static SimpleTargets;

// arrange
// this script should be run with targets "what2", "build", and "what1", in that order.
var targets = new TargetDictionary{ { "build", () => { } } };

var expectedMessage = $@"The following targets were not found: ""what1"", ""what2"".";

// act
var exception = Record.Exception(() => Run(Args, targets));

// assert

Assert.HasMessage(exception, expectedMessage);
