using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;

namespace OrientBoxToFace.Commands
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class StartupCommand : ExternalCommand
    {
        public override void Execute()
        {
            UIDocument uidoc = this.UiDocument;
            Document doc = uidoc.Document;
            if (!(uidoc.ActiveView is View3D view3D))
            {
                TaskDialog.Show("Error", "You must be in a 3D view.");
                return;
            }

            try
            {
                Reference faceRef = uidoc.Selection.PickObject(ObjectType.Face, "Pick a face to align to");
                Element element = doc.GetElement(faceRef);
                Face face = element.GetGeometryObjectFromReference(faceRef) as Face;

                if (face == null) return;

                // Get World Normal of the selected face
                XYZ worldNormal;
                if (element is FamilyInstance fi)
                {
                    Transform transform = fi.GetTransform();
                    worldNormal = transform.OfVector(face.ComputeNormal(new UV(0.5, 0.5))).Normalize();
                }
                else
                {
                    worldNormal = face.ComputeNormal(new UV(0.5, 0.5)).Normalize();
                }

                // 3. Get Section Box and determine the "Long" axis
                BoundingBoxXYZ box = view3D.GetSectionBox();

                double lengthX = box.Max.X - box.Min.X;
                double lengthY = box.Max.Y - box.Min.Y;

                // We want to align the 'Long' side's normal to the face normal
                // If X is longer, the face of the box is controlled by BasisY.
                // If Y is longer, the face of the box is controlled by BasisX.
                XYZ boxReferenceVector;
                if (lengthX >= lengthY)
                {
                    boxReferenceVector = box.Transform.BasisY.Normalize();
                }
                else
                {
                    boxReferenceVector = box.Transform.BasisX.Normalize();
                }

                // Calculate Signed Angle around Z Axis (Flattened to XY plane)
                XYZ v1 = new XYZ(boxReferenceVector.X, boxReferenceVector.Y, 0).Normalize();
                XYZ v2 = new XYZ(worldNormal.X, worldNormal.Y, 0).Normalize();

                double angle = v1.AngleTo(v2);

                // Cross product determines if we rotate clockwise or counter-clockwise
                XYZ cross = v1.CrossProduct(v2);
                if (cross.Z < 0) angle = -angle;

                // Calculate Rotation Point (World center of the current box)
                XYZ centerLocal = (box.Max + box.Min) * 0.5;
                XYZ centerWorld = box.Transform.OfPoint(centerLocal);

                Transform rotation = Transform.CreateRotationAtPoint(XYZ.BasisZ, angle, centerWorld);
                box.Transform = rotation.Multiply(box.Transform);

                using (Transaction t = new Transaction(doc, "Align section box to face"))
                {
                    t.Start();
                    view3D.SetSectionBox(box);
                    t.Commit();
                }

                uidoc.RefreshActiveView();
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                // User pressed Esc
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }
    }
}

