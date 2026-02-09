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
            RibbonPanel selectObjectsPanel = Application.CreatePanel("Selection Based", GlobalVars.TAB_NAME);
            RibbonPanel guiBasedPanel = Application.CreatePanel("Interface Based", GlobalVars.TAB_NAME);
            RibbonPanel otherPanel = Application.CreatePanel("Other", GlobalVars.TAB_NAME);


            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // Column 1
            selectObjectsPanel.AddStackedItems(
                Utils.CreateDefaultButton("Type Renamer", assemblyPath, typeof(TypeRenamer).FullName),
                Utils.CreateDefaultButton("Fix Constraints", assemblyPath, typeof(FixConstraints).FullName),
                Utils.CreateDefaultButton("ReValue", assemblyPath, typeof(ReValue).FullName)
            );

            // Column 2
            guiBasedPanel.AddStackedItems(
                Utils.CreateDefaultButton("CAD Detective", assemblyPath, typeof(CADDetective).FullName),
                Utils.CreateDefaultButton("Draw Crop Regions", assemblyPath, typeof(CropReg).FullName),
                Utils.CreateDefaultButton("Find Scheds Legends", assemblyPath, typeof(FindScheds).FullName)

            );

            // Column 3
            guiBasedPanel.AddStackedItems(
                Utils.CreateDefaultButton("Set By Index", assemblyPath, typeof(SetByIndex).FullName),
                Utils.CreateDefaultButton("Toolbox", assemblyPath, typeof(Toolbox).FullName)
            );


            PulldownButtonData keyData = new PulldownButtonData("keyBinds", "Key Binds");
            PulldownButtonData oneClickData = new PulldownButtonData("minimalTools", "Minimal Tools");

            IList<RibbonItem> stackedItems = otherPanel.AddStackedItems(keyData, oneClickData);

            // Cast the resulting RibbonItems to PulldownButtons to populate them
            PulldownButton keyPullDown = stackedItems[0] as PulldownButton;
            PulldownButton oneClickPullDown = stackedItems[1] as PulldownButton;

            keyPullDown.AddPushButton(new PushButtonData("btnTogglePC", "Toggle Point Cloud", assemblyPath, typeof(TogglePC).FullName));
            keyPullDown.AddPushButton(new PushButtonData("btnPCNormals", "Point Cloud Normals", assemblyPath, typeof(PCNormals).FullName));
            keyPullDown.AddPushButton(new PushButtonData("btnEvr", "Element View Range", assemblyPath, typeof(Evr).FullName));
            keyPullDown.AddPushButton(new PushButtonData("btnOrientBox", "Orient Box To Face", assemblyPath, typeof(OrientBox).FullName));


            oneClickPullDown.AddPushButton(new PushButtonData("btnMatchGrids", "Match Grid Extents", assemblyPath, typeof(MatchGrids).FullName));
            oneClickPullDown.AddPushButton(new PushButtonData("btnSmartBubbles", "Smart Grid Bubbles", assemblyPath, typeof(SmartBubbles).FullName));


            /*
            // REGION Keybind recommended
            PulldownButton keyPullDown = Utils.CreateDefaultPulldown("Key Binds", otherPanel);

            keyPullDown.AddPushButton(Utils.CreateDefaultButton("Toggle Point Cloud", assemblyPath, typeof(TogglePC).FullName, false));
            keyPullDown.AddPushButton(Utils.CreateDefaultButton("Point Cloud Normals", assemblyPath, typeof(PCNormals).FullName, false));
            keyPullDown.AddPushButton(Utils.CreateDefaultButton("Element View Range", assemblyPath, typeof(Evr).FullName, false));
            keyPullDown.AddPushButton(Utils.CreateDefaultButton("Orient Box To Face", assemblyPath, typeof(OrientBox).FullName, false));


            // ENDREGION

            // REGION Minimal Tools

            PulldownButton oneClickPullDown = Utils.CreateDefaultPulldown("Minimal Tools", otherPanel);

            oneClickPullDown.AddPushButton(Utils.CreateDefaultButton("Match Grid Extents", assemblyPath, typeof(MatchGrids).FullName, false));
            oneClickPullDown.AddPushButton(Utils.CreateDefaultButton("Smart Grid Bubbles", assemblyPath, typeof(SmartBubbles).FullName, false));

            // ENDREGION
            */


        }
    }
}