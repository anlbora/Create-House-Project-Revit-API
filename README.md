# Simple House Project

## Overview

This project is a Revit add-in developed using the Revit API. The add-in allows users to create a simple house structure, including walls, floors, roofs, windows, and doors, directly within Autodesk Revit.

## Features

- **Create Walls**: Automatically generates walls based on specified dimensions.
- **Add Windows**: Inserts windows into the created walls.
- **Add Doors**: Inserts doors into the created walls.
- **Create Floor**: Generates a floor based on specified dimensions.
- **Create Roof**: Creates a roof structure with specified dimensions and slope.

## Requirements

- Autodesk Revit
- Revit API

## Installation

1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Build the solution to generate the add-in DLL file.
4. Load the add-in in Autodesk Revit to use the functionality.

## Usage

1. Launch Autodesk Revit.
2. Load the Simple House Project add-in.
3. Use the custom Ribbon Panel to access the 'Create House' button.
   
  ![1](https://github.com/anlbora/Create-House-Project-Revit-API/assets/100442507/4df000eb-2919-4c40-87d8-ff6a1dd21221)

  ![2](https://github.com/anlbora/Create-House-Project-Revit-API/assets/100442507/1f497520-6b06-4cdb-a7d9-841d7ca458ef)
   
4. Follow the on-screen instructions to specify dimensions and other parameters.

  ![3](https://github.com/anlbora/Create-House-Project-Revit-API/assets/100442507/94fe64a7-ec49-4bc6-ad34-93cfe0499842)
  
  ![4](https://github.com/anlbora/Create-House-Project-Revit-API/assets/100442507/6f4af9ce-f4ef-4596-887d-1fe0d972be3d)
  
5. Click 'Create House' to initiate the house creation process.

  ![5](https://github.com/anlbora/Create-House-Project-Revit-API/assets/100442507/058a0ba6-5286-4071-8b6b-b4222a3e8983)
  
  ![6](https://github.com/anlbora/Create-House-Project-Revit-API/assets/100442507/2f187745-8384-4d23-bbcf-0ae3a5b4f311)

## Structure

- `MainUI`: Represents the main user interface form where users can input dimensions and select materials.
- `Application`: Represents the main entry point of the add-in, responsible for creating the custom Ribbon Panel.
- `main`: Represents the external command class that executes the main logic of the add-in.

## Future Improvements

### User Interface Enhancements
- **Advanced Configuration Options**
  - **Windows and Doors Customization**: Enable users to specify the types, quantities, and placement of windows and doors within walls.
  - **Sill Height Specification**: Provide an option to define the height of window sills.
  - **Material Selection**: Allow users to select materials for walls, floors, roofs, and potentially ceilings.
  - **Additional Creation Tools**: Introduce dedicated buttons for "Create Window" and "Create Door" to streamline the process of adding doors and windows.
  - **Multi-floor Capability**: Implement functionality to design multi-story structures.
  - **Dimensional Annotations**: Introduce an "Add Dimensions" feature to automatically annotate dimensions on the floor plan.
  - **Document Export Options**: Add methods for creating sheets, exporting to PDF, and exporting to DWG format.

- **Visual Feedback Mechanisms**: Enhance the user experience by providing real-time visual feedback during the creation process.

### Additional Features
- **Furniture and Fixture Integration**: Incorporate a library of standard furniture and fixtures to allow users to furnish their designs.
- **Custom Templates**: Enable users to save and load custom templates for quicker project setup and consistency.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
