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

            RibbonButton typeRenamerbutton = panel.AddPushButton<Invoke01>("Type Renamer")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");

            RibbonButton fixConstraintsbutton = panel.AddPushButton<Invoke02>("Fix Constraints")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");


            RibbonButton reValuebutton = panel.AddPushButton<Invoke03>("ReValue")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");

            RibbonButton cadDetectiveButton = panel.AddPushButton<Invoke04>("CAD Detective")
    .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
    .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");

            RibbonButton cropRegButton = panel.AddPushButton<Invoke05>("Crop Reg")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");

            RibbonButton elementViewRangeButton = panel.AddPushButton<Invoke06>("Element View Range")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");

            RibbonButton findSchedsLegendsButton = panel.AddPushButton<Invoke07>("Find Scheds Legends")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");

            RibbonButton orientBoxToFace08Button = panel.AddPushButton<Invoke08>("Orient Box To Face")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");

            RibbonButton pointCloudNormals09Button = panel.AddPushButton<Invoke09>("Point Cloud Normals")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");

            RibbonButton reValueDuplicateButton = panel.AddPushButton<Invoke10>("ReValue Duplicate")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");

            RibbonButton setByIndexButton = panel.AddPushButton<Invoke11>("Set By Index")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");

            RibbonButton togglePointCloudButton = panel.AddPushButton<Invoke12>("Toggle Point Cloud")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");

            RibbonButton toolboxButton = panel.AddPushButton<Invoke13>("Toolbox")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");

            RibbonButton orientBoxToFace14Button = panel.AddPushButton<Invoke14>("Orient Box To Face")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");

            RibbonButton pointCloudNormals15Button = panel.AddPushButton<Invoke15>("Point Cloud Normals")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");

        }
    }
}