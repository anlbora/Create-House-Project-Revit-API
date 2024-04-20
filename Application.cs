using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Diagnostics;
using System.Reflection;

namespace SimpleHouseProject
{
    /// <summary>
    /// External application class to initialize and manage the Revit add-in.
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Application : IExternalApplication
    {
        /// <summary>
        /// Called when Revit is shutting down.
        /// </summary>
        /// <param name="application">The controlled application.</param>
        /// <returns>Result.Succeeded if successful.</returns>
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        /// <summary>
        /// Called when Revit starts up.
        /// </summary>
        /// <param name="application">The controlled application.</param>
        /// <returns>Result.Succeeded if successful.</returns>
        public Result OnStartup(UIControlledApplication application)
        {
            // Create the ribbon panel and add the push button to it
            RibbonPanel panel = RibbonPanel(application);
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            if (panel.AddItem(new PushButtonData("main", "Create House", thisAssemblyPath, "SimpleHouseProject.main"))
                is PushButton button)
            {
                button.ToolTip = "Creating a house";
            }

            return Result.Succeeded;
        }

        /// <summary>
        /// Creates or retrieves the ribbon panel for the add-in.
        /// </summary>
        /// <param name="application">The controlled application.</param>
        /// <returns>The ribbon panel for the add-in.</returns>
        public RibbonPanel RibbonPanel(UIControlledApplication application)
        {
            string tab = "HOUSE";
            RibbonPanel ribbonPanel = null;

            // Attempt to create a new ribbon tab
            try
            {
                application.CreateRibbonTab(tab);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            // Attempt to create a new ribbon panel
            try
            {
                application.CreateRibbonPanel(tab, "Create House");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            // Retrieve the ribbon panel
            List<RibbonPanel> panelList = application.GetRibbonPanels(tab);
            foreach (RibbonPanel panel in panelList.Where(p => p.Name == "Create House"))
            {
                ribbonPanel = panel;
            }

            return ribbonPanel;
        }
    }
}
