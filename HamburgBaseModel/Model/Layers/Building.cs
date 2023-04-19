using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;

namespace HamburgBaseModel.Model.Layers
{
    /**
     * The Building class is used to represent building features of the city in the model.
     * A Building is held by the VectorBuildingsLayer.
     */
    public class Building : IVectorFeature
    {

        #region Constructor

        public void Init(ILayer layer, VectorStructuredData data)
        {
            var centroid = data.Geometry.Centroid;
            Position = Position.CreatePosition(centroid.X, centroid.Y);
            VectorStructured = data;
        }

        #endregion

        #region Properties

        // The position (x- and y-coordinate) of the Building
        public Position Position { get; private set; }
        
        public VectorStructuredData VectorStructured { get; set; }

        #endregion

        #region Methods

        public void Update(VectorStructuredData data)
        {
            //do nothing
        }

        #endregion
    }
}