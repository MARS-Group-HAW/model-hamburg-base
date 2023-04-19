using System;
using System.Linq;
using Mars.Components.Layers;
using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;

namespace HamburgBaseModel.Model.Layers
{
    /**
     * The DoctorLayer is a vector layer that holds DoctorsOffice, enabling agents to interact with them.
     */
    public class DoctorLayer : VectorLayer<DoctorsOffice>, IDoctorLayer
    {

        #region Constructor

        // Layer initialization occurs by passing the input parameters to the base init method
        public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle,
            UnregisterAgent unregisterAgentHandle)
        {
            var succ = base.InitLayer(layerInitData, registerAgentHandle, unregisterAgentHandle);
            return succ;
        }

        #endregion

        #region Methods

        // This method allows agents to query the DoctorLayer for the DoctorsOffice that is nearest to their current
        // position.
        public DoctorsOffice Nearest(Position position, bool freeCapacity = true)
        {
            bool Predicate(DoctorsOffice doctorsOffice)
            {
                return !freeCapacity || doctorsOffice.HasCapacity;
            }
            
            return Explore(position.PositionArray, -1, 1, Predicate).FirstOrDefault()?.Value;
        }
        
        // Similar to the above method, but here, the agent can specify a required minimum rating for the DoctorsOffice.
        public DoctorsOffice Nearest(Position position, double minRating, bool freeCapacity = true)
        {
            bool Predicate(DoctorsOffice doctorsOffice)
            {
                return !freeCapacity || doctorsOffice.HasCapacity;
            }

            if (minRating == 0)
            {
                return Explore(position.PositionArray, -1, 1, Predicate).FirstOrDefault()?.Value;
            }

            var allDoctorOffices = Explore(position.PositionArray, -1, -1, Predicate);
            foreach (var doctorsOffice in allDoctorOffices)
            {
                if (doctorsOffice.Value.Rating >= minRating)
                {
                    return doctorsOffice.Value;
                }
            }
            Console.WriteLine("kein passendes DoctorsOffice gefunden");
            
            return Explore(position.PositionArray, -1, 1, Predicate).FirstOrDefault()?.Value;
        }

        #endregion
        
        #region Properties

        public ModalChoice ModalChoice { get; }
        
        #endregion

    }

}