#load "../artifacts/files/contentFiles/csx/any/simple-targets.csx"

using static SimpleTargets;

var targets = new TargetDictionary();

targets.Add("default", () => Console.WriteLine("Hello, world!"));

Run(Args, targets);
