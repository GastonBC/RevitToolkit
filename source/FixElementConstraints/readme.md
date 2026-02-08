## Fix Element Constraints

**Fix Element Constraints** automatically cleans up level assignments and offsets for a cleaner, more logical model. It ensures elements are hosted by their closest Level while maintaining their exact physical position.

### How it works

The tool analyzes an element's current elevation and compares it to the project's Level structure:

* **The Logic:** If a wall is hosted on **Level 1** with a **4000mm offset**, and **Level 2** is exactly at **4000mm**, the tool re-hosts the wall to **Level 2** with a **0 offset**.
* **The Result:** Your element stays in the same place, but your properties and schedules become much easier to manage.

### 🛠 Supported Elements

This tool works with most level-based Revit categories, including:

* Walls
* Columns
* Floors and Ceilings
* Roofs
* Most standard hosted components.