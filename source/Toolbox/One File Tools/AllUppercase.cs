using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using System.Windows;


namespace Toolbox
{
    public partial class MainWindow : Window
    {
        private void AllUppercase_Click(object sender, RoutedEventArgs e)
        {
            using (Transaction t = new Transaction(doc, "Text to uppercase"))
            {
                t.Start();

                /*
                IEnumerable<View> viewsCol = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Views).WhereElementIsNotElementType().Cast<View>();
                foreach (View view in viewsCol)
                {
                    try
                    {
                        string title = view.Name;
                        view.Name = title.ToUpper();
                    }
                    catch(Autodesk.Revit.Exceptions.ArgumentException)
                    {
                        continue;
                    }
                }
                */

                IEnumerable<TextNote> txNotesCol = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_TextNotes)
                    .WhereElementIsNotElementType().Cast<TextNote>();

                foreach (TextNote note in txNotesCol)
                {
                    //string NoteContent = note.Text;
                    //note.Text = NoteContent.ToUpper();


                    FormattedText textNoteFormatted = note.GetFormattedText();
                    textNoteFormatted.SetAllCapsStatus(true);

                    note.SetFormattedText(textNoteFormatted);

                }

                t.Commit();
            }
        }
    }
}

