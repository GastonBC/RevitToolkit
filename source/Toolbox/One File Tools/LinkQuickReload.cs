using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using System.Windows;
using Utilities;

namespace Toolbox
{
    public partial class MainWindow : Window
    {
        private void LinkQuickReload_Click(object sender, RoutedEventArgs e)
        {
            Close();
            string ElemsReloaded = "";

            ICollection<ElementId> selection = uidoc.Selection.GetElementIds();

            // Reload selection
            if (selection.Count != 0)
            {
                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Quick link reload");
                    foreach (ElementId elemId in selection)
                    {
                        Element elem = doc.GetElement(elemId);
                        if (elem is ImportInstance)
                        {
                            ImportInstance elemLink = elem as ImportInstance;
                            if (elemLink.IsLinked)
                            {
                                CADLinkType linkType = doc.GetElement(elemLink.GetTypeId()) as CADLinkType;
                                linkType.Reload();
                                ElemsReloaded += linkType.Name + "\n";
                            }
                        }
                    }
                    t.Commit();
                    Utils.SimpleDialog("Links reloaded", ElemsReloaded);
                }
            }

            // Reload all links
            else
            {
                if (Utils.ConfirmDialog("Reload all links?", "This operation may take a few minutes"))
                {
                    FilteredElementCollector linkCol = new FilteredElementCollector(doc);
                    linkCol.OfClass(typeof(ImportInstance))
                           .ToElements()
                           .Cast<ImportInstance>();

                    using (Transaction t = new Transaction(doc))
                    {
                        t.Start("Quick link reload");
                        foreach (ImportInstance elemLink in linkCol)
                        {
                            if (elemLink.IsLinked)
                            {
                                CADLinkType linkType = doc.GetElement(elemLink.GetTypeId()) as CADLinkType;
                                linkType.Reload();
                                ElemsReloaded += linkType.Name + "\n";
                            }
                        }
                        t.Commit();
                        Utils.SimpleDialog("Links reloaded", ElemsReloaded);
                    }
                }
            }
        }

    }
}

