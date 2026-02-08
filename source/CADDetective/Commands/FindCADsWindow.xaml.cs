using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CADDetective
{
    public class CADItem : INotifyPropertyChanged
    {
        public ImportInstance CADImportInstance { get; set; }
        public View OwnerView { get; set; }
        public string IsHide { get; set; }
        public string IsLink { get; set; }
        public string ViewName { get; set; }

        private bool _Checked;
        public bool Checked
        {
            get { return _Checked; }
            set
            {
                if (_Checked == value)
                    return;
                _Checked = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Checked"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }

    /// <summary>
    /// Interaction logic for FindCADsWindow.xaml
    /// </summary>
    public partial class FindCADsWindow : Window
    {
        readonly UIDocument uidoc;
        readonly Document doc;
        private readonly ObservableCollection<CADItem> CADList = new ObservableCollection<CADItem>();

        public FindCADsWindow(UIDocument uid)
        {
            InitializeComponent();
            CADListView.ItemsSource = CADList;
            uidoc = uid;
            doc = uidoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(ImportInstance))
                     .WhereElementIsNotElementType()
                     .Cast<ImportInstance>();

            foreach (ImportInstance IInstance in collector)
            {
                if (IInstance.OwnerViewId != null && IInstance.OwnerViewId.Value != -1)
                {
                    View ownerView = doc.GetElement(IInstance.OwnerViewId) as View;

                    ViewFamilyType ViewTy = doc.GetElement(ownerView.GetTypeId()) as ViewFamilyType;

                    string ViewN = ViewTy.Name + " - " + ownerView.Name;

                    CADItem addition = new CADItem
                    {
                        Checked = false,
                        ViewName = ViewN,
                        CADImportInstance = IInstance,
                        OwnerView = ownerView,
                        IsHide = IInstance.IsHidden(ownerView).ToString()
                    };
                    CADList.Add(addition);
                }
                else
                {
                    CADItem addition = new CADItem
                    {
                        Checked = false,
                        ViewName = null,
                        CADImportInstance = IInstance,
                        OwnerView = null,
                        IsHide = null
                    };
                    CADList.Add(addition);
                }
            }
        }

        private void GoCAD_Click(object sender, RoutedEventArgs e)
        {
            CADItem SelectedCad = CADList.Where(item => item.Checked == true).First();
            uidoc.ActiveView = SelectedCad.OwnerView;
            uidoc.ShowElements(SelectedCad.CADImportInstance.Id);
        }

        private void SelectCAD_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<CADItem> SelectedCads = CADList.Where(item => item.Checked == true);

            List<ElementId> Ids = new List<ElementId>();

            foreach (CADItem SelectedCad in SelectedCads) // add selected cads to selection by ID
            {
                Ids.Add(SelectedCad.CADImportInstance.Id);
            }
            uidoc.Selection.SetElementIds(Ids);
            Close();
        }

        private void GoView_Click(object sender, RoutedEventArgs e)
        {
            CADItem SelectedCad = CADList.Where(item => item.Checked == true).First();

            View selectedCadView = SelectedCad.OwnerView;
            uidoc.ActiveView = selectedCadView;
        }

        private void onCheckBoxCheck(object sender, RoutedEventArgs e)
        {
            SelectionCheckChange();
            // deselect single item if a checkbox is checked or it will change your selection
            // as well
            if (CADListView.SelectedItems.Count == 1)
            {
                CADListView.SelectedItem = null;
            }

            else
            {
                foreach (object ListObj in CADListView.SelectedItems)
                {
                    CADItem ListItem = ListObj as CADItem;
                    ListItem.Checked = true;
                }
            }
        }

        private void onCheckBoxUncheck(object sender, RoutedEventArgs e)
        {
            SelectionCheckChange();
            // deselect single item if a checkbox is checked or it will change your selection
            // as well
            if (CADListView.SelectedItems.Count == 1)
            {
                CADListView.SelectedItem = null;
            }
            else
            {
                foreach (object ListObj in CADListView.SelectedItems)
                {
                    CADItem ListItem = ListObj as CADItem;
                    ListItem.Checked = false;
                }
            }
        }


        private void SelectionCheckChange()
        {
            IEnumerable<CADItem> checkedCads = CADList.Where(item => item.Checked == true);

            if (checkedCads.Count() == 1 && checkedCads.First().OwnerView != null) // If only one cad selected and is view-specific
            {
                GoCAD.IsEnabled = true;
                GoView.IsEnabled = true;
                SelCAD.IsEnabled = true;
            }
            else // Else allow only to select the cads
            {
                GoCAD.IsEnabled = false;
                GoView.IsEnabled = false;
                SelCAD.IsEnabled = true;
            }
        }

        private void search_txt_changed(object sender, TextChangedEventArgs e)
        {
            CADListView.ItemsSource = CADList.Where(c => c.CADImportInstance.Category.Name.ToLower().Contains(Search_tb.Text.ToLower()));
        }

        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            Search_tb.Clear();
            CADListView.SelectedItems.Clear();
        }
    }
}
