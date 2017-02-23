:: the windows shell, so amazing

:: options
@echo Off
cd %~dp0
setlocal

:: determine cache dir
set NUGET_CACHE_DIR=%LocalAppData%\.nuget\v3.4.4

:: download nuget to cache dir
set NUGET_URL=https://dist.nuget.org/win-x86-commandline/v3.4.4/NuGet.exe
if not exist %NUGET_CACHE_DIR%\NuGet.exe (
  if not exist %NUGET_CACHE_DIR% md %NUGET_CACHE_DIR%
  echo Downloading '%NUGET_URL%'' to '%NUGET_CACHE_DIR%\NuGet.exe'...
  @powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest '%NUGET_URL%' -OutFile '%NUGET_CACHE_DIR%\NuGet.exe'"
)

:: copy nuget locally
if not exist .nuget\NuGet.exe (
  if not exist .nuget md .nuget
  copy %NUGET_CACHE_DIR%\NuGet.exe .nuget\NuGet.exe > nul
)

:: build package
mkdir artifacts
.nuget\NuGet.exe pack src/simple-targets-csx.nuspec -OutputDirectory artifacts

:: run tests
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "If (Test-Path .\artifacts\files\) { Remove-Item .\artifacts\files\ -Recurse -Force }"
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "Get-ChildItem .\artifacts\ -Filter *.nupkg | Select-Object -First 1 | foreach { Copy-Item $_.FullName .\artifacts\files.zip -Force }"
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "Expand-Archive .\artifacts\files.zip -DestinationPath .\artifacts\files"
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\csi.exe" .\artifacts\files\internal\runner.csx || goto :error
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\csi.exe" .\artifacts\files\internal\target-runner.csx || goto :error
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\csi.exe" .\artifacts\files\internal\util.csx || goto :error
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\csi.exe" .\artifacts\files\simple-targets-target.csx || goto :error
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\csi.exe" .\artifacts\files\simple-targets.csx || goto :error
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\csi.exe" .\tests\test.csx -? || goto :error
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\csi.exe" .\tests\test.csx -T || goto :error
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\csi.exe" .\tests\test.csx -D || goto :error
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\csi.exe" .\tests\test.csx -n || goto :error
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\csi.exe" .\tests\test.csx || goto :error
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\csi.exe" .\tests\test.csx "hell""o" || goto :error
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\csi.exe" .\tests\quickstart.csx || goto :error

goto :EOF
:error
exit /b %errorlevel%
