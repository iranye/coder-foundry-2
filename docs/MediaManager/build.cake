///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////


Task("Clean")
    // .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    // CleanDirectory($"./pub/{configuration}");
});

Task("Version")
    .IsDependentOn("Clean")
    .Does(() => {
		var propsFile = "./Directory.Build.props";
		var readedVersion = XmlPeek(propsFile, "//Version");
		var currentVersion = new Version(readedVersion);

		var semVersion = new Version(currentVersion.Major, currentVersion.Minor, currentVersion.Build + 1);
		var version = semVersion.ToString();

		XmlPoke(propsFile, "//Version", version);
});

Task("Publish")
    .IsDependentOn("Build")
	.Does(() => {
		DotNetPublish(".", new DotNetPublishSettings
		{
			 Framework = "net6.0-windows",
			 Configuration = configuration,
			 OutputDirectory = ($"./pub/{configuration}"),
		});	
});

Task("Build")
//    .IsDependentOn("Clean")
    .IsDependentOn("Version")
    .Does(() =>
{
    DotNetBuild("./MediaManager.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Default")
    .IsDependentOn("Clean");

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetTest("./MediaManager.sln", new DotNetTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

// TO PUBLISH W/ CLEAN, RUN:
// dotnet cake --target="Publish" --rebuild
