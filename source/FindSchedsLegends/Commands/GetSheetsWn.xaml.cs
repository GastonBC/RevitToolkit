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


namespace FindSchedsLegends
{
    internal class ElementsSheets
    {
        public string InstSheet { get; set; }
        public string InstElemName { get; set; }
        public ViewSheet SheetElem { get; set; }
        public Element InstElem { get; set; }
    }


    public partial class GetSheetsWn : Window
    {
        private GridViewColumnHeader listViewSortCol = null;
        private WindowUtils.SortAdorner listViewSortAdorner = null;

        UIDocument uidoc = null;
        Document doc = null;
        FindSLMainWn w_main = null;

        List<ElementsSheets> ElemTypeList = new List<ElementsSheets>();

        public GetSheetsWn(UIDocument uid,
                           System.Collections.IList SelectionRes,
                           FindSLMainWn MainWn)
        {
            InitializeComponent();
            uidoc = uid;
            doc = uidoc.Document;
            w_main = MainWn;


            foreach (ElementsType Elem in SelectionRes)
            {
                foreach (Element Instance in Elem.InstanceCount)
                {

                    if (Instance is Viewport)
                    {
                        ViewSheet sheet = doc.GetElement(
                                Instance.OwnerViewId) as ViewSheet;

                        View VP = doc.GetElement(
                            ((Viewport)Instance).ViewId) as View;

                        ElementsSheets addition = new ElementsSheets
                        {
                            InstSheet = sheet.SheetNumber + " - " + sheet.Name,
                            InstElemName = VP.Name,
                            SheetElem = sheet,
                            InstElem = VP
                        };
                        this.ElemTypeList.Add(addition);
                    }

                    else if (Instance is ScheduleSheetInstance)
                    {
                        ViewSheet sheet = doc.GetElement(
                                Instance.OwnerViewId) as ViewSheet;


                        ElementsSheets addition = new ElementsSheets
                        {
                            InstSheet = sheet.SheetNumber + " - " + sheet.Name,
                            InstElemName = Instance.Name,
                            SheetElem = sheet,
                            InstElem = Instance
                        };
                        this.ElemTypeList.Add(addition);
                    }


                }

                SheetsListView.ItemsSource = ElemTypeList;
                TextLab.Content = $"Count: {ElemTypeList.Count}";
            }
        }

        private void b_GoElem_Click(object sender, RoutedEventArgs e)
        {
            ElementsSheets elem = SheetsListView.SelectedItem as ElementsSheets;
            uidoc.ActiveView = elem.SheetElem;
            uidoc.ShowElements(elem.InstElem.Id);
        }

        private void b_Select_Click(object sender, RoutedEventArgs e)
        {
            List<ElementId> Ids = new List<ElementId>();

            foreach (ElementsSheets Elem in SheetsListView.SelectedItems)
            {
                Ids.Add(Elem.InstElem.Id);
            }
            uidoc.Selection.SetElementIds(Ids);

            w_main.Close();
            this.Close();
            
        }

        private void onSelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if (SheetsListView.SelectedItems.Count == 1)
            {
                b_GoElem.IsEnabled = true;
                b_Select.IsEnabled = true;
            }
            else if (SheetsListView.SelectedItems.Count == 0) // if no selection
            {
                b_GoElem.IsEnabled = false;
                b_Select.IsEnabled = false;
            }
            else // Else allow only to select the elements
            {
                b_GoElem.IsEnabled = false;
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
                SheetsListView.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new WindowUtils.SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            SheetsListView.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void b_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            w_main.ShowDialog();
        }

        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            Search_tb.Clear();
            SheetsListView.SelectedItems.Clear();
        }

        private void search_txt_changed(object sender, TextChangedEventArgs e)
        {
            SheetsListView.ItemsSource = ElemTypeList.Where(i => i.InstSheet.ToLower().Contains(Search_tb.Text.ToLower()) ||
                                                                i.InstElemName.ToLower().Contains(Search_tb.Text.ToLower()) );
        }
    }

}
