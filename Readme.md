# Revit Toolkit

A collection of high-performance Revit add-ins written in C#. This toolkit includes original tools and several popular utilities—like **ReValue**—carefully ported from the **pyRevit Toolbox** and converted to C#.

## 🚀 Two Ways to Use

This toolkit is designed with maximum flexibility. You can choose how to load and interact with the tools based on your workflow.

### 1. Proxy Loaded (Dynamic Development Mode)

This is the recommended method for developers or users who want a dedicated interface. By using the **AddinLoader**, the tools are loaded through a proxy.

* **How to use:** Ensure the `AddinLoader.addin` file is present in your Revit Addins folder.
* **The Benefit:** All tools will appear in a custom Ribbon Tab.
* **The "No-Restart" Advantage:** Because they are proxy-loaded, you can modify, rebuild, and re-run the add-ins **without restarting Revit**. The loader dynamically handles the assembly execution, saving hours of development time.

### 2. Independent Loading (Native Mode)

If you only need specific tools and prefer the native Revit "External Tools" menu, you can load them individually.

* **How to use:** Copy the specific `.dll` of the tool you want into your Addins folder, accompanied by its proper `.addin` manifest file.
* **The Result:** These add-ins will show up under the **External Commands** dropdown menu in the Revit Add-ins tab.

---

## 📦 Installation

You can install the tools individually or all at once:

* **Individual:** Download the specific project DLLs from the latest release.
* **Full Suite:** Run the **Release Installer (.msi)**. This will install the entire suite and set up the `AddinLoader` automatically for the best experience.

---

## 🛠 Included Add-ins

| Add-in | Description |
| --- | --- |
| **[ReValue](./source/ReValue/readme.md)** | Ported from pyRevit. Powerful parameter value management. |
| **Type Renamer** | Batch rename elements with prefix/suffix logic. |
| **Fix Constraints** | Automatically adjust element levels to the nearest level. |
| **CAD Detective** | Locate and manage imported or linked CAD files hidden in the model. |
| **Toolbox** | A collection of minor productivity helpers. |
| *(And many more...)* | See the full list in the command ribbon. |

---

## 🏗 Developer Notes

This project uses **NUKE Build** for automation.

* To compile the solution: `nuke compile`
* To create installers: `nuke createinstaller`

The build system targets Revit **2024, 2025, and 2026** specifically, ensuring compatibility with the latest Revit API changes and .NET versions.

This project and the installer have been created using **[Nice3Point's Revit templates](https://github.com/Nice3point/RevitTemplates)**.s

[Detailed Build Information](nuke_readme.md)


