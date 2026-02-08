using AddinLoader.Commands;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using System.IO;
using System.Reflection;
using Utilities; 

namespace AddinLoader
{
    /// <summary>
    ///     Application entry point
    /// </summary>
    [UsedImplicitly]
    public class Application : ExternalApplication
    {
        public override void OnStartup()
        {
            CreateRibbon();
        }

        private void CreateRibbon()
        {
            RibbonPanel panel = Application.CreatePanel("Commands", "AddinLoader");

            RibbonButton typeRenamerbutton = panel.AddPushButton<Invoke01>("Type Renamer")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");

            RibbonButton fixConstraintsbutton = panel.AddPushButton<Invoke02>("Fix Constraints")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");


            RibbonButton reValuebutton = panel.AddPushButton<Invoke03>("ReValue")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");
        }
    }
}