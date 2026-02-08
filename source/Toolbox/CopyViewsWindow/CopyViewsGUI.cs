using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Windows;
using Utilities;

namespace Toolbox
{
    public partial class MainWindow : Window
    {
        // Unused
        private void SelectedViews_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();

                ICollection<ElementId> Selec = uidoc.Selection.GetElementIds();

                if (Selec.Count() != 0) // check if all elements are views
                {
                    ElementCategoryFilter viewsFilter = new ElementCategoryFilter(BuiltInCategory.OST_Views);
                    ElementCategoryFilter sheetsFilter = new ElementCategoryFilter(BuiltInCategory.OST_Sheets);


                    IEnumerable<View> ViewsCollector = new FilteredElementCollector(doc, Selec)
                    .WherePasses(viewsFilter)
                    .Cast<View>()
                    .Where(view => view.ViewType != ViewType.ThreeD); // check if all elements are views

                    ViewSheet SampSheet = new FilteredElementCollector(doc, Selec)
                        .WherePasses(sheetsFilter)
                        .Cast<ViewSheet>()
                        .FirstOrDefault(); // discard placeholders just in case


                    if (ViewsCollector.Count() != 0) // did any element pass the filters?
                    {
                        CopyViewsWindow.CopyViewsWindow Window = new CopyViewsWindow.CopyViewsWindow(uidoc, ViewsCollector, SampSheet);
                        Window.ShowDialog();
                    }
                    else
                    {
                        Utils.SimpleDialog("Select non-3d views", "");
                    }
                }
                else
                {
                    Utils.SimpleDialog("Select non-3d views", "");
                }
            }
            catch (Exception ex)
            {
                Utils.CatchDialog(ex);
            }
        }
    }
}


