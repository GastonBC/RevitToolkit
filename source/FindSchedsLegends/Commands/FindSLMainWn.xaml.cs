using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.ComponentModel;
using System.Diagnostics;


// TODO add checkbox system

namespace FindSchedsLegends
{
    public class ElementsType
    {
        public Element ElemInst { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int ElemCount { get; set; }
        public List<Element> InstanceCount { get; set; }
    }


    public partial class FindSLMainWn : Window
    {
        private GridViewColumnHeader listViewSortCol = null;
        private WindowUtils.SortAdorner listViewSortAdorner = null;

        UIDocument uidoc = null;
        Document doc = null;

        List<ElementsType> ElemTypeList = new List<ElementsType>();
        
        public FindSLMainWn(UIDocument uid,
                            FilteredElementCollector SheetCollector,
                            FilteredElementCollector SchedCollector)
        {
            InitializeComponent();
            uidoc = uid;
            doc = uidoc.Document;

            foreach (ViewSheet sheet in SheetCollector)
            {
                // Process schedules from sched collector
                foreach (ScheduleSheetInstance SchedInstance in SchedCollector)
                {
                    if (SchedInstance.IsTitleblockRevisionSchedule || SchedInstance.OwnerViewId != sheet.Id)
                    {
                        // Skip, it's a revision or not part of the sheet
                        continue;
                    }

                    ViewSchedule Sched = doc.GetElement(SchedInstance.ScheduleId) as ViewSchedule;

                    ElementsType SchedElem = ElemTypeList.Find(
                        item => item.ElemInst.Id == Sched.Id);

                    if (SchedElem != null)
                    {

                        SchedElem.ElemCount++;

                        // Add the instance to the list to use Select button
                        SchedElem.InstanceCount.Add(SchedInstance as Element);

                    }

                    else
                    {
                        ElementsType addition = new ElementsType
                        {
                            Name = Sched.Name,
                            Type = "Schedule",
                            ElemInst = Sched,
                            ElemCount = 1,
                            InstanceCount = new List<Element>()
                        };
                        addition.InstanceCount.Add(SchedInstance as Element);

                        ElemTypeList.Add(addition);
                    }
                }

                // Process legends right from sheet viewports
                // This is faster than looping a viewport collector
                foreach (ElementId VportInstanceId in sheet.GetAllViewports())
                {
                    Viewport VportInstance = doc.GetElement(VportInstanceId) as Viewport;
                    View ViewLeg = doc.GetElement(VportInstance.ViewId) as View;

                    if (ViewLeg.ViewType is ViewType.Legend)
                    {
                        ElementsType LegElem = ElemTypeList.Find(
                                item => item.ElemInst.Id == ViewLeg.Id);

                        if (LegElem != null)
                        {
                            LegElem.ElemCount++;

                            LegElem.InstanceCount.Add(VportInstance as Element);
                        }

                        else
                        {
                            ElementsType addition = new ElementsType
                            {
                                Name = ViewLeg.Name,
                                Type = "Legend",
                                ElemInst = ViewLeg,
                                ElemCount = 1,
                                InstanceCount = new List<Element>()
                            };
                            addition.InstanceCount.Add(VportInstance as Element);

                            ElemTypeList.Add(addition);
                        }
                    }
                }
            }


            ElemTypeList.Sort((x, y) => x.Type.CompareTo(y.Type));

            SLListView.ItemsSource = ElemTypeList;

            TextLab.Content = $"Count: {ElemTypeList.Count}";
        }

        public FindSLMainWn(UIDocument uid, 
                            FilteredElementCollector SheetCollector,
                            FilteredElementCollector SchedCollector,
                            List<Element> ElementsSelected)
        {
            InitializeComponent();
            uidoc = uid;
            doc = uidoc.Document;

            foreach (ViewSheet sheet in SheetCollector)
            {

                // Process schedules from sched collector
                foreach (ScheduleSheetInstance SchedInstance in SchedCollector)
                {
                    if (SchedInstance.IsTitleblockRevisionSchedule || SchedInstance.OwnerViewId != sheet.Id)
                    {
                        // Skip
                        continue;
                    }

                    ViewSchedule Sched = doc.GetElement(SchedInstance.ScheduleId) as ViewSchedule;

                    // Is the type part of our selection?
                    bool PartOfSelection = ElementsSelected.Any(item => item.Id == Sched.Id);
                    if (PartOfSelection)
                    {

                        ElementsType SchedElem = ElemTypeList.Find(
                                item => item.ElemInst.Id == Sched.Id);

                        if (SchedElem != null)
                        {
                            SchedElem.ElemCount++;

                            // Add the instance to the list to use Select button
                            SchedElem.InstanceCount.Add(SchedInstance as Element);

                        }
                        else
                        {
                            ElementsType addition = new ElementsType
                            {
                                Name = Sched.Name,
                                Type = "Schedule",
                                ElemInst = Sched,
                                ElemCount = 1,
                                InstanceCount = new List<Element>()
                            };
                            addition.InstanceCount.Add(SchedInstance as Element);

                            ElemTypeList.Add(addition);
                        }
                    }


                }

                // Process legends right from sheet viewports
                // This is faster than looping a viewport collector
                foreach (ElementId VportId in sheet.GetAllViewports())
                {
                    Viewport VportInstance = doc.GetElement(VportId) as Viewport;
                    View ViewLeg = doc.GetElement(VportInstance.ViewId) as View;

                    // Is the type part of our selection?
                    bool PartOfSelection = ElementsSelected.Any(
                        item => item.Id == ViewLeg.Id);

                    if (PartOfSelection && ViewLeg.ViewType == ViewType.Legend)
                    {
                        ElementsType LegElem = ElemTypeList.Find(
                                item => item.ElemInst.Id == ViewLeg.Id);

                        if (LegElem != null)
                        {
                            LegElem.ElemCount++;

                            LegElem.InstanceCount.Add(VportInstance as Element);
                        }
                        else
                        {
                            ElementsType addition = new ElementsType
                            {
                                Name = ViewLeg.Name,
                                Type = "Legend",
                                ElemInst = ViewLeg,
                                ElemCount = 1,
                                InstanceCount = new List<Element>()
                            };
                            addition.InstanceCount.Add(VportInstance as Element);

                            ElemTypeList.Add(addition);
                        }
                    }
                }
            }


            ElemTypeList.Sort((x, y) => x.Type.CompareTo(y.Type));

            SLListView.ItemsSource = ElemTypeList;

            int inList = ElemTypeList.Count;
            int inSelection = ElementsSelected.Count;

            TextLab.Content = $"Count: {inList} out of {inSelection} selected";
        }

        private void onSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if (SLListView.SelectedItems.Count >= 1) 
            {
                b_ViewSheetGUI.IsEnabled = true;
                b_Select.IsEnabled = true;
            }
            else // Else allow only to select the cads
            {
                b_ViewSheetGUI.IsEnabled = false;
                b_Select.IsEnabled = false;
            }
        }

        private void ColHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                SLListView.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new WindowUtils.SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            SLListView.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));


        }

        private void b_Select_Click(object sender, RoutedEventArgs e)
        {
            // Select all PLACED instances of selection

            List<ElementId> Ids = new List<ElementId>();

            foreach (ElementsType Elem in SLListView.SelectedItems)
            {
                foreach (Element Instance in Elem.InstanceCount) // This is the instance list
                {
                    Ids.Add(Instance.Id);
                }
            }
            uidoc.Selection.SetElementIds(Ids);
            Close();
        }

        private void b_GoSheetGUI_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            List<ElementsType> ItemSelection = SLListView.SelectedItems as List<ElementsType>;
            
            GetSheetsWn SheetsWnGUI = new GetSheetsWn(uidoc, SLListView.SelectedItems, this);
            SheetsWnGUI.ShowDialog();

            // This button will open a new GUI with a list of sheets for the current
            // selected element

            // If allow only one selection, display those sheets
            // If allow multiple, display more
        }

        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            Search_tb.Clear();
            SLListView.SelectedItems.Clear();
        }

        private void search_txt_changed(object sender, TextChangedEventArgs e)
        {
            SLListView.ItemsSource = ElemTypeList.Where(i => i.Name.ToLower().Contains(Search_tb.Text.ToLower()) ||
                                                            i.Type.ToLower().Contains(Search_tb.Text.ToLower()) );
        }
    }
}
