using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace SimpleHouseProject
{
    /// <summary>
    /// Main external command class to execute the Revit add-in.
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class main : IExternalCommand
    {
        /// <summary>
        /// Gets or sets the current Revit document.
        /// </summary>
        public Document doc { get; set; }

        /// <summary>
        /// Gets or sets the current Revit UI application.
        /// </summary>
        public UIApplication uiApp { get; set; }

        /// <summary>
        /// Gets or sets the current Revit UI document.
        /// </summary>
        public UIDocument uidoc { get; set; }

        /// <summary>
        /// Executes the main logic of the Revit add-in.
        /// </summary>
        /// <param name="commandData">The external command data.</param>
        /// <param name="message">The message to return if the command fails.</param>
        /// <param name="elements">The set of elements to which the command applies.</param>
        /// <returns>Result.Succeeded if the command succeeds.</returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Initialize Revit API objects
            uiApp = commandData.Application;
            uidoc = uiApp.ActiveUIDocument;
            doc = uidoc.Document;

            // Create and display the main UI form
            MainUI mainPage = new MainUI(uidoc, doc);
            mainPage.ShowDialog();

            return Result.Succeeded;
        }
    }
}
