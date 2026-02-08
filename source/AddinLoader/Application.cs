using AddinLoader.Commands;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
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
                Utils.CreateDefaultButton("Type Renamer", assemblyPath, typeof(TypeRenamer).FullName),
                Utils.CreateDefaultButton("Fix Constraints", assemblyPath, typeof(FixConstraints).FullName),
                Utils.CreateDefaultButton("ReValue", assemblyPath, typeof(ReValue).FullName)
            );

            // Column 2
            panel.AddStackedItems(
                Utils.CreateDefaultButton("CAD Detective", assemblyPath, typeof(CADDetective).FullName),
                Utils.CreateDefaultButton("Crop Reg", assemblyPath, typeof(CropReg).FullName),
                Utils.CreateDefaultButton("Find Scheds Legends", assemblyPath, typeof(FindScheds).FullName)

            );

            // Column 3
            panel.AddStackedItems(
                Utils.CreateDefaultButton("Set By Index", assemblyPath, typeof(SetByIndex).FullName),
                Utils.CreateDefaultButton("Orient Box To Face", assemblyPath, typeof(OrientBox).FullName),
                Utils.CreateDefaultButton("Toolbox", assemblyPath, typeof(Toolbox).FullName)

            );



            // REGION Keybind recommended
            PulldownButton keyPullDown = Utils.CreateDefaultPulldown("Key Binds", panel);

            keyPullDown.AddPushButton(Utils.CreateDefaultButton("Toggle Point Cloud", assemblyPath, typeof(TogglePC).FullName, false));
            keyPullDown.AddPushButton(Utils.CreateDefaultButton("Point Cloud Normals", assemblyPath, typeof(PCNormals).FullName, false));
            keyPullDown.AddPushButton(Utils.CreateDefaultButton("Element View Range", assemblyPath, typeof(Evr).FullName,false));

            // ENDREGION

            // REGION Minimal Tools

            PulldownButton oneClickPullDown = Utils.CreateDefaultPulldown("Minimal Tools", panel);

            oneClickPullDown.AddPushButton(Utils.CreateDefaultButton("Match Grid Extents", assemblyPath, typeof(MatchGrids).FullName, false));
            oneClickPullDown.AddPushButton(Utils.CreateDefaultButton("Smart Grid Bubbles", assemblyPath, typeof(SmartBubbles).FullName, false));

            // ENDREGION

        }
    }
}