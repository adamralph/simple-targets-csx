:: the windows shell, so amazing

:: options
@echo Off
cd %~dp0
setlocal

:: set tool versions
set NUGET_VERSION=4.3.0
set MSBUILD_VERSION=15
set CSI_VERSION=2.3.1

:: determine cache dir
set NUGET_CACHE_DIR=%LocalAppData%\.nuget\v%NUGET_VERSION%
set NUGET_LOCAL_DIR=.nuget\v%NUGET_VERSION%

:: download nuget to cache dir
set NUGET_URL=https://dist.nuget.org/win-x86-commandline/v%NUGET_VERSION%/NuGet.exe
if not exist %NUGET_CACHE_DIR%\NuGet.exe (
  if not exist %NUGET_CACHE_DIR% mkdir %NUGET_CACHE_DIR%
  echo Downloading '%NUGET_URL%'' to '%NUGET_CACHE_DIR%\NuGet.exe'...
  @powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest '%NUGET_URL%' -OutFile '%NUGET_CACHE_DIR%\NuGet.exe'"
)

:: copy nuget locally
if not exist %NUGET_LOCAL_DIR%\NuGet.exe (
  if not exist %NUGET_LOCAL_DIR% mkdir %NUGET_LOCAL_DIR%
  copy %NUGET_CACHE_DIR%\NuGet.exe %NUGET_LOCAL_DIR%\NuGet.exe > nul
)

:: restore packages
echo Restoring NuGet packages...
%NUGET_LOCAL_DIR%\NuGet.exe restore .\packages.config -PackagesDirectory ./packages -MSBuildVersion %MSBUILD_VERSION% -Verbosity quiet

:: build package
if not exist artifacts mkdir artifacts
%NUGET_LOCAL_DIR%\NuGet.exe pack src/simple-targets-csx.nuspec -OutputDirectory artifacts

:: prepare package for testing
echo Preparing package for testing...
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "If (Test-Path .\artifacts\files\) { Remove-Item .\artifacts\files\ -Recurse -Force }"
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "Get-ChildItem .\artifacts\ -Filter *.nupkg | Select-Object -First 1 | foreach { Copy-Item $_.FullName .\artifacts\files.zip -Force }"
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "Expand-Archive .\artifacts\files.zip -DestinationPath .\artifacts\files"

:: run tests
echo Running tests...

set RUNNER=".\packages\Microsoft.Net.Compilers.%CSI_VERSION%\tools\csi.exe"

@echo On

%RUNNER% .\artifacts\files\internal\runner.csx || goto :error
%RUNNER% .\artifacts\files\internal\target-runner.csx || goto :error
%RUNNER% .\artifacts\files\internal\util.csx || goto :error
%RUNNER% .\artifacts\files\simple-targets-target.csx || goto :error
%RUNNER% .\artifacts\files\simple-targets.csx || goto :error

%RUNNER% .\tests\test.csx -? || goto :error
%RUNNER% .\tests\test.csx -T || goto :error
%RUNNER% .\tests\test.csx -D || goto :error
%RUNNER% .\tests\test.csx -n || goto :error
%RUNNER% .\tests\test.csx || goto :error
%RUNNER% .\tests\test.csx "hell""o" || goto :error

%RUNNER% .\tests\quickstart.csx || goto :error

%RUNNER% .\tests\double-dependency.csx || goto :error

%RUNNER% .\tests\check-targets-up-front.csx build notarealtarget || goto :error
%RUNNER% .\tests\check-targets-up-front-two-bad-targets.csx what2 build what1 || goto :error

@echo Off

:: exit
goto :EOF
:error
@echo Off
exit /b %errorlevel%
