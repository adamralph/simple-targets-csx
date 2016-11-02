#load "../artifacts/files/simple-targets-csx.csx"

using static SimpleTargets;

var targets = new TargetDictionary();

targets.Add("default", DependsOn("world", "exclaim"));

targets.Add("hello", () => Console.WriteLine("Hello, "));

targets.Add("world", DependsOn("hello"), () => Console.WriteLine("World"));

targets.Add("exclaim", () => Console.WriteLine("!"));

Run(Args, targets);
