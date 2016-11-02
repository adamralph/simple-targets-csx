#load "../artifacts/files/simple-targets-csx.csx"

using static SimpleTargets;

var targets = new TargetDictionary();

targets.Add("default", () => Console.WriteLine("Hello, world!"));

Run(Args, targets);
