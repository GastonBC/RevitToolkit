using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.PointClouds;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using System;

namespace PointCloudNormals.Commands
{
    /// <summary>
    ///     External command entry point
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class StartupCommand : ExternalCommand
    {
        public override void Execute()
        {
            UIDocument uidoc = this.UiDocument;
            Document doc = uidoc.Document;
            View view = doc.ActiveView;

            using (Transaction t = new Transaction(doc, "Set point clouds to normal mode"))
            {
                t.Start();

                // Example: hide all point clouds in the view
                FilteredElementCollector collector = new FilteredElementCollector(doc)
                   .OfClass(typeof(PointCloudInstance)).WhereElementIsNotElementType();

                PointCloudOverrideSettings pcSettings = new PointCloudOverrideSettings();
                pcSettings.ColorMode = PointCloudColorMode.Normals;

                PointCloudOverrides pcOverrides = view.GetPointCloudOverrides();

                foreach (Element pc in collector)
                {
                    pcOverrides.SetPointCloudScanOverrideSettings(pc.Id, pcSettings);
                }
            }
        }
    }
}