using System;
using HamburgBaseModel.Misc;
using HamburgBaseModel.Model.Layers;
using Mars.Interfaces.Annotations;
using SOHModel.Multimodal.Model.Agents;

namespace HamburgBaseModel.Model.Agents
{
    /**
     * This is the agent type Patient. If a Patient is sick, it goes to the nearest doctor's office, stays there
     * for some amount of time, and then returns home.
     */
    public class Patient : MultiCapableAgent<UrbanResidentLayer>
    {
        
        #region Constructor
        
        // An agent's Init method is used to initialize the agent and set the property values
        // that are requires at the start of the simulation.
        public override void Init(UrbanResidentLayer urbanResidentLayer)
        {
            // Store a reference to patientLayer in public property. The agent "live" on this layer.
            UrbanResidentLayer = urbanResidentLayer;

            // call Init method of base agent type (MultiCapableAgent) to enable multimodal features (i.e., cars)
            base.Init(urbanResidentLayer);

            var randomPosition = UrbanResidentLayer.SidewalkEnvironment.GetRandomNode().Position;
            var home = VectorBuildingsLayer.Nearest(randomPosition);
            Position = home.Position;
            StartPosition = home.Position;

            // In the resident initialization file, the attribute "activity" is used to assign a routine to each
            // resident which it will pursue during the simulation. In this base model example, only the state "sick"
            // is processed. Logic for other states can be written in a similar fashion.

            // Activity:
            // Goal: make a multimodal trip from starting position to a doctor
            // Process:
            // 1. Find the nearest doctor.
            // 2. Set its location as goal position.
            // 3. Plan a multimodal route from start position to the goal position.
            // Technical aspects: interaction with a vector layer (DoctorsLayer) to find nearest node
            if (Activity == "sick")
            {
                CurrentActivity = AgentState.Sick;
                Activity = CurrentActivity.ToString();
                // The DoctorLayer can be queried with or without a rating parameter. The rating parameter
                // denotes the rating of the doctor's office by past patients.
                DoctorsOffice = DoctorLayer.Nearest(Position);
                // Search for a doctor's office that is nearest to current position and has at least the specified rating (between 0.0 and 1.0)
                // DoctorsOffice = DoctorLayer.Nearest(Position, 0.5);
                var goalPosition = DoctorsOffice.Position;
                // Plan a trip from home (Position) to the chosen DoctorsOffice
                MultimodalRoute = UrbanResidentLayer.RouteFinder.Search(this, Position, goalPosition, Capabilities);
                // Note: If the patient was unable to get an appointment at any DoctorsOffice, it remains home during
                // the simulation.
            }
        }
        
        #endregion

        #region Tick

        // The Tick method contains the agents behavior routine, which is executed during every time step of the
        // simulation. Here, the Patient travels to the DoctorsOffice at which is got an appointment. Once it has
        // arrived, it spend some amount of time (RemainingTime) there, and then returns home.
        public override void Tick()
        {
            if (UrbanResidentLayer.Context.CurrentTick >= StartTime)
            {
                // If Patient hasn't reached its destination (DoctorsOffice or its home) yet, continue moving towards it.
                if (!GoalReached)
                {
                    Move();
                }
                else
                {
                    // Patient has reached destination. Check if its current status is Sick
                    // (i.e., it has reached a DoctorsOffice and requires treatment).
                    if (CurrentActivity.Equals(AgentState.Sick))
                    {
                        var r = new Random();
                        RemainingTime = r.Next(600, 3600);
                        CurrentActivity = AgentState.InTreatment;
                        Activity = CurrentActivity.ToString();
                        var hasEntered = DoctorsOffice.Enter(this);
                        // Check if there is room (Capacity) for the Patient at the DoctorsOffice
                        if (!hasEntered)
                        {
                            // Patient did not get an appointment. Increment counter by 1 to track how many Patients did
                            // not get an appointment at the end of the simulation.
                            NoAppointmentCount += 1;
                        }
                    }

                    // Check if Patient still has time to spend at DoctorsOffice
                    if (RemainingTime != 0)
                    {
                        RemainingTime -= 1;
                        if (RemainingTime == 0)
                        {
                            // Patient's visit at DoctorsOffice is over. Create a route from the DoctorsOffice to home.
                            CurrentActivity = AgentState.Cured;
                            Activity = CurrentActivity.ToString();
                            DoctorsOffice.Leave(this);
                            // TODO: nächste Zeile auskommentieren
                            DoctorsOffice = null;
                            MultimodalRoute =
                                UrbanResidentLayer.RouteFinder.Search(this, Position, StartPosition, Capabilities);
                        }
                    }
                }
            }
        }

        #endregion

        #region Properties

        // The agent's current health status (the value mirrors that of the property "Activity". CurrentActivity is
        // included here to illustrate the use of Enums).
        public Enum CurrentActivity { get; set; }
        
        // An agent's current activity (initial activity is obtained from agent initialization file)
        [PropertyDescription(Name = "activity")]
        public string Activity { get; set; }

        // Increments to 1 if Patient was unable to find a DoctorsOffice with free capacity
        public int NoAppointmentCount { get; set; }

        // The number of remaining time steps the agent intends to spend at the doctor's office
        public long RemainingTime { get; set; }
        
        // Random start time between 06:00 and 09:00 (Number in Seconds (Ticks))
        public int StartTime { get; } = new Random().Next(1, 36000);
        
        // An agent's reference to the UrbanResidentLayer on which it "lives" (obtained from configuration files)
        [PropertyDescription] public UrbanResidentLayer UrbanResidentLayer { get; set; }

        // An agent's reference to the city's buildings (obtained from configuration files)
        [PropertyDescription] public VectorBuildingsLayer VectorBuildingsLayer { get; set; }

        // An agent's reference to the city's doctors (obtained from configuration files)
        [PropertyDescription] public DoctorLayer DoctorLayer { get; set; }

        // The DoctorsOffice the agent chooses to go to
        public DoctorsOffice DoctorsOffice { get; set; }

        #endregion

    }
}