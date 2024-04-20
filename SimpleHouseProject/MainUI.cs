using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleHouseProject
{
    public partial class MainUI : System.Windows.Forms.Form
    {

        private Document doc = null;
        private UIDocument uidoc = null;

        private float width;
        private float height;
        private float length;

        /// <summary>
        /// Initializes a new instance of the MainUI class.
        /// </summary>
        /// <param name="uidoc">The current UI document.</param>
        /// <param name="doc">The Revit document.</param>
        public MainUI(UIDocument uidoc, Document doc)
        {
            InitializeComponent();

            // Initialize the UIDocument and Document properties
            this.uidoc = uidoc;
            this.doc = doc;

            // Retrieve and populate the list of materials
            List<string> materials = GetMaterials();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            
        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Event handler for the button click event to create the building structure.
        /// Validates the user input for dimensions and invokes the methods to create walls, floor, and roof.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void btn_create_Click(object sender, EventArgs e)
        {
            // Initialize variables to hold the dimensions entered by the user
            double width, height, length;

            // Validate and parse the width, height, and length from the text boxes
            if (!double.TryParse(txt_width.Text, out width) ||
                !double.TryParse(txt_height.Text, out height) ||
                !double.TryParse(txt_length.Text, out length))
            {
                // Display an error message if the dimensions are invalid
                MessageBox.Show("Please enter valid dimensions.");
                return;
            }

            // Invoke the methods to create the building structure components
            CreateWall(length, height, width);  // Pass the dimensions to the CreateWall method
            CreateFloor(length, width);
            CreateRoof(length, width, height);
        }

        /// <summary>
        /// Retrieves a list of unique material names from the Revit document.
        /// </summary>
        /// <returns>A list of unique material names.</returns>
        private List<string> GetMaterials()
        {
            // Initialize a list to hold the material names
            List<string> materials = new List<string>();

            // Retrieve materials from Revit document
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(Material));

            // Iterate through the collected materials and add their names to the list
            foreach (Material material in collector)
            {
                materials.Add(material.Name);
            }

            // Return a distinct list of material names
            return materials.Distinct().ToList();
        }

        /// <summary>
        /// Creates walls for a room with specified dimensions and adds windows and a door to the walls.
        /// </summary>
        /// <param name="length">The length of the room.</param>
        /// <param name="height">The height of the walls.</param>
        /// <param name="width">The width of the room.</param>
        private void CreateWall(double length, double height, double width)
        {
            // Start a new transaction for wall creation
            Transaction transaction = new Transaction(doc, "Create Walls for Room");

            try
            {
                transaction.Start();

                // Define the points for each wall segment
                XYZ startPoint1 = new XYZ(0, 0, 0);
                XYZ endPoint1 = new XYZ(length, 0, 0);
                Line wallLine1 = Line.CreateBound(startPoint1, endPoint1);

                XYZ startPoint2 = endPoint1;
                XYZ endPoint2 = new XYZ(length, width, 0);
                Line wallLine2 = Line.CreateBound(startPoint2, endPoint2);

                XYZ startPoint3 = endPoint2;
                XYZ endPoint3 = new XYZ(0, width, 0);
                Line wallLine3 = Line.CreateBound(startPoint3, endPoint3);

                XYZ startPoint4 = endPoint3;
                XYZ endPoint4 = new XYZ(0, 0, 0);
                Line wallLine4 = Line.CreateBound(startPoint4, endPoint4);

                // Retrieve the wall type named "Generic - 12\""
                WallType wallType = null;
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(WallType));

                foreach (WallType wt in collector)
                {
                    if (wt.Name.Contains("Generic - 12\""))
                    {
                        wallType = wt;
                        break;
                    }
                }

                // Get the level for wall placement
                Level level = GetLevel(); // Assume this method returns the first level found

                // Create the four walls for the room
                Wall wall1 = Wall.Create(doc, wallLine1, wallType.Id, level.Id, height, 0, false, false);
                Wall wall2 = Wall.Create(doc, wallLine2, wallType.Id, level.Id, height, 0, false, false);
                Wall wall3 = Wall.Create(doc, wallLine3, wallType.Id, level.Id, height, 0, false, false);
                Wall wall4 = Wall.Create(doc, wallLine4, wallType.Id, level.Id, height, 0, false, false);

                // Create windows if create windows checkbox "yes" checked
                if (!chkBox_WindowNo.Checked && chkBox_WindowYes.Checked)
                {

                    // Define the positions for the windows and door
                    XYZ window1_xyz = new XYZ((length / 2) / 2, 0, 4);
                    XYZ window2_xyz = new XYZ(((length / 2) / 2) * 3, 0, 4);
                    XYZ window3_xyz = new XYZ(length, width / 2, 4);
                    XYZ window4_xyz = new XYZ(length / 2, width, 4);
                    XYZ window5_xyz = new XYZ(0, width / 2, 4);

                    // Add windows to the walls
                    AddWindowToWall(wall1, window1_xyz, 2, 2);
                    AddWindowToWall(wall1, window2_xyz, 2, 2);
                    AddWindowToWall(wall2, window3_xyz, 2, 2);
                    AddWindowToWall(wall3, window4_xyz, 2, 2);
                    AddWindowToWall(wall4, window5_xyz, 2, 2);
                }

                // Create windows if create door checkbox "yes" checked
                if (!chkBox_DoorNo.Checked && chkBox_DoorYes.Checked)
                {
                    XYZ door_xyz = new XYZ(length / 2, 0, 0);
                    // Add door to the wall 1
                    AddDoorToWall(wall1, door_xyz, 2, 2);
                }

                // Commit the transaction
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error and display an error message
                transaction.RollBack();
                TaskDialog.Show("Error", ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the first level found in the project.
        /// </summary>
        /// <returns>The first level found in the project.</returns>
        private Level GetLevel()
        {
            // Create a collector to retrieve levels from the Revit document
            FilteredElementCollector collector = new FilteredElementCollector(doc);

            // Filter the collector to only include Level elements
            collector.OfClass(typeof(Level));

            // Retrieve the first level from the filtered elements
            Level level = collector.FirstElement() as Level;

            // Return the retrieved level
            return level;
        }

        /// <summary>
        /// Creates a floor with specified dimensions.
        /// </summary>
        /// <param name="length">The length of the floor.</param>
        /// <param name="width">The width of the floor.</param>
        private void CreateFloor(double length, double width)
        {
            Transaction transaction = new Transaction(doc, "Create Floor");

            try
            {
                transaction.Start();

                // Define floor profile
                XYZ first = new XYZ(0, 0, 0);
                XYZ second = new XYZ(length, 0, 0);
                XYZ third = new XYZ(length, width, 0);
                XYZ fourth = new XYZ(0, width, 0);

                CurveLoop profile = new CurveLoop();
                profile.Append(Line.CreateBound(first, second));
                profile.Append(Line.CreateBound(second, third));
                profile.Append(Line.CreateBound(third, fourth));
                profile.Append(Line.CreateBound(fourth, first));

                // Get the default floor type
                ElementId floorTypeId = Floor.GetDefaultFloorType(doc, false);

                // Retrieve the first level from the project
                Level level = GetLevel();

                // Create a flat floor using the defined profile, default type, and level
                Floor.Create(doc, new List<CurveLoop> { profile }, floorTypeId, level.Id);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                // If an error occurs during the transaction, rollback changes and display error message
                transaction.RollBack();
                TaskDialog.Show("Error", ex.Message);
            }
        }

        /// <summary>
        /// Creates a new level with the specified height above the highest existing level.
        /// </summary>
        /// <param name="height">The height of the new level.</param>
        /// <returns>The newly created level.</returns>
        private Level CreateNewLevel(double height)
        {
            Level newLevel = null;

            try
            {
                // Create a new level at the specified height
                newLevel = Level.Create(doc, height);
            }
            catch (Exception ex)
            {
                // If an error occurs during the transaction, display error message
                TaskDialog.Show("Error", ex.Message);
            }

            return newLevel;
        }

        /// <summary>
        /// Adds a window to the specified wall at the given position with the specified dimensions.
        /// </summary>
        /// <param name="wall">The wall to which the window will be added.</param>
        /// <param name="position">The position of the window.</param>
        /// <param name="width">The width of the window.</param>
        /// <param name="height">The height of the window.</param>
        private void AddWindowToWall(Wall wall, XYZ position, double width, double height)
        {
            // Filter to find a WindowType to use
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_Windows);

            FamilySymbol windowSymbol = collector.FirstElement() as FamilySymbol;

            // Check if a window type is found
            if (windowSymbol == null)
            {
                TaskDialog.Show("Error", "No window type found.");
                return;
            }

            // Check if the window type is not loaded
            if (!windowSymbol.IsActive)
            {
                windowSymbol.Activate();
                doc.Regenerate();
            }

            try
            {
                // Create a new FamilyInstance (Window) at the desired position
                FamilyInstance windowInstance = doc.Create.NewFamilyInstance(position, windowSymbol, wall, GetLevel(), StructuralType.NonStructural);

                // Set the window instance width and height
                Parameter widthParam = windowInstance.get_Parameter(BuiltInParameter.WINDOW_WIDTH);
                Parameter heightParam = windowInstance.get_Parameter(BuiltInParameter.WINDOW_HEIGHT);

                if (widthParam != null)
                {
                    widthParam.Set(width);
                }

                if (heightParam != null)
                {
                    heightParam.Set(height);
                }
            }
            catch (Exception ex)
            {
                // If an error occurs, display error message
                TaskDialog.Show("Error", ex.Message);
            }
        }

        /// <summary>
        /// Adds a door to the specified wall at the given position with the specified dimensions.
        /// </summary>
        /// <param name="wall">The wall to which the door will be added.</param>
        /// <param name="position">The position of the door.</param>
        /// <param name="width">The width of the door.</param>
        /// <param name="height">The height of the door.</param>
        private void AddDoorToWall(Wall wall, XYZ position, double width, double height)
        {
            // Filter to find a DoorType to use
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_Doors);

            FamilySymbol doorSymbol = collector.FirstElement() as FamilySymbol;

            // Check if a door type is found
            if (doorSymbol == null)
            {
                TaskDialog.Show("Error", "No door type found.");
                return;
            }

            // Check if the door type is not loaded
            if (!doorSymbol.IsActive)
            {
                doorSymbol.Activate();
                doc.Regenerate();
            }

            try
            {
                // Create a new FamilyInstance (Door) at the desired position
                FamilyInstance doorInstance = doc.Create.NewFamilyInstance(position, doorSymbol, wall, GetLevel(), StructuralType.NonStructural);

                // Set the door instance width and height
                Parameter widthParam = doorInstance.get_Parameter(BuiltInParameter.DOOR_WIDTH);
                Parameter heightParam = doorInstance.get_Parameter(BuiltInParameter.DOOR_HEIGHT);

                if (widthParam != null)
                {
                    widthParam.Set(width);
                }

                if (heightParam != null)
                {
                    heightParam.Set(height);
                }
            }
            catch (Exception ex)
            {
                // If an error occurs, display error message
                TaskDialog.Show("Error", ex.Message);
            }
        }

        /// <summary>
        /// Creates a roof with the specified dimensions and height.
        /// </summary>
        /// <param name="length">The length of the roof.</param>
        /// <param name="width">The width of the roof.</param>
        /// <param name="height">The height of the roof from the base level.</param>
        private void CreateRoof(double length, double width, double height)
        {
            Transaction transaction = new Transaction(doc, "Create Roof");

            try
            {
                transaction.Start();

                // Retrieve the RoofType from the Revit document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(RoofType));
                RoofType roofType = collector.FirstElement() as RoofType;

                // Check if a RoofType is found
                if (roofType == null)
                {
                    throw new Exception("No RoofType found.");
                }

                // Create a new level at the specified height
                Level roofLevel = CreateNewLevel(height);

                // Check if the new level is created successfully
                if (roofLevel == null)
                {
                    throw new Exception("Failed to create new level.");
                }

                // Define the footprint for the roof based on the provided dimensions
                CurveArray footprint = new CurveArray();

                XYZ startPoint = new XYZ(0, 0, 0);
                XYZ endPoint = new XYZ(length, 0, 0);
                footprint.Append(Line.CreateBound(startPoint, endPoint));

                startPoint = endPoint;
                endPoint = new XYZ(length, width, 0);
                footprint.Append(Line.CreateBound(startPoint, endPoint));

                startPoint = endPoint;
                endPoint = new XYZ(0, width, 0);
                footprint.Append(Line.CreateBound(startPoint, endPoint));

                startPoint = endPoint;
                endPoint = new XYZ(0, 0, 0);
                footprint.Append(Line.CreateBound(startPoint, endPoint));

                // Create footprint roof
                ModelCurveArray footPrintToModelCurveMapping = new ModelCurveArray();
                FootPrintRoof footprintRoof = doc.Create.NewFootPrintRoof(footprint, roofLevel, roofType, out footPrintToModelCurveMapping);
                ModelCurveArrayIterator iterator = footPrintToModelCurveMapping.ForwardIterator();

                // Set slope for each model curve of the roof
                iterator.Reset();
                while (iterator.MoveNext())
                {
                    ModelCurve modelCurve = iterator.Current as ModelCurve;
                    footprintRoof.set_DefinesSlope(modelCurve, true);
                    footprintRoof.set_SlopeAngle(modelCurve, 5.0 / 12.0); // 5/12 slope value
                }

                // Set base offset for the roof
                Parameter baseOffsetParam = footprintRoof.get_Parameter(BuiltInParameter.ROOF_BASE_LEVEL_PARAM);
                if (baseOffsetParam != null)
                {
                    baseOffsetParam.Set(height);
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                // If an error occurs, display error message and rollback the transaction
                transaction.RollBack();
                TaskDialog.Show("Error", ex.Message);
            }
        }
    }
}


