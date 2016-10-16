#load "../artifacts/files/simple-targets-csharp.csx"

var targets = new Dictionary<string, Target>();

targets.Add("default", new Target(new[] { "world", "exclaim" }));

targets.Add("hello", new Target(() => Console.WriteLine("Hello, ")));

targets.Add("world", new Target(new[] { "hello" }, () => Console.WriteLine("World")));

targets.Add("exclaim", new Target(() => Console.WriteLine("!")));

Run(Args, targets);
