using AddinLoader.Commands;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using System.IO;
using System.Reflection;
using Utilities; 

namespace AddinLoader
{
    /// <summary>
    ///     Application entry point
    /// </summary>
    [UsedImplicitly]
    public class Application : ExternalApplication
    {
        public override void OnStartup()
        {
            CreateRibbon();
        }

        private void CreateRibbon()
        {
            RibbonPanel panel = Application.CreatePanel("Commands", "AddinLoader");


            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // Column 1
            panel.AddStackedItems(
                Utils.CreateButton("Type Renamer", assemblyPath, typeof(Invoke01).FullName),
                Utils.CreateButton("Fix Constraints", assemblyPath, typeof(Invoke02).FullName),
                Utils.CreateButton("ReValue", assemblyPath, typeof(Invoke03).FullName)
            );

            // Column 2
            panel.AddStackedItems(
                Utils.CreateButton("CAD Detective", assemblyPath, typeof(Invoke04).FullName),
                Utils.CreateButton("Crop Reg", assemblyPath, typeof(Invoke05).FullName),
                Utils.CreateButton("Element View Range", assemblyPath, typeof(Invoke06).FullName)
            );

            // Column 3
            panel.AddStackedItems(
                Utils.CreateButton("Find Scheds Legends", assemblyPath, typeof(Invoke07).FullName),
                Utils.CreateButton("Orient Box To Face", assemblyPath, typeof(Invoke08).FullName),
                Utils.CreateButton("Point Cloud Normals", assemblyPath, typeof(Invoke09).FullName)
            );

            // Column 4
            panel.AddStackedItems(
                Utils.CreateButton("ReValue Duplicate", assemblyPath, typeof(Invoke10).FullName),
                Utils.CreateButton("Set By Index", assemblyPath, typeof(Invoke11).FullName),
                Utils.CreateButton("Toggle Point Cloud", assemblyPath, typeof(Invoke12).FullName)
            );

            // Column 5
            panel.AddStackedItems(
                Utils.CreateButton("Toolbox", assemblyPath, typeof(Invoke13).FullName),
                Utils.CreateButton("Match Grid Extents", assemblyPath, typeof(Invoke14).FullName),
                Utils.CreateButton("Smart Grid Bubbles", assemblyPath, typeof(Invoke15).FullName)
            );


        }
    }
}