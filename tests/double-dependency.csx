#load "../artifacts/files/contentFiles/csx/any/simple-targets.csx"

using static SimpleTargets;

var readVersionCount = 0;

var targets = new TargetDictionary();
targets.Add("default", DependsOn("pack"), () =>{});
targets.Add("pack", DependsOn("build", "readVersion"), () =>{});
targets.Add("build", DependsOn("readVersion"), () =>{});
targets.Add("readVersion", () =>
    {
        if ( readVersionCount++ > 0 )
        {
            throw new Exception("readVersion was called twice");
        }
    });

Run(Args, targets);

