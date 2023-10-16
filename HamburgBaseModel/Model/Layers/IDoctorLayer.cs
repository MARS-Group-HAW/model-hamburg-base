using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;
using SOHDomain.Model;

namespace HamburgBaseModel.Model.Layers
{
    public interface IDoctorLayer : IVectorLayer<DoctorsOffice>, IModalLayer
    {
        DoctorsOffice Nearest(Position position, bool freeCapacity = true);
        
    }
}