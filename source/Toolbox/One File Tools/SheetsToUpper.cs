using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using System.Windows;


namespace Toolbox
{
    public partial class MainWindow : Window
    {
        private void SheetsToUpper_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder sheets print as blank pages, we don't want them
            FilteredElementCollector sheetList = new FilteredElementCollector(doc);
            sheetList.OfClass(typeof(ViewSheet))
                        .Cast<ViewSheet>();

            using (Transaction t = new Transaction(doc, "Schedule to uppercase"))
            {
                t.Start();
                foreach (ViewSheet sheet in sheetList)
                {
                    foreach (ElementId vpId in sheet.GetAllViewports())
                    {
                        Viewport vp = (Viewport)doc.GetElement(vpId);
                        View view = (View)doc.GetElement(vp.ViewId);


                        view.Name = view.Name.ToUpper();
                    }
                }
                t.Commit();
            }

        }
    }
}

