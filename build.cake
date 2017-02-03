var target          = Argument("target", "Default");
var configuration   = Argument<string>("configuration", "Release");
var buildArtifacts      = Directory("./artifacts/packages");
var version = Argument<string>("targetversion", "beta" + (EnvironmentVariable("APPVEYOR_BUILD_NUMBER") ?? EnvironmentVariable("BUILD_BUILDNUMBER") ?? "0"));

Task("Clean")
    .Does(() =>
{
    CleanDirectories(new DirectoryPath[] { buildArtifacts });
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
	var projects = GetFiles("./**/project.json");

	foreach(var project in projects)
	{
        var settings = new DotNetCoreBuildSettings 
        {
            Configuration = configuration
            // Runtime = IsRunningOnWindows() ? null : "unix-x64"
        };

	    DotNetCoreBuild(project.GetDirectory().FullPath, settings); 
    }
});
Task("Default")
  .IsDependentOn("Build");

Task("Restore")
    .Does(() =>
{
    var settings = new DotNetCoreRestoreSettings
    {
        Sources = new [] { "https://api.nuget.org/v3/index.json" }
    };

    DotNetCoreRestore("./src", settings);
 
});


Task("Package")
.IsDependentOn("Build")
    .Does(() =>
{
     var settings = new DotNetCorePackSettings
     {
         Configuration = configuration,
         OutputDirectory = buildArtifacts,
		 VersionSuffix = version
     };
	 var projects = GetFiles("./**/project.json");
	
	foreach(var project in projects)
	{
      DotNetCorePack(project.GetDirectory().FullPath, settings);
	}
 
});
RunTarget(target);
