using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows;
using Toolbox.One_file_tools;
using System.Collections.Generic;
using System;
using Toolbox.MoveCatsToWorkset;
using Toolbox.TemplateReplace;
using System.Linq;
using Utilities;

namespace Toolbox
{

    public partial class MainWindow : Window
    {
        UIDocument uidoc = null;
        Document doc = null;
        UIApplication uiapp = null;
        Autodesk.Revit.ApplicationServices.Application app = null;

        public MainWindow(UIDocument uid, ExternalCommandData _commandData, ElementSet _elements)
        {
            InitializeComponent();

            app = _commandData.Application.Application;
            uiapp = new UIApplication(app);
            uidoc = uid;
            doc = uidoc.Document;
        }

        //private void QuickSelect_Click(object sender, RoutedEventArgs e)
        //{
        //    Close();
        //    QuickSelectGUI MainWn = new QuickSelectGUI(doc);
        //    MainWn.ShowDialog();
        //}

        private void FamLoader_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //private void KnEditor_Click(object sender, RoutedEventArgs e)
        //{
        //    Close();
        //    KnEditorWn MainWn = new KnEditorWn(doc);
        //    MainWn.ShowDialog();
        //}

        private void ReplaceDimTxt_Click(object sender, RoutedEventArgs e)
        {
            One_file_tools.ReplaceDimTxt MainWn = new ReplaceDimTxt(uidoc, doc);
            MainWn.ShowDialog();
        }

        private void DelTagByTxt_Click(object sender, RoutedEventArgs e)
        {
            One_file_tools.DelTagByTxt MainWn = new DelTagByTxt(uidoc, doc);
            MainWn.ShowDialog();
        }

        private void CatToWkst_Click(object sender, RoutedEventArgs e)
        {
            MoveCatsToWorkset.CatToWkst MainWn = new CatToWkst(uidoc);
            MainWn.ShowDialog();
        }

        private void StandardLineStyleCheck_Click(object sender, RoutedEventArgs e)
        {
            View activeView = doc.ActiveView;

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Get line styles");
                Line L0 = Line.CreateBound(new XYZ(0, 0, 0),
                                           new XYZ(1, 1, 0));

                DetailCurve detailLine = doc.Create.NewDetailCurve(activeView, L0);
                ICollection<ElementId> AllStylesIds = detailLine.GetLineStyleIds();
                t.RollBack();

                List<GraphicsStyle> AllStyles = new List<GraphicsStyle>();

                foreach (ElementId StyleId in AllStylesIds)
                {
                    GraphicsStyle GraphStyle = doc.GetElement(StyleId) as GraphicsStyle;
                    AllStyles.Add(GraphStyle);
                }


                Category lineStyle = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);
                CategoryNameMap lineStyleSubTypes = lineStyle.SubCategories;

                t.Start("Change Styles Name");

                foreach (Category LineSubCat in lineStyleSubTypes)
                {
                    if (LineSubCat.LineColor.Blue == 0 && LineSubCat.LineColor.Red == 0 && LineSubCat.LineColor.Green == 0)
                    {
                        try
                        {
                            string PatternName = doc.GetElement(LineSubCat.GetLinePatternId(GraphicsStyleType.Projection)).Name;

                            string weight = LineSubCat.GetLineWeight(GraphicsStyleType.Projection).GetValueOrDefault().ToString("00");

                            string NewName = weight + " - " + PatternName;

                            Utils.SimpleDialog(NewName);


                            GraphicsStyle StyleElement = AllStyles.Find(Style => Style.Name == LineSubCat.Name);

                            StyleElement.Name = NewName;
                        }
                        catch (System.NullReferenceException)
                        {
                            continue;
                        }
                        catch (System.InvalidOperationException)
                        { continue; }
                    }
                }
                t.Commit();

            }

        }



        private void ToggleGridBubbles_Click(object sender, RoutedEventArgs e)
        {
            using (Transaction t = new Transaction(doc, "Toggle grids"))
            {
                t.Start();

                foreach (ElementId elemId in uidoc.Selection.GetElementIds())
                {
                    Element SelectedElem = doc.GetElement(elemId);
                    if (SelectedElem is DatumPlane)
                    {
                        DatumPlane xGrid = SelectedElem as DatumPlane;

                        if (xGrid.IsBubbleVisibleInView(DatumEnds.End0, uidoc.ActiveView) && !xGrid.IsBubbleVisibleInView(DatumEnds.End1, uidoc.ActiveView))
                        {
                            xGrid.HideBubbleInView(DatumEnds.End0, uidoc.ActiveView);
                            xGrid.ShowBubbleInView(DatumEnds.End1, uidoc.ActiveView);
                        }

                        else if (!xGrid.IsBubbleVisibleInView(DatumEnds.End0, uidoc.ActiveView) && xGrid.IsBubbleVisibleInView(DatumEnds.End1, uidoc.ActiveView))
                        {
                            xGrid.ShowBubbleInView(DatumEnds.End0, uidoc.ActiveView);
                            xGrid.ShowBubbleInView(DatumEnds.End1, uidoc.ActiveView);
                        }

                        else
                        {
                            xGrid.ShowBubbleInView(DatumEnds.End0, uidoc.ActiveView);
                            xGrid.HideBubbleInView(DatumEnds.End1, uidoc.ActiveView);
                        }
                    }
                }

                t.Commit();
            }
        }

        private void ReplaceTemplates_Click(object sender, RoutedEventArgs e)
        {
            TemplateReplace.MainWn MainWindow = new TemplateReplace.MainWn(uidoc);
            MainWindow.ShowDialog();
        }

        private void SPN_To_SPNBronze_Click(object sender, RoutedEventArgs e)
        {
            // THIS ADDIN TAKES THE USER SELECTION,
            // IF IT'S A BROWSER SCHEDULE (SELECTED FROM THE VIEW TREE)
            // READS THE TEMPLATE AND CHANGES THE AOR GLOBAL PARAMETER
            // DEPENDING ON IF THE TEMPLATE IS FOR CA OR TX
            // THEN PRINTS ALL SETS SELECTED

            // Get user selection to see if it's a single sheet schedule
            ICollection<ElementId> elementIds = uidoc.Selection.GetElementIds();

            foreach (ElementId elementId in elementIds)
            {

                Element element = doc.GetElement(elementId);

                ViewSchedule viewSched = null;

                // If element is a SCHEDULE FROM THE BROWSER
                if (element is ViewSchedule)
                {
                    viewSched = element as ViewSchedule;

                    // Placeholder sheets print as blank pages, we don't want them
                    FilteredElementCollector sheetList = new FilteredElementCollector(doc);
                    sheetList.OfClass(typeof(ViewSheet))
                                .Cast<ViewSheet>()
                                .Where(i => !i.IsPlaceholder);

                    ViewSet viewSet = new ViewSet();

                    // Read the data from the schedule
                    TableData table = viewSched.GetTableData();
                    TableSectionData section = table.GetSectionData(SectionType.Body);


                    int nRows = section.NumberOfRows;

                    // Read each cell and find the sheet with that number
                    foreach (int num in Enumerable.Range(0, nRows))
                    {

                        string SchedSheetNum = viewSched.GetCellText((SectionType.Body), num, 0).ToString();

                        foreach (ViewSheet sheet in sheetList)
                        {
                            if (SchedSheetNum == sheet.SheetNumber && sheet.IsPlaceholder == false)
                            {
                                viewSet.Insert(sheet);
                                break;
                            }
                        }

                    }

                    using (Transaction transac = new Transaction(doc))
                    {
                        transac.Start("Set by index");

                        // Change the parameter
                        ElementId paramId = GlobalParametersManager.FindByName(doc, "AOR");
                        GlobalParameter param = doc.GetElement(paramId) as GlobalParameter;

                        string AOR = null;

                        Element ViewTemplate = doc.GetElement(viewSched.ViewTemplateId);
                        if (ViewTemplate.Name.EndsWith("TX"))
                        {
                            AOR = "TEXAS";
                        }
                        else if (ViewTemplate.Name.EndsWith("CA"))
                        {
                            AOR = "CALI";
                        }

                        param.SetValue(new StringParameterValue(AOR));




                        // PrintManager saves the set and selects it
                        PrintManager printManager = doc.PrintManager;
                        printManager.PrintRange = PrintRange.Select;

                        ViewSheetSetting viewSheetSetting = printManager.ViewSheetSetting;
                        viewSheetSetting.CurrentViewSheetSet.Views = viewSet;

                        string SetName = "Automatic Set";

                        try
                        {
                            viewSheetSetting.SaveAs(SetName);

                        }
                        catch (Autodesk.Revit.Exceptions.InvalidOperationException)
                        {
                            // Delete existing set
                            FilteredElementCollector Sheet_Set_Collector = new FilteredElementCollector(doc);

                            ViewSheetSet SetToDel = Sheet_Set_Collector.OfClass(typeof(ViewSheetSet))
                                                          .Cast<ViewSheetSet>()
                                                          .Where(i => i.Name == SetName)
                                                          .FirstOrDefault();
                            doc.Delete(SetToDel.Id);

                            viewSheetSetting.SaveAs(SetName);
                        }

                        finally
                        {
                            transac.Commit();

                            doc.Print(viewSet, true);
                            //RevitCommandId command = RevitCommandId.LookupCommandId("ID_REVIT_FILE_PRINT");
                            //uiapp.PostCommand(command);
                        }


                    }
                }

            }
        }

    }
}
