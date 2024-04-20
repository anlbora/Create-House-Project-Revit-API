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
4. Click 'Create House' to initiate the house creation process.
5. Follow the on-screen instructions to specify dimensions and other parameters.

## Structure

- `MainUI`: Represents the main user interface form where users can input dimensions and select materials.
- `Application`: Represents the main entry point of the add-in, responsible for creating the custom Ribbon Panel.
- `main`: Represents the external command class that executes the main logic of the add-in.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
