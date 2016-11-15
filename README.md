![Icon](/assets/simple-targets-csx.png)

# simple-targets-csx

[![NuGet version](https://img.shields.io/nuget/v/simple-targets-csx.svg?style=flat)](https://www.nuget.org/packages/simple-targets-csx) [![Build status](https://ci.appveyor.com/api/projects/status/cmkx89k0sj0h3ebw/branch/master?svg=true)](https://ci.appveyor.com/project/adamralph/simple-targets-csx/branch/master)

A minimalist C# script library for writing targets for building, analysing, testing, packaging, deploying, etc. and running them using any runner which supports the "standard" C# script dialect (as defined by [csi.exe](https://msdn.microsoft.com/en-us/magazine/mt614271.aspx)).

In no way restricted to writing targets related to .NET projects.

### Quickstart

* Install (or download and unzip) the [NuGet package](https://www.nuget.org/packages/simple-targets-csx).
* Create a C# script named `build.csx` and add the following code:
```C#
#load "packages/simple-targets-csx.5.0.0/simple-targets.csx" // change the path as required

using static SimpleTargets;

var targets = new TargetDictionary();

targets.Add("default", () => Console.WriteLine("Hello, world!"));

Run(Args, targets);
```
* Run `"%ProgramFiles(x86)%\MSBuild\14.0\Bin\csi.exe" .\build.csx` (or use one of the other runners listed below).

### Usage

| Action                        | Command                                                                             |
|-------------------------------|-------------------------------------------------------------------------------------|
| Run a specific target         | `{runner} .\build.csx targetname`                                                   |
| Run multiple targets          | `{runner} .\build.csx target1name target2name`                                      |
| List targets                  | `{runner} .\build.csx -T`                                                           |
| List targets and dependencies | `{runner} .\build.csx -D`                                                           |
| Dry run                       | Append `-n`, e.g. `{runner} .\build.csx -n` or `{runner} .\build.csx targetname -n` |
| Show full usage details       | `{runner} .\build.csx targetname -?`                                                |

### Runners

* [csi.exe](https://msdn.microsoft.com/en-us/magazine/mt614271.aspx) - Microsoft's "C# REPL Command-Line Interface". This is bundled with MSBuild 14 (and Visual Studio 2015) onwards so it's a good choice if you are using simple-targets-csx to write a build script for a .NET project, since the project will already likely have a dependency on MSBuild.
* [Dude](https://github.com/adamralph/dude) - the portable C# script runner (csi.exe conveniently repackaged as a single self-contained exe). This is a good choice if you don't want or need to have a dependency on MSBuild. `dude.exe` can easily be downloaded and cached by a bootstrap command (similar to `NuGet.exe` in the example below).

Other C# script runners such as [dotnet-script](https://github.com/filipw/dotnet-script) should also work.

### Examples

xBehave.net uses a [bootstrap command](https://github.com/xbehave/xbehave.net/blob/dev/build.cmd) to

* Download and cache `NuGet.exe`
* Restore NuGet packages (including simple-targets-csx)
* Execute its [build script](https://github.com/xbehave/xbehave.net/blob/dev/build.csx) using csi.exe

---

<sub>[Target](https://thenounproject.com/term/target/345443) by [Franck Juncker](https://thenounproject.com/franckjuncker/) from [the Noun Project](https://thenounproject.com/).</sub>
