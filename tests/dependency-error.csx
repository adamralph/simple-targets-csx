#load "../artifacts/files/simple-targets.csx"

using static SimpleTargets;

var readVersionCount = 0;

var targets = new TargetDictionary();
targets.Add("default", DependsOn("throw"), () => {});
targets.Add("throw", () => { throw new Exception("I fail, but am not required for the build"); });

Run(Args, targets);
