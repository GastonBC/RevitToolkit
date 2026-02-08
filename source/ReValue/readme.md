# ReValue for Revit

**ReValue** is a powerful parameter management tool that brings flexible, pattern-based string manipulation to Revit elements. Originally a staple of the **pyRevit Toolbox**, this version has been completely translated to C# to make it independent of any framework.

## 💡 How it Works

The application uses a **Tag-Based Matching** system. You define how a current parameter value (like a Sheet Name or Type name) is structured using `{tags}`, and the app extracts those values to rebuild a new value based on your template.

### The Pattern System

* **Current Pattern:** Tells the app how to "read" or "parse" the existing data.
* **New Pattern Template:** Tells the app how to "write" the new data.

#### Example:

Imagine you have a series of Sheets named:

`A101 - Level 1 - Floor Plan`
`A102 - Level 2 - Floor Plan`
`A103 - Level 3 - Floor Plan`

1. **Current Pattern:** `{id} - {level} - {desc}`
2. **New Pattern:** `{level} ({id}) | {desc}`

**Result:** 
`Level 1 (A101) | Floor Plan`
`Level 2 (A102) | Floor Plan`
`Level 3 (A103) | Floor Plan`

---

## ✨ Features

* **Instant Translation:** C# implementation ensures high-speed processing, even in models with thousands of elements.
* **Bulk Renaming:** Apply patterns to Categories, Selection Sets, or the entire Project.
* **Preview Mode:** See exactly how the values will change before committing the transaction to the Revit database.
* **Parameter Agnostic:** Works on any string-based parameter (Comments, Marks, Sheet Names, etc.).

---

## 🛠 Usage

1. **Select Elements:** Choose the elements you wish to modify.
2. **Define Logic:** Input your "Current Pattern" to map the existing string.
3. **Set Target:** Input your "New Pattern" to define the output.
4. **Execute:** Click ReValue to update the parameters across your selection.

---

## 📜 Credits

This tool is a C# translation of the **ReValue** tool originally found in the **[pyRevit](https://github.com/eirannejad/pyRevit)** framework created by Ehsan Iran-Nejad.

The logic and user workflow remain faithful to the original pyRevit implementation, while the engine has been rebuilt for the .NET environment to support the latest Revit versions (2024–2026).

---

### 🦀 Beyond Revit: Desktop Version

I find the logic of this tool so useful for general file management that I created a standalone executable version for the desktop.

If you need the same **Tag-Based Matching** power to rename files on your OS (music, photos, project folders, etc.) with the speed and safety of **Rust**, check out my other repository:

👉 **[File Renamer (Rust Version)](https://github.com/GastonBC/file_renamer)**