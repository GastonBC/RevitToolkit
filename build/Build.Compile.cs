using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

sealed partial class Build
{
    /// <summary>
    ///     Compile all solution configurations.
    /// </summary>
    Target Compile => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            // Filter to only include configurations ending in R24, R25, or R26
            var targetConfigurations = GlobBuildConfigurations()
                .Where(x => x.Contains("R24") || x.Contains("R25") || x.Contains("R26"));

            foreach (var configuration in targetConfigurations)
            {
                DotNetBuild(settings => settings
                    .SetProjectFile(Solution)
                    .SetConfiguration(configuration)
                    .SetVersion(ReleaseVersionNumber)
                    .SetVerbosity(DotNetVerbosity.minimal));
            }
        });
}