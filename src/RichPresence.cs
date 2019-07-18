using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using System.IO;
using System.Linq;
using MonoDevelop.Ide.CustomTools;
using System.Threading;
using MonoDevelop.Core;
using DiscordRPC;
using Newtonsoft.Json.Serialization;

namespace MonoPresence
{
    public class StartupHandler : CommandHandler
    {
        protected override void Run()
        {
            Presence.Initialize();
        }
    }


    public static class Presence
    {
        public static DiscordRpcClient client;

        public static void Initialize()
        {
            //601092423107608598 - MonoDevelop
            //601449424077455443 - Xamarin Studio
            //601450055756415017 - Visual Studio for Mac

            client = new DiscordRpcClient("601092423107608598");

            //Connect to the RPC
            client.Initialize();

            //Set the rich presence
            client.SetPresence(new RichPresence()
            {
                State = "Idle",
                Assets = new Assets()
                {
                    LargeImageKey = "logo",
                    LargeImageText = "MonoDevelop",
                }
            });

            IdeApp.Workspace.SolutionLoaded += Workspace_SolutionLoaded;
            IdeApp.Workspace.SolutionUnloaded += Workspace_SolutionUnloaded;

            IdeApp.Workbench.ActiveDocumentChanged += Workbench_ActiveDocumentChanged;

            void Workspace_SolutionLoaded(object sender, MonoDevelop.Projects.SolutionEventArgs e)
            {
                var p = new RichPresence
                {
                    Details = "Idle",
                    State = "Developing " + e.Solution.Name,
                    Assets = new Assets()
                    {
                        LargeImageKey = "logo",
                        LargeImageText = e.Solution.Name,
                    }
                };

                client.SetPresence(p);
            }

            void Workspace_SolutionUnloaded(object sender, MonoDevelop.Projects.SolutionEventArgs e)
            {
                client.SetPresence(new RichPresence()
                {
                    State = "Idle",
                    Assets = new Assets()
                    {
                        LargeImageKey = "logo",
                        LargeImageText = "MonoDevelop",
                    }
                });
            }

            void Workbench_ActiveDocumentChanged(object sender, EventArgs e)
            {
                var p = new RichPresence();

                p.State = client.CurrentPresence.State;

                var doc = IdeApp.Workbench.ActiveDocument;

                if (doc != null)
                {
                    p.Details = new FileInfo(doc.Name).Name;
                    p.Timestamps = new Timestamps(DateTime.UtcNow);
                    p.Assets = new Assets()
                    {
                        LargeImageKey = "file",
                        LargeImageText = new FileInfo(doc.Name).Name
                    };

                    client.SetPresence(p);
                }
                else
                {
                    p.Details = "Idle";
                    p.Timestamps = null;
                    p.Assets = new Assets()
                    {
                        LargeImageKey = "logo",
                        LargeImageText = "MonoDevelop",
                    };

                    client.SetPresence(p);
                }
            }
        }
    }
}
