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
        private void HostedElems_Click(object sender, RoutedEventArgs e)
        {
            Close();
            ICollection<ElementId> selection = uidoc.Selection.GetElementIds();

            if (selection != null && selection.Count == 1 && doc.GetElement(selection.First()) is HostObject)
            {
                HostObject hostElem = doc.GetElement(selection.First()) as HostObject;
                IList<ElementId> insertions = hostElem.FindInserts(true, true, true, true);

                string dialogContent = "";

                foreach (ElementId id in insertions)
                {
                    dialogContent = dialogContent + doc.GetElement(id).Name + "\n";
                }

                dialogContent = dialogContent + "Final count: " + insertions.Count;

                Utils.SimpleDialog("Contents", dialogContent);
            }

            else
            {
                Utils.SimpleDialog("Select a host element", "");
            }
        }
    }
}
