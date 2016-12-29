#load "../artifacts/files/simple-targets.csx"

using static SimpleTargets;

var targets = new TargetDictionary();

targets.Add("default", DependsOn("worl:d", "exclaim"));

targets.Add("hell\"o", () => Console.WriteLine("Hello, "));

targets.Add("worl:d", DependsOn("hell\"o"), () => Console.WriteLine("World"));

targets.Add("exclaim", () => Console.WriteLine("!"));

Run(Args, targets);
