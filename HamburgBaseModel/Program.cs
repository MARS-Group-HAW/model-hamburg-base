using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using HamburgBaseModel.Model.Agents;
using HamburgBaseModel.Model.Layers;
using Mars.Common.Core.Logging;
using Mars.Components.Starter;
using Mars.Core.Simulation;
using Mars.Interfaces;
using Mars.Interfaces.Model;
using SOHBicycleModel.Rental;
using SOHCarModel.Model;
using SOHCarModel.Parking;
using SOHDomain.Graph;

namespace HamburgBaseModel
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("EN-US");
            LoggerFactory.SetLogLevel(LogLevel.Off);

            var description = new ModelDescription();

            description.AddLayer<SpatialGraphMediatorLayer>(new[] { typeof(ISpatialGraphLayer) });

            description.AddLayer<BicycleRentalLayer>(new[] { typeof(IBicycleRentalLayer) });
            description.AddLayer<CarParkingLayer>(new[] { typeof(ICarParkingLayer) });

            description.AddLayer<DoctorLayer>(new[] { typeof(IDoctorLayer) });

            description.AddLayer<UrbanResidentLayer>();
            description.AddLayer<VectorBuildingsLayer>();

            description.AddAgent<Patient, UrbanResidentLayer>();
            description.AddAgent<CityDriver, UrbanResidentLayer>();
            
            description.AddEntity<RentalBicycle>();
            description.AddEntity<Car>();

            ISimulationContainer application;
            if (args != null && args.Any())
            {
                var container = CommandParser.ParseAndEvaluateArguments(description, args);

                var config = container.SimulationConfig;
                config.SimulationRunIteration = 1;

                application = SimulationStarter.BuildApplication(description, config);
            }
            else
            {
                var file = File.ReadAllText("config.json");
                var simConfig = SimulationConfig.Deserialize(file);
                application = SimulationStarter.BuildApplication(description, simConfig);
            }

            var simulation = application.Resolve<ISimulation>();


            var watch = Stopwatch.StartNew();
            var state = simulation.StartSimulation();

            watch.Stop();

            Console.WriteLine($"Executed iterations {state.Iterations} lasted {watch.Elapsed}");
            application.Dispose();
        }
    }
}