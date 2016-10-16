#load "../artifacts/files/simple-targets-csharp.csx"

using static SimpleTargets;

var targets = new Dictionary<string, Target>();

targets.Add("default", new Target(DependsOn("world", "exclaim")));

targets.Add("hello", new Target(() => Console.WriteLine("Hello, ")));

targets.Add("world", new Target(DependsOn("hello"), () => Console.WriteLine("World")));

targets.Add("exclaim", new Target(() => Console.WriteLine("!")));

Run(Args, targets);
