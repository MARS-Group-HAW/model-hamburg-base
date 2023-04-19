<h1 align="center">Hamburg Base Model (HBM)</h1>

The Hamburg Base Model (HBM) is a base model for developing agent-based models and running simulations for the city of Hamburg, Germany. As a base model, it contains and provides georeferenced information (GIS data) pertaining to the entirety of Hamburg (see below for a list of layer types). The HBM is based on the SmartOpenHamburg (SOHH) model (https://smartopenhamburg.de) designed by the MARS group (https://mars-group.org), which is a traffic model and digital twin of Hamburg, Germany.

## Requirements

The following system requirements need to be met to build and run MARS models:

- .NET Core (https://dotnet.microsoft.com/download)
- JetBrains Rider IDE (https://www.jetbrains.com/de-de/rider/)
- Installing the MARS NuGet package (see below)

## IDE Setup
- Clone the GitLab repository
- Open Rider
- Click on "Import Project from Solution"
- Select the HBM solution file: `C# Models/HamburgBaseModels/HamburgBaseModel.sln`
- Once the project has loaded, click on "NuGet" (in the footer bar of Rider)
- Enter "Mars.Life.Simulations" in the search bar
- Install the package "Mars.Life.Simulations" for both the `HamburgBaseModel` project and the `SOHModel` project
- The IDE should now be able to resolve the project dependencies and imports

## Simulation

- In Rider, open the HBM's main file: `HamburgBaseModel/HamburgBaseModel/Program.cs`
- Run `Main(string[] args)`

## Input Data

Below is a list of some of the layer data provided along ith the HBM:
- `HamburgBaseModel/Resources/altona_mitte`: these are geodata for the districts Ottensen, Altona-Altstadt, St. Pauli, Altona-Nord, and Sternschanze
- `HamburgBaseModel/Resources/hamburg`: these are geodata for the Hamburg metropolitan area

## Visualisation

By default, simulations produce a file `trips.geojson` in the bin folder that is auto-generated when the project is compiled and run. This file contains the movement trajectories of all agents that moved during the simulation. To visualize the trajectories:
- Go to https://kepler.gl
- Click on "Get Started"
- Drag-and-drop the GEOJSON file into the window
- Click the play button to start the visualization. You can make adjustments to the visualization in the left sidebar.
