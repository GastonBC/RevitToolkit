using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Windows;

namespace Toolbox.CopyViewsWindow
{
    public partial class CopyViewsWindow : Window
    {
        public void CopyDetail(Document doc, IEnumerable<View> ViewsList)
        {
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Duplicate Views");
                foreach (View view in ViewsList)
                {

                    ElementId dupedViewId = view.Duplicate(ViewDuplicateOption.WithDetailing);
                    View dupedView = doc.GetElement(dupedViewId) as View;

                    string p = dupedView.Name; // TODO do something when view name repeats
                }
                t.Commit();
            }
        }
    }
}

