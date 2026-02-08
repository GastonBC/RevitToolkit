using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using System.Windows.Controls.Primitives;

namespace TypeRenamer.Commands
{

    // BUG: Counter for same type of something should restart after each family


    /// <summary>
    ///     External command entry point
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class StartupCommand : ExternalCommand
    {

        // This function defines the number to be attached to the name type to make sure there are no duplicates
        private Dictionary<T, string> GetRenamedElements<T>(List<T> elements, Func<T, string> nameGenerator) where T : Element
        {
            var typesToRename = new Dictionary<T, string>();
            var nameCounts = new Dictionary<string, int>();

            foreach (T element in elements)
            {
                string newName = nameGenerator(element);

                if (nameCounts.ContainsKey(newName))
                {
                    int count = nameCounts[newName];
                    nameCounts[newName] = count + 1;
                    typesToRename[element] = newName + " Type " + count.ToString();
                }
                else
                {
                    nameCounts[newName] = 1;
                    typesToRename[element] = newName;
                }
            }
            return typesToRename;
        }

        // This function performs the actual renaming
        private void RenameTypes<T>(Document doc, List<T> types, Func<T, string> nameGenerator) where T : Element
        {
            Dictionary<T, string> typesToRename = GetRenamedElements(types, nameGenerator);

            TaskDialog.Show("Revit", "Found " + typesToRename.Count.ToString() + " types to rename.");

            using (Transaction t = new Transaction(doc, "Adjust type names"))
            {
                t.Start();
                foreach (KeyValuePair<T, string> entry in typesToRename)
                {
                    T elementType = entry.Key;
                    string finalName = entry.Value;
                    if (elementType.Name != finalName)
                    {
                        elementType.Name = finalName;
                    }
                }
                t.Commit();
            }
        }

        private void PanelNaming(Document doc, ICollection<ElementId> selected)
        {
            // Get the base collection (either the whole doc or the selected IDs)
            IEnumerable<Element> source = (selected.Count == 0)
                ? (IEnumerable<Element>)new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
                : selected.Select(id => doc.GetElement(id));

            List<PanelType> elemTypes = new List<PanelType>();

            elemTypes = source
            .Cast<PanelType>()
            .ToList();

            // The naming is specific to each type, so we define the logic here as an inline function
            Func<PanelType, string> nameGenerator = familyType =>
            {
                string thickParam = familyType.get_Parameter(BuiltInParameter.CURTAIN_WALL_SYSPANEL_THICKNESS).AsValueString();
                string offsetParam = familyType.get_Parameter(BuiltInParameter.CURTAIN_WALL_SYSPANEL_OFFSET).AsValueString();

                return "T" + thickParam + " (OF" + offsetParam + ")";
            };

            RenameTypes(doc, elemTypes, nameGenerator);
        }

        private void WindowNaming(Document doc, ICollection<ElementId> selected)
        {

            // Get the base collection (either the whole doc or the selected IDs)
            IEnumerable<Element> source = (selected.Count == 0)
                ? (IEnumerable<Element>)new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
                : selected.Select(id => doc.GetElement(id));

            List<FamilySymbol> elemTypes = new List<FamilySymbol>();

            elemTypes = source
                .Cast<FamilySymbol>()
                .Where(f => f.Family.FamilyCategory.BuiltInCategory == BuiltInCategory.OST_Windows)
                .ToList();

            // The naming is specific to each type, so we define the logic here as an inline function
            Func<FamilySymbol, string> nameGenerator = familyType =>
            {

                string widthParam = familyType.get_Parameter(BuiltInParameter.DOOR_WIDTH).AsValueString();
                string heightParam = familyType.get_Parameter(BuiltInParameter.GENERIC_HEIGHT).AsValueString();

                return "H" + heightParam + " x W" + widthParam;

            };

            RenameTypes(doc, elemTypes, nameGenerator);
        }

        private void DoorNaming(Document doc, ICollection<ElementId> selected)
        {
            // 1. Get the base collection (either the whole doc or the selected IDs)
            IEnumerable<Element> source = (selected.Count == 0)
                ? (IEnumerable<Element>)new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
                : selected.Select(id => doc.GetElement(id));

            // 2. Apply common filters once
            List<FamilySymbol> elemTypes = source
                .OfType<FamilySymbol>()
                .Where(f => f.Category.BuiltInCategory == BuiltInCategory.OST_Doors)
                .ToList();

            // Define a function to generate the base name for a type
            Func<FamilySymbol, string> nameGenerator = familyType =>
            {

                string widthParam = familyType.get_Parameter(BuiltInParameter.DOOR_WIDTH).AsValueString();
                string heightParam = familyType.get_Parameter(BuiltInParameter.GENERIC_HEIGHT).AsValueString();
                string thickParam = familyType.get_Parameter(BuiltInParameter.WINDOW_THICKNESS).AsValueString();

                return "H" + heightParam + " x W" + widthParam + " x T" + thickParam;

            };

            RenameTypes(doc, elemTypes, nameGenerator);

        }

        private void MullionNaming(Document doc, ICollection<ElementId> selected)
        {

            // 1. Get the base collection (either the whole doc or the selected IDs)
            IEnumerable<Element> source = (selected.Count == 0)
                ? (IEnumerable<Element>)new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
                : selected.Select(id => doc.GetElement(id));

            // 2. Apply common filters once
            List<MullionType> elemTypes = source
                .OfType<MullionType>()
                .ToList();

            // Define a function to generate the base name for a MullionType
            Func<MullionType, string> nameGenerator = familyType =>
            {
                if (!familyType.FamilyName.Contains("Rectangular")) return familyType.Name;

                string width1Param = familyType.get_Parameter(BuiltInParameter.RECT_MULLION_WIDTH1).AsValueString();
                string width2Param = familyType.get_Parameter(BuiltInParameter.RECT_MULLION_WIDTH2).AsValueString();
                int width = int.Parse(width1Param) + int.Parse(width2Param);

                string depthParam = familyType.get_Parameter(BuiltInParameter.RECT_MULLION_THICK).AsValueString();
                string offParam = familyType.get_Parameter(BuiltInParameter.MULLION_OFFSET).AsValueString();

                if (offParam == "0")
                {
                    return "W" + width.ToString() + " x D" + depthParam;
                }
                else
                {
                    return "W" + width.ToString() + " x D" + depthParam + " (OF" + offParam + ")";
                }
            };

            RenameTypes(doc, elemTypes, nameGenerator);
        }

        public override void Execute()
        {

            UIDocument uidoc = this.UiDocument;
            Document doc = uidoc.Document;

            ICollection<ElementId> selection = uidoc.Selection.GetElementIds();

            // Create the task dialog instance
            TaskDialog td = new TaskDialog("Revit Naming Tool");

            td.MainInstruction = "Select an element type to rename:";

            td.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Windows");
            td.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Doors");
            td.AddCommandLink(TaskDialogCommandLinkId.CommandLink3, "Mullions");
            td.AddCommandLink(TaskDialogCommandLinkId.CommandLink4, "Panels");

            TaskDialogResult result = td.Show();

            switch (result)
            {
                case TaskDialogResult.CommandLink1:
                    WindowNaming(doc, selection);
                    break;
                case TaskDialogResult.CommandLink2:
                    DoorNaming(doc, selection);
                    break;
                case TaskDialogResult.CommandLink3:
                    MullionNaming(doc, selection);
                    break;
                case TaskDialogResult.CommandLink4:
                    PanelNaming(doc, selection);
                    break;
            }

        }
    }
}
