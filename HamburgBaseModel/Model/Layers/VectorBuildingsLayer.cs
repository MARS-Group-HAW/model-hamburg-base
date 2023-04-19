using System.Linq;
using Mars.Components.Layers;
using Mars.Interfaces.Environments;

namespace HamburgBaseModel.Model.Layers
{
    /**
     * The layer VectorBuildingsLayer holds the city's buildings, enabling agents to interact with them.
     */
    public class VectorBuildingsLayer : VectorLayer<Building>
    {
        public Building Nearest(Position position)
        {
            return Explore(position.PositionArray, -1, 1).FirstOrDefault().Value;
        }
    }
}