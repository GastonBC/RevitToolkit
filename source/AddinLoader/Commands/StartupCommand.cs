using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using Utilities;

namespace AddinLoader.Commands
{

    [Transaction(TransactionMode.Manual)]
    public abstract class BaseInvoke : IExternalCommand
    {
        protected abstract string Path { get; }
        protected abstract string LogName { get; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                return Utils.InvokeCmd(commandData, ref message, elements, Path, LogName);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }

    public class Invoke01 : BaseInvoke { protected override string Path => GlobalVars.INVOKE01_PATH; protected override string LogName => "INVOKE 01"; }
    public class Invoke02 : BaseInvoke { protected override string Path => GlobalVars.INVOKE02_PATH; protected override string LogName => "INVOKE 02"; }
    public class Invoke03 : BaseInvoke { protected override string Path => GlobalVars.INVOKE03_PATH; protected override string LogName => "INVOKE 03"; }
    public class Invoke04 : BaseInvoke { protected override string Path => GlobalVars.INVOKE04_PATH; protected override string LogName => "INVOKE 04"; }
    public class Invoke05 : BaseInvoke { protected override string Path => GlobalVars.INVOKE05_PATH; protected override string LogName => "INVOKE 05"; }
    public class Invoke06 : BaseInvoke { protected override string Path => GlobalVars.INVOKE06_PATH; protected override string LogName => "INVOKE 06"; }
    public class Invoke07 : BaseInvoke { protected override string Path => GlobalVars.INVOKE07_PATH; protected override string LogName => "INVOKE 07"; }
    public class Invoke08 : BaseInvoke { protected override string Path => GlobalVars.INVOKE08_PATH; protected override string LogName => "INVOKE 08"; }
    public class Invoke09 : BaseInvoke { protected override string Path => GlobalVars.INVOKE09_PATH; protected override string LogName => "INVOKE 09"; }
    public class Invoke10 : BaseInvoke { protected override string Path => GlobalVars.INVOKE10_PATH; protected override string LogName => "INVOKE 10"; }
    public class Invoke11 : BaseInvoke { protected override string Path => GlobalVars.INVOKE11_PATH; protected override string LogName => "INVOKE 11"; }
    public class Invoke12 : BaseInvoke { protected override string Path => GlobalVars.INVOKE12_PATH; protected override string LogName => "INVOKE 12"; }
    public class Invoke13 : BaseInvoke { protected override string Path => GlobalVars.INVOKE13_PATH; protected override string LogName => "INVOKE 13"; }
    public class Invoke14 : BaseInvoke { protected override string Path => GlobalVars.INVOKE13_PATH; protected override string LogName => "INVOKE 14"; }
    public class Invoke15 : BaseInvoke { protected override string Path => GlobalVars.INVOKE13_PATH; protected override string LogName => "INVOKE 15"; }
}