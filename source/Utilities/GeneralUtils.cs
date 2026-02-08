using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Utilities
{
    public static partial class Utils
    {
        public static PushButtonData CreateDefaultButton(string ButtonName, string exeConfigPath, string InvokerClass, bool Icon = true, string Tooltip="Tooltip soon")
        {
            PushButtonData buttonData = new PushButtonData(ButtonName, ButtonName, exeConfigPath, InvokerClass);

            buttonData.ToolTip = Tooltip;

            if (Icon)
            {
                // Define the resource paths
                string icon16 = "pack://application:,,,/AddinLoader;component/Resources/Icons/RibbonIcon16.png";
                string icon32 = "pack://application:,,,/AddinLoader;component/Resources/Icons/RibbonIcon32.png";

                // Assign the images
                buttonData.Image = new BitmapImage(new Uri(icon16));
                buttonData.LargeImage = new BitmapImage(new Uri(icon32));
            }

            return buttonData;
        }

        public static PulldownButton CreateDefaultPulldown(string name, RibbonPanel panel)
        {
            PulldownButton pullDown = panel.AddPullDownButton(name);

            string icon16 = "pack://application:,,,/AddinLoader;component/Resources/Icons/RibbonIcon16.png";
            string icon32 = "pack://application:,,,/AddinLoader;component/Resources/Icons/RibbonIcon32.png";

            pullDown.Image = new BitmapImage(new Uri(icon16));
            pullDown.LargeImage = new BitmapImage(new Uri(icon32));

            return pullDown;
        }


        public static string ReFormatString(string origStr, string origFormat, string newFormat)
        {
            // Use a regex that doesn't capture the tag name during the Split call
            var splitRegex = new Regex(@"\{.+?\}");
            var tagRegex = new Regex(@"\{(.+?)\}");

            var tags = new List<string>();
            foreach (Match m in tagRegex.Matches(origFormat))
            {
                tags.Add(m.Groups[1].Value);
            }

            // Split only gives us the text BETWEEN the tags
            string[] literals = splitRegex.Split(origFormat);

            string patternStr = "^";
            for (int i = 0; i < literals.Length; i++)
            {
                patternStr += Regex.Escape(literals[i]);
                if (i < tags.Count)
                {
                    patternStr += "(.+?)";
                }
            }
            patternStr += "$";

            var pattern = new Regex(patternStr);
            var matchResult = pattern.Match(origStr);

            if (!matchResult.Success) return origStr;

            string result = newFormat;
            for (int i = 0; i < tags.Count; i++)
            {
                string val = matchResult.Groups[i + 1].Value;
                result = result.Replace("{" + tags[i] + "}", val);
            }

            return result;
        }


        public static void SimpleDialog(string header, string content)
        {
            TaskDialog mainDialog = new TaskDialog("Revit Toolkin - GasBC");
            mainDialog.TitleAutoPrefix = false;
            mainDialog.MainInstruction = header;
            mainDialog.MainContent = content;
            mainDialog.Show();
        }

        public static void SimpleDialog(string content)
        {
            TaskDialog mainDialog = new TaskDialog("Revit Toolkin - GasBC");
            mainDialog.TitleAutoPrefix = false;
            mainDialog.MainContent = content;
            mainDialog.Show();
        }

        public static bool ConfirmDialog(string header, string content)
        {
            TaskDialog mainDialog = new TaskDialog("Revit Toolkin - GasBC")
            {
                TitleAutoPrefix = false,
                MainInstruction = header,
                MainContent = content,
                CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No
            };

            TaskDialogResult res = mainDialog.Show();

            switch (res)
            {
                case TaskDialogResult.Yes:
                    return true;
                default:
                    return false;
            }
        }

        public static string GetExeConfigPath(string DllName)
        {
            string ThisDllPath = Assembly.GetExecutingAssembly().Location;
            Assembly ThisAssembly = Assembly.GetExecutingAssembly();

            // Assembly that contains the invoke method
            return Path.GetDirectoryName(ThisDllPath) + "\\" + DllName;
        }

        public static RibbonPanel GetRevitPanel(UIControlledApplication uiApp, string PanelName)
        {
            RibbonPanel DefaultPanel = null;


            // Create the panel in the addins tab
            try
            {
                DefaultPanel = uiApp.CreateRibbonPanel(PanelName);
            }

            catch (Autodesk.Revit.Exceptions.ArgumentException)
            {
                DefaultPanel = uiApp.GetRibbonPanels().FirstOrDefault(n => n.Name.Equals(PanelName, StringComparison.InvariantCulture));
            }

            return DefaultPanel;
        }

        public static void CatchDialog(Exception ex)
        {
            string head = ex.Source + " - " + ex.GetType().ToString();
            string moreText = ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Data;

            Utils.SimpleDialog(head, moreText);
        }


        internal static void CatchDialog(Exception ex, string TitlePrefix)
        {
            string head = TitlePrefix + " " + ex.Source + " - " + ex.GetType().ToString();
            string moreText = ex.Message + "\n\n" + ex.StackTrace;

            Utils.SimpleDialog(head, moreText);
        }


        /// <summary>
        /// Convert embedded ico, png, jpg, etc to something usable by Revit
        /// </summary>
        /// <param name="imagePath">Path to the embedded resource</param>
        /// <returns>ImageSource</returns>
        //public static ImageSource RetriveImage(string imagePath, Assembly assembly)
        //{

        //    Stream manifestResourceStream = assembly.GetManifestResourceStream(imagePath);
        //    string str = imagePath.Substring(imagePath.Length - 3);

        //    if (str == "jpg")
        //        return (ImageSource)new JpegBitmapDecoder(manifestResourceStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];
        //    else if (str == "bmp")
        //        return (ImageSource)new BmpBitmapDecoder(manifestResourceStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];
        //    else if (str == "png")
        //        return (ImageSource)new PngBitmapDecoder(manifestResourceStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];
        //    else if (str == "ico")
        //        return (ImageSource)new IconBitmapDecoder(manifestResourceStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];
        //    else
        //        return (ImageSource)null;
        //}

        public static List<ViewSheet> GetAllSheets(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet));
            List<ViewSheet> SheetList = new List<ViewSheet>();
            foreach (Element elem in collector)
            {
                ViewSheet Sheet = elem as ViewSheet;
                SheetList.Add(Sheet);
            }
            return SheetList;
        }
    }
}
