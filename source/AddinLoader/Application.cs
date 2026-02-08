using AddinLoader.Commands;
using Nice3point.Revit.Toolkit.External;

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
            var panel = Application.CreatePanel("Commands", "AddinLoader");

            panel.AddPushButton<StartupCommand>("Execute")
                .SetImage("/AddinLoader;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/AddinLoader;component/Resources/Icons/RibbonIcon32.png");
        }
    }
}