using AddinLoader.Commands;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using Utilities;
using Globals = Utilities.GlobalVars;

namespace AddinLoader
{

    [UsedImplicitly]
    public class Application : ExternalApplication
    {
        public override void OnStartup()
        {
            CreateRibbon();
        }

        private void CreateRibbon()
        {
            RibbonPanel selectObjectsPanel = Application.CreatePanel("Selection Based", Globals.TAB_NAME);
            RibbonPanel guiBasedPanel = Application.CreatePanel("Interface Based", Globals.TAB_NAME);
            RibbonPanel otherPanel = Application.CreatePanel("Other", Globals.TAB_NAME);


            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // Column 1
            selectObjectsPanel.AddStackedItems(
                Utils.CreateDefaultButton("Type Renamer", assemblyPath, typeof(TypeRenamer).FullName),
                Utils.CreateDefaultButton("Fix Constraints", assemblyPath, typeof(FixElementConstraints).FullName),
                Utils.CreateDefaultButton("ReValue", assemblyPath, typeof(ReValue).FullName)
            );

            // Column 2
            guiBasedPanel.AddStackedItems(
                Utils.CreateDefaultButton("CAD Detective", assemblyPath, typeof(CADDetective).FullName),
                Utils.CreateDefaultButton("Draw Crop Regions", assemblyPath, typeof(CropReg).FullName),
                Utils.CreateDefaultButton("Find Scheds Legends", assemblyPath, typeof(FindSchedsLegends).FullName)

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

            keyPullDown.AddPushButton(new PushButtonData("btnTogglePC", "Toggle Point Cloud", assemblyPath, typeof(TogglePointCloud).FullName));
            keyPullDown.AddPushButton(new PushButtonData("btnPCNormals", "Point Cloud Normals", assemblyPath, typeof(PointCloudNormals).FullName));
            keyPullDown.AddPushButton(new PushButtonData("btnEvr", "Element View Range", assemblyPath, typeof(ElementViewRange).FullName));
            keyPullDown.AddPushButton(new PushButtonData("btnOrientBox", "Orient Box To Face", assemblyPath, typeof(OrientBoxToFace).FullName));


            oneClickPullDown.AddPushButton(new PushButtonData("btnMatchGrids", "Match Grid Extents", assemblyPath, typeof(MatchGridExtents).FullName));
            oneClickPullDown.AddPushButton(new PushButtonData("btnSmartBubbles", "Smart Grid Bubbles", assemblyPath, typeof(SmartGridBubbles).FullName));
            oneClickPullDown.AddPushButton(new PushButtonData("dimGrids", "Auto Dimension\nGrids", assemblyPath, typeof(AutoDimGrids).FullName));


        }
    }
}