using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;

namespace WallConstraints.Commands
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

            // Collect all Level elements present in the document
            FilteredElementCollector levelCollector = new FilteredElementCollector(doc);
            IEnumerable<Level> allLevels = levelCollector.OfClass(typeof(Level)).Cast<Level>();


            var selection = uidoc.Selection.GetElementIds();

            if (selection.Count == 0) return;


            using (Transaction t = new Transaction(doc, "Adjust Level to Closest Level"))
            {
                t.Start();

                foreach (ElementId elemId in selection)
                {
                    Element SelectedElem = doc.GetElement(elemId);

                    switch (SelectedElem.Category.BuiltInCategory)
                    {
                        case BuiltInCategory.OST_Walls:
                            DoubleLevelAdjust(doc, (Wall)SelectedElem, allLevels,
                                BuiltInParameter.WALL_BASE_CONSTRAINT,
                                BuiltInParameter.WALL_BASE_OFFSET,
                                BuiltInParameter.WALL_HEIGHT_TYPE,
                                BuiltInParameter.WALL_TOP_OFFSET,

                                BuiltInParameter.WALL_USER_HEIGHT_PARAM);

                            break;

                        // BUG: object set to null or something not doing anything
                        case BuiltInCategory.OST_StructuralColumns:
                            DoubleLevelAdjust(doc, SelectedElem, allLevels,
                                BuiltInParameter.FAMILY_BASE_LEVEL_PARAM,
                                BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM,
                                BuiltInParameter.FAMILY_TOP_LEVEL_PARAM,
                                BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM,

                                null);
                            break;

                        // BUG: object set to null or something, not doing anything
                        case BuiltInCategory.OST_Columns:
                            DoubleLevelAdjust(doc, SelectedElem, allLevels,
                                BuiltInParameter.FAMILY_BASE_LEVEL_PARAM,
                                BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM,
                                BuiltInParameter.FAMILY_TOP_LEVEL_PARAM,
                                BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM,

                                null);
                            break;


                        // Beams have one level, and two offsets (start and end). Figure it out later
                        //case BuiltInCategory.OST_StructuralFraming:
                        //    OneLevelAdjust(doc, SelectedElem, allLevels, BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM, BuiltInParameter.STRUCTURAL_REFERENCE_LEVEL_ELEVATION);
                        //    break;

                        case BuiltInCategory.OST_Floors:
                            OneLevelAdjust(doc, SelectedElem, allLevels, BuiltInParameter.LEVEL_PARAM, BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM);
                            break;
                        case BuiltInCategory.OST_Ceilings:
                            OneLevelAdjust(doc, SelectedElem, allLevels, BuiltInParameter.LEVEL_PARAM, BuiltInParameter.CEILING_HEIGHTABOVELEVEL_PARAM);
                            break;
                        case BuiltInCategory.OST_StairsRailing:
                            OneLevelAdjust(doc, SelectedElem, allLevels, BuiltInParameter.STAIRS_RAILING_BASE_LEVEL_PARAM, BuiltInParameter.STAIRS_RAILING_HEIGHT_OFFSET);
                            break;
                        case BuiltInCategory.OST_Roofs:
                            OneLevelAdjust(doc, SelectedElem, allLevels, BuiltInParameter.ROOF_BASE_LEVEL_PARAM, BuiltInParameter.ROOF_LEVEL_OFFSET_PARAM);
                            break;

                        default:
                            OneLevelAdjust(doc, SelectedElem, allLevels, BuiltInParameter.FAMILY_LEVEL_PARAM, BuiltInParameter.INSTANCE_ELEVATION_PARAM);
                            break;
                    }
                }

                t.Commit();
            }
        }

        // Most elements are defined by one level, but the parameter used is different on each
        private static void OneLevelAdjust(Document doc, Element elem, IEnumerable<Level> allLevels, BuiltInParameter base_builtin, BuiltInParameter offset_builtin)
        {
            Parameter base_level_param = elem.get_Parameter(base_builtin);
            Parameter offset_param = elem.get_Parameter(offset_builtin);

            Level base_level = doc.GetElement(base_level_param.AsElementId()) as Level;


            double elemZ = base_level.Elevation + offset_param.AsDouble();

            Level newBaseLevel = GetClosestLevel(allLevels, elemZ);

            double newBaseOffset = elemZ - newBaseLevel.Elevation;

            base_level_param.Set(newBaseLevel.Id);
            offset_param.Set(newBaseOffset);
        }


        private static void DoubleLevelAdjust(
            Document doc,
            Element elem,
            IEnumerable<Level> allLevels,
            BuiltInParameter base_builtin,
            BuiltInParameter offset_base_builtin,
            BuiltInParameter top_builtin,
            BuiltInParameter offset_top_builtin,
            BuiltInParameter? unconnected_builtin
            )
        {
            Parameter base_level_param = elem.get_Parameter(base_builtin);
            Parameter top_level_param = elem.get_Parameter(top_builtin);

            Parameter base_offset_param = elem.get_Parameter(offset_base_builtin);
            Parameter top_offset_param = elem.get_Parameter(offset_top_builtin);

            ElementId baseLevelId = base_level_param.AsElementId();
            ElementId topLevelId = top_level_param.AsElementId();


            Level base_level = doc.GetElement(base_level_param.AsElementId()) as Level;
            Level top_level;

            double baseZ = 0;
            double topZ = 0;


            // Case 1: Wall has top level. Column has top level.
            // Column shouldnt ever jump to the other part of the if statement
            if (topLevelId != ElementId.InvalidElementId)
            {
                top_level = doc.GetElement(top_level_param.AsElementId()) as Level;

                // Get the current absolute Z-coordinates for the wall's base and top.
                baseZ = base_level.Elevation + base_offset_param.AsDouble();
                topZ = top_level.Elevation + top_offset_param.AsDouble();
            }

            // Case 2: top_level_param has no level assigned. Wall has unconnected height
            else
            {
                BuiltInParameter builtin = unconnected_builtin.Value;

                Parameter unconnected_param = elem.get_Parameter(builtin);

                baseZ = base_level.Elevation + base_offset_param.AsDouble();
                topZ = baseZ + unconnected_param.AsDouble();
            }


            Level newTopLevel = GetClosestLevel(allLevels, topZ);
            Level newBaseLevel = GetClosestLevel(allLevels, baseZ);

            double newBaseOffset = baseZ - newBaseLevel.Elevation;
            double newTopOffset = topZ - newTopLevel.Elevation;



            base_level_param.Set(newBaseLevel.Id);
            top_level_param.Set(newTopLevel.Id);

            base_offset_param.Set(newBaseOffset);
            top_offset_param.Set(newTopOffset);
        }

        private static Level GetClosestLevel(IEnumerable<Level> allLevels, double targetElevation)
        {
            Level closestLevel = null;
            double minDifference = double.MaxValue;

            foreach (Level level in allLevels)
            {
                double currentDifference = Math.Abs(targetElevation - level.Elevation);

                // If this level's elevation is closer to the target elevation than any previously found level,
                // update closestLevel and minDifference.
                if (currentDifference < minDifference)
                {
                    minDifference = currentDifference;
                    closestLevel = level;
                }
            }
            return closestLevel;

        }

    }
}