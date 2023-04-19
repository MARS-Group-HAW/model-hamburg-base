using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;
using SOHModel.Domain.Model.Layers;

namespace HamburgBaseModel.Model.Layers
{
    public interface IDoctorLayer : IVectorLayer<DoctorsOffice>, IModalLayer
    {
        DoctorsOffice Nearest(Position position, bool freeCapacity = true);
        
    }
}