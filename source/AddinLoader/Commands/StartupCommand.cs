using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using Utilities;

namespace AddinLoader.Commands
{
    public abstract class BaseInvoke : IExternalCommand
    {
        protected abstract string Path { get; }
        protected abstract string LogName { get; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Utils.InvokeCmd(commandData, ref message, elements, Path, LogName);
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)] public class Invoke01 : BaseInvoke { protected override string Path => GlobalVars.INVOKE01_PATH; protected override string LogName => "INVOKE 01"; }
    [Transaction(TransactionMode.Manual)] public class Invoke02 : BaseInvoke { protected override string Path => GlobalVars.INVOKE02_PATH; protected override string LogName => "INVOKE 02"; }
    [Transaction(TransactionMode.Manual)] public class Invoke03 : BaseInvoke { protected override string Path => GlobalVars.INVOKE03_PATH; protected override string LogName => "INVOKE 03"; }
    [Transaction(TransactionMode.Manual)] public class Invoke04 : BaseInvoke { protected override string Path => GlobalVars.INVOKE04_PATH; protected override string LogName => "INVOKE 04"; }
    [Transaction(TransactionMode.Manual)] public class Invoke05 : BaseInvoke { protected override string Path => GlobalVars.INVOKE05_PATH; protected override string LogName => "INVOKE 05"; }
    [Transaction(TransactionMode.Manual)] public class Invoke06 : BaseInvoke { protected override string Path => GlobalVars.INVOKE06_PATH; protected override string LogName => "INVOKE 06"; }
    [Transaction(TransactionMode.Manual)] public class Invoke07 : BaseInvoke { protected override string Path => GlobalVars.INVOKE07_PATH; protected override string LogName => "INVOKE 07"; }
    [Transaction(TransactionMode.Manual)] public class Invoke08 : BaseInvoke { protected override string Path => GlobalVars.INVOKE08_PATH; protected override string LogName => "INVOKE 08"; }
    [Transaction(TransactionMode.Manual)] public class Invoke09 : BaseInvoke { protected override string Path => GlobalVars.INVOKE09_PATH; protected override string LogName => "INVOKE 09"; }
    [Transaction(TransactionMode.Manual)] public class Invoke10 : BaseInvoke { protected override string Path => GlobalVars.INVOKE10_PATH; protected override string LogName => "INVOKE 10"; }
    [Transaction(TransactionMode.Manual)] public class Invoke11 : BaseInvoke { protected override string Path => GlobalVars.INVOKE11_PATH; protected override string LogName => "INVOKE 11"; }
    [Transaction(TransactionMode.Manual)] public class Invoke12 : BaseInvoke { protected override string Path => GlobalVars.INVOKE12_PATH; protected override string LogName => "INVOKE 12"; }
    [Transaction(TransactionMode.Manual)] public class Invoke13 : BaseInvoke { protected override string Path => GlobalVars.INVOKE13_PATH; protected override string LogName => "INVOKE 13"; }
    [Transaction(TransactionMode.Manual)] public class Invoke14 : BaseInvoke { protected override string Path => GlobalVars.INVOKE14_PATH; protected override string LogName => "INVOKE 14"; }
    [Transaction(TransactionMode.Manual)] public class Invoke15 : BaseInvoke { protected override string Path => GlobalVars.INVOKE15_PATH; protected override string LogName => "INVOKE 15"; }
}