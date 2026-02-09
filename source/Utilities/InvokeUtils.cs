using Autodesk.Revit.UI;

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static partial class Utils
    {
        public static Result InvokeApp(UIControlledApplication application, string assemblyPath, string className)
        {
            try
            {

                // Load bytes to avoid locking the DLL
                byte[] assemblyBytes = File.ReadAllBytes(assemblyPath);
                Assembly objAssembly = Assembly.Load(assemblyBytes);

                // Find the class (Application)
                IEnumerable<Type> types = GetTypesSafely(objAssembly);
                foreach (Type type in types)
                {
                    if (type.IsClass && type.Name.Equals(className, StringComparison.OrdinalIgnoreCase))
                    {
                        // 3. Instantiate the class
                        object appInstance = Activator.CreateInstance(type);

                        // 4. Prepare arguments for OnStartup(UIControlledApplication application)
                        object[] arguments = new object[] { application };

                        // 5. Invoke OnStartup
                        object result = type.InvokeMember("OnStartup",
                            BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance | BindingFlags.OptionalParamBinding,
                            null, appInstance, arguments);

                        // Handle the return type (void vs Result)
                        if (result is Result res) return res;
                        return Result.Succeeded;
                    }
                }
            }
            catch (TargetInvocationException ex)
            {
                // This is the "real" error happening inside your Application class
                Exception realError = ex.InnerException;
                Utils.SimpleDialog("Real Error", realError.Message + "\n" + realError.StackTrace);
                return Result.Failed;
            }

            catch (Exception ex)
            {
                // Replace with your CatchDialog logic
                TaskDialog.Show("Proxy App Error", ex.Message);
                return Result.Failed;
            }
            return Result.Failed;
        }



        /// <summary>
        /// Invoke a dll to memmory and then close it. Used to execute the addin. Pass the
        /// first 3 arguments from the calling class
        /// </summary>
        /// <param name="commandData">ExternalCommandData</param>
        /// <param name="message">string</param>
        /// <param name="elements">ElementSet</param>
        /// <param name="AssemblyPath">Path to dll</param>
        /// <param name="InvokePosition">INVOKE XX</param>
        /// <param name="CommandName">Optional. Should be ThisApplication always</param>
        /// <returns></returns>
        public static Result InvokeCmd(ExternalCommandData commandData, ref string message, ElementSet elements,
                                         string AssemblyPath, string InvokePosition, string CommandName = "StartupCommand")
        {
            try
            {
                string assemblyPath = AssemblyPath; // Addin dll path

                byte[] assemblyBytes = File.ReadAllBytes(assemblyPath); // Read bytes to memory (from docstring: "[...] then closes the file")
                Assembly objAssembly01 = Assembly.Load(assemblyBytes); // Load assembly

                string strCommandName = CommandName; // Class where the command or app reside

                IEnumerable<Type> myIEnumerableType = GetTypesSafely(objAssembly01);
                foreach (Type objType in myIEnumerableType)
                {
                    if (objType.IsClass)
                    {
                        if (objType.Name.ToLower() == strCommandName.ToLower())
                        {
                            object ibaseObject = Activator.CreateInstance(objType);
                            object[] arguments = new object[] { commandData, message, elements };
                            object result = null;

                            result = objType.InvokeMember("Execute",
                                BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance,
                                null, ibaseObject, arguments);

                            break;
                        }
                    }
                }
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                Utils.CatchDialog(ex, InvokePosition);
                return Result.Failed;
            }
        }

        public static IEnumerable<Type> GetTypesSafely(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(x => x != null);
            }
        }
    }
}
