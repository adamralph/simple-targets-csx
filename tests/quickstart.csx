#load "../artifacts/files/simple-targets-csharp.csx"

using static SimpleTargets;

var targets = new Dictionary<string, Target>();

targets.Add("default", new Target(() => Console.WriteLine("Hello, world!")));

Run(Args, targets);
