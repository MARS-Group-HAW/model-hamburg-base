# Hamburg Base Model (HBM)

The Hamburg Base Model (HBM) is a base model for developing agent-based models and running simulations set in the city of Hamburg, Germany. As a base model, it provides georeferenced data spanning all of Hamburg (see below for a list of layer types). The HBM is builds on the [SmartOpenHamburg (SOH) model](https://www.mars-group.org/docs/tutorial/soh/) designed by the [MARS group](https://www.mars-group.org), which is a traffic model and digital twin of Hamburg, Germany.

## Requirements

The following system requirements need to be met to build and run MARS models:

- [.NET Core](https://dotnet.microsoft.com/download)
- [JetBrains Rider](https://www.jetbrains.com/de-de/rider/)
- Installing the MARS NuGet package (see below)

## IDE Setup

1. Clone the GitLab repository
2. Open Rider
3. Click on "Import Project from Solution"
4. Select the HBM solution file `HamburgBaseModel.sln`
5. Once the project has loaded, click on "NuGet" (in the footer bar of Rider)
6. Enter "Mars.Life.Simulations" in the search bar
7. Install the NuGet packages "Mars.Life.Simulations" and "Mars.Life.SOH"
8. The Rider IDE should now be able to resolve the project dependencies and imports

## Simulation

To run a simulation, follow these steps:

1. In Rider, open the HBM's main file: `HamburgBaseModel/Program.cs`
2. Run `Main(string[] args)`

## Input Data

Below is a list of some of the layer data provided along ith the HBM:

- `HamburgBaseModel/Resources/altona_mitte`: georeferenced environment data for the Hamburg districts Ottensen, Altona-Altstadt, St. Pauli, Altona-Nord, and Sternschanze
- `HamburgBaseModel/Resources/hamburg`: georeferenced environment data for the Hamburg metropolitan area

## Visualisation

By default, simulations produce a file `trips.geojson` in the directory `bin` that is auto-generated when the project is compiled and run. This file contains the movement trajectories of all agents that moved during the simulation. To visualize the trajectories:

- In an internet browser, open [kepler.gl](https://kepler.gl)
- Click on "Get Started"
- Drag-and-drop the GEOJSON file into the window
- Click the play button to start the visualization. You can make adjustments to the visualization in the left sidebar.
