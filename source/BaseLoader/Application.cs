using BaseLoader.Commands;
using Autodesk.Revit.UI;
using System.Reflection;
using System;

namespace BaseLoader
{
    public class Application : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                CreateRibbon(application);
                return Result.Succeeded;
            }
            catch (Exception)
            {
                return Result.Failed;
            }
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private void CreateRibbon(UIControlledApplication application)
        {
            // Create the Tab explicitly
            string tabName = "BaseLoader";
            try { application.CreateRibbonTab(tabName); } catch { /* Tab already exists */ }

            // Create the Panel
            RibbonPanel panel = application.CreateRibbonPanel(tabName, "Commands");

            // Create the Button Data
            // We use the LocalPath logic to ensure the path isn't empty during proxy load
            string assemblyPath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;

            PushButtonData buttonData = new PushButtonData(
                "btnExecute",
                "Execute",
                assemblyPath,
                typeof(StartupCommand).FullName);

            panel.AddItem(buttonData);
        }
    }
}