using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Windows;
using Utilities;

/* TODO:
Duplicate sheets with all the same parameters
*/

namespace Toolbox.CopyViewsWindow
{

    public partial class CopyViewsWindow : Window
    {
        UIDocument uidoc;
        Document doc;
        IEnumerable<View> ViewsList;
        ViewSheet SampleSheet;

        public CopyViewsWindow(UIDocument uid, IEnumerable<View> _ViewsList, ViewSheet _SampleSheet)
        {
            uidoc = uid;
            doc = uidoc.Document;
            ViewsList = _ViewsList;
            SampleSheet = _SampleSheet;
            InitializeComponent();

        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Close();

            bool pass = false;
            if (Duplicate.IsChecked == true)
            {
                CopyDuplicate(doc, ViewsList);
                pass = true;
            }
            else if (Detailing.IsChecked == true)
            {
                CopyDetail(doc, ViewsList);
                pass = true;
            }
            else if (Dependent.IsChecked == true)
            {
                CopyDependent(doc, ViewsList);
                pass = true;
            }
            else
            {
                Utils.SimpleDialog("Select an option");
                pass = false;
            }

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Duplicate sheets");
                // now duplicate sheets, only if something is checked
                if (pass)
                {

                    ViewSheet newSheet = ViewSheet.Create(doc, SampleSheet.GetTypeId());

                    foreach (Parameter param in newSheet.Parameters)
                    {
                        Parameter SampleParam = SampleSheet.LookupParameter(param.Definition.Name);
                        try
                        {
                            param.SetValueString(SampleParam.AsString());
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                t.Commit();
            }
        }
    }
}
