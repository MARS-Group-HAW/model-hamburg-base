using HamburgBaseModel.Model.Layers;
using Mars.Interfaces.Annotations;
using Mars.Interfaces.Environments;
using SOHMultimodalModel.Model;

namespace HamburgBaseModel.Model.Agents
{
    /**
     * This is the agent type CityDriver. It starts at a fixed start position and travels to a goal position using a
     * range of travel modalities (walking, cycling, and driving). The travel time between start and goal is tracked.
     */
    public class CityDriver : MultiCapableAgent<UrbanResidentLayer>
    {
        
        #region Constructor
        
        public override void Init(UrbanResidentLayer urbanResidentLayer)
        {
            // Store a reference to urbanResidentLayer in a public property. The agent "lives" on this layer.
            UrbanResidentLayer = urbanResidentLayer;

            // Assign a position at which the CityDriver will be located at the beginning of the simulation
            // and a position to which it will travel
            StartPosition = Position.CreatePosition(9.912456, 53.558935);
            GoalPosition = Position.CreatePosition(9.9639571, 53.5524253);
            
            // Call Init method of base agent type (MultiCapableAgent) to enable multimodal features (i.e., using
            // cars, bicycles, etc.)
            base.Init(urbanResidentLayer);
            
            // Create a multi-modal route using the modality options (Capabilities) that are available to the agent
            MultimodalRoute = UrbanResidentLayer.RouteFinder.Search(this, StartPosition, GoalPosition, Capabilities);

        }
        
        #endregion

        #region Tick
        
        // The Tick method contains the agents behavior routine, which is executed during every time step of the
        // simulation. Here, the CityDriver moves from its StartPosition towards its GoalPosition one tick at a time.
        // The travel time (in seconds) is tracked by incrementing the TravelTime counter by 1 when the agent moves.
        // Once the agent has reached its destination, it no longer moves.
        public override void Tick()
        {
            if (!GoalReached)
            {
                Move();
                TravelTime += 1;
            }
        }
        
        #endregion

        #region Properties
        
        // A CityDriver's reference to the UrbanResidentLayer on which it "lives" (obtained from configuration files)
        [PropertyDescription] public UrbanResidentLayer UrbanResidentLayer { get; set; }
        
        // The destination of the CityDriver
        public Position GoalPosition { get; set; }
        
        // The travel time (in seconds) spent in route from the StartPosition to the GoalPosition
        public int TravelTime { get; set; }
        
        #endregion
        
    }
}