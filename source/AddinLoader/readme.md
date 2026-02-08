# AddinLoader

**AddinLoader** is the heart of the Revit Toolkit. It serves as a dynamic proxy loader that scans the toolkit add-ins and generates a Ribbon tab. It is designed to bridge the gap between development and production, allowing for a highly flexible Revit environment.

## 🌟 Key Features

### 🛠 The Proxy System (Zero-Restart Workflow)

The most powerful feature of this loader is its ability to execute commands via reflection.

* **Dynamic Loading:** Instead of Revit locking your DLLs upon startup, the AddinLoader points to the assemblies.
* **Hot-Reloading:** You can modify your code in Visual Studio, rebuild the specific project, and run the command again in the same Revit session. **No more restarting Revit** every time you change a line of code.

### 🎀 Unified Ribbon UI

Instead of having 15+ individual add-ins cluttering the "External Tools" menu, AddinLoader gathers them into a dedicated, clean Ribbon Tab.

* **Organized Layout:** Uses stacked columns (3 buttons high) to maximize space and efficiency.
* **Automated Invocation:** Handles the complex task of passing `ExternalCommandData` from the loader's interface to the individual tool DLLs.

---

## 📂 Installation & Setup

To enable the proxy-loading system:

1. Place the `AddinLoader.dll` and `AddinLoader.addin` in your Revit Addins folder:
`%AppData%\Autodesk\Revit\Addins\[Version]\AddinLoader`
2. Ensure your tool DLLs are located in the path defined in `GlobalVars` (a subfolder within the Addins directory).
3. On startup, AddinLoader will scan the paths, build the ribbon, and link the buttons to the `InvokeXX` commands.

---

## 🏗 Technical Architecture

The loader operates using an **Invoker Pattern**:

1. **The Ribbon:** AddinLoader creates a `PushButton` on the Revit UI.
2. **The Invoker:** The button is tied to an `IExternalCommand` (e.g., `Invoke01`) inside the AddinLoader assembly.
3. **The Proxy:** When clicked, the Invoker uses a utility method (`Utils.InvokeCmd`) to:
* Load the target assembly from disk into memory.
* Locate the entry point of the tool.
* Execute the command while passing the active Revit `commandData`.
