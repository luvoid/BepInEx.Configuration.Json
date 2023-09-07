for /f "usebackq delims=|" %%f in (`dir /S /b BepInEx.Configuration.Json\bin\Release\LuVoid.*.nupkg`) do (
	nuget add %%f -Source %USERPROFILE%/.nuget/packages -Expand
)
