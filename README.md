![Icon](/simple-targets-csharp.png)

# simple-targets-csharp

[![NuGet version](https://img.shields.io/nuget/v/simple-targets-csharp.svg?style=flat)](https://www.nuget.org/packages/simple-targets-csharp) [![Build status](https://ci.appveyor.com/api/projects/status/cmkx89k0sj0h3ebw/branch/master?svg=true)](https://ci.appveyor.com/project/adamralph/simple-targets-csharp/branch/master)

Simple target runner for use in C# scripts.

### Quickstart

* Install (or download and unzip) the [NuGet package](https://www.nuget.org/packages/simple-targets-csharp).
* Create a C# script named `build.csx` and add the following code:
```C#
#load "packages/simple-targets-csharp.2.0.0/simple-targets-csharp.csx" // change the path as required

var targets = new Dictionary<string, Target>();

targets.Add("default", new Target { Do = () => Console.WriteLine("Hello, world!"), });

Run(Args, targets);
```
* Run `"%ProgramFiles(x86)%\MSBuild\14.0\Bin\csi.exe" .\build.csx` (or use one of the other runners listed below).

### Runners

* [csi.exe](https://msdn.microsoft.com/en-us/magazine/mt614271.aspx) - Microsoft's "C# REPL Command-Line Interface". This is bundled with MSBuild 14 (and Visual Studio 2015) onwards so it's a good choice if you are using simple-targets-csharp to write a build script for a .NET project, since the project will already likely have a dependency on MSBuild.
* [Dude](https://github.com/adamralph/dude) - the portable C# script runner (csi.exe conveniently repackaged as a single self-contained exe). This is a good choice if you don't want or need to have a dependency on MSBuild. `dude.exe` can easily be downloaded and cached by a bootstrap command (simliar to `NuGet.exe` in the example below).

Other C# script runners such as [scriptcs](http://scriptcs.net/) should also work.

### Examples

xBehave.net uses a [bootstrap command](https://github.com/xbehave/xbehave.net/blob/dev/build.cmd) to

* Download and cache `NuGet.exe`
* Restore NuGet packages (including simple-targets-csharp)
* Execute it's [build script](https://github.com/xbehave/xbehave.net/blob/dev/build.csx) using csi.exe

---

<sub>[Target](https://thenounproject.com/term/target/345443) by [Franck Juncker](https://thenounproject.com/franckjuncker/) from [the Noun Project](https://thenounproject.com/).</sub>
