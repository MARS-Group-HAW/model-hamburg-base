using System;
using System.Collections.Generic;
using System.Linq;
using HamburgBaseModel.Model.Agents;
using Mars.Common.Core.Logging;
using Mars.Core.Data;
using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;
using SOHMultimodalModel.Multimodal;

namespace HamburgBaseModel.Model.Layers
{
    /**
     * The UrbanResidentLayer is the layer on which agents of the model (Patient and CityDriver) "live".
     * The layer is responsible for initializing the requested number of agents and performing other agent
     * management tasks during the simulation.
     */
    public class UrbanResidentLayer : AbstractMultimodalLayer
    {
        private static readonly ILogger Logger = LoggerFactory.GetLogger(typeof(UrbanResidentLayer));
        
        #region Properties

        /// <summary>
        ///     Collection storing all agents that have been registered and spawned on the layer
        /// </summary>
        public IDictionary<Guid, Patient> PatientMap { get; set; }
        
        /// <summary>
        ///     Collection storing all agents that have been registered and spawned on the layer
        /// </summary>
        public IDictionary<Guid, CityDriver> CityDriverMap { get; set; }

        // **********************************
        // References to travel networks that are associated with the PatientLayer:
        // **********************************

        /// <summary>
        ///     Network environment mapping the pedestrian sidewalks of Hamburg
        /// </summary>
        public ISpatialGraphEnvironment SidewalkEnvironment => SpatialGraphMediatorLayer.Environment;

        #endregion

        #region Constructor

        // A layer's initLayer method serves to register agents (in this case, Patient) and entities
        // (in this case, Car) that reside and move on the layer.
        public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle = null,
            UnregisterAgent unregisterAgentHandle = null)
        {
            // call the super class's InitLayer method to register agents on the layer
            base.InitLayer(layerInitData, registerAgentHandle, unregisterAgentHandle);

            // spawn agents on layer and store them in PatientMap collection
            var agentManager = layerInitData.Container.Resolve<IAgentManager>();

            PatientMap = agentManager.Spawn<Patient, UrbanResidentLayer>().ToDictionary(patient => patient.ID);
            CityDriverMap = agentManager.Spawn<CityDriver, UrbanResidentLayer>().ToDictionary(cityDriver => cityDriver.ID);

            Logger.LogInfo("Created Patients: " + PatientMap.Count);
            Logger.LogInfo("Created City Drivers: " + CityDriverMap.Count);

            return PatientMap.Count != 0 || CityDriverMap.Count != 0;
        }

        #endregion

    }
}