using System;
using Mono.Addins;
using Mono.Addins.Description;

[assembly: Addin(
    "MonoPresence",
    Namespace = "MonoPresence",
    Version = "1.0"
)]

[assembly: AddinName("MonoPresence")]
[assembly: AddinCategory("IDE extensions")]
[assembly: AddinDescription("Enables Discord Rich Presence which show your current project")]
[assembly: AddinAuthor("Ryhon")]

[assembly: AddinDependency("::MonoDevelop.Core", MonoDevelop.BuildInfo.Version)]
[assembly: AddinDependency("::MonoDevelop.Ide", MonoDevelop.BuildInfo.Version)]