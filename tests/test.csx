#load "../artifacts/files/contentFiles/csx/any/simple-targets.csx"

using static SimpleTargets;

var targets = new TargetDictionary();

targets.Add("default", DependsOn("worl:d", "exclai: m"));

targets.Add("hell\"o", () => Console.WriteLine("Hello"));

targets.Add("comm/a", DependsOn("hell\"o"), () => Console.WriteLine(", "));

targets.Add("worl:d", DependsOn("comm/a"), () => Console.WriteLine("World"));

targets.Add("exclai: m", () => Console.WriteLine("!"));

Run(Args, targets);
