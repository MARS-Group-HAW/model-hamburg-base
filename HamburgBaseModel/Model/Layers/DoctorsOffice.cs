using HamburgBaseModel.Model.Agents;
using Mars.Common.Core;
using Mars.Common.Core.Collections.NonBlockingDictionary;
using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;

namespace HamburgBaseModel.Model.Layers
{
    /**
     * A DoctorsOffice is held by the DoctorLayer. It represents a health facility in the model to which agents can
     * travel to get an appointment and treatment.
     */
    public class DoctorsOffice : IVectorFeature
    {
        #region Fields

        private ConcurrentDictionary<Patient, byte> _patientsInOffice;

        #endregion

        #region Constructors

        // In the Init method, the position, capacity, and rating of a DoctorsOffice is set.
        public void Init(ILayer layer, VectorStructuredData data)
        {
            VectorStructured = data;
            var centroid = VectorStructured.Geometry.Centroid;
            Position = Position.CreatePosition(centroid.X, centroid.Y);

            Capacity = VectorStructured.Attributes.Exists("capacity")
                ? VectorStructured.Attributes["capacity"].Value<int>()
                : 0;
            Rating = VectorStructured.Attributes.Exists("rating")
                ? VectorStructured.Attributes["rating"].Value<double>()
                : 0.0;
        }

        #endregion

        #region Properties

        // The position (x- and y-coordinate) of the DoctorsOffice
        public Position Position { get; private set; }

        // This property grants access to the field _patientsInOffice that holds the Patients currently in the
        // DoctorsOffice.
        public ConcurrentDictionary<Patient, byte> PatientsInOffice =>
            _patientsInOffice ??= new ConcurrentDictionary<Patient, byte>();

        // The number of Patients that can visit a DoctorsOffice simultaneously.
        public int Capacity { get; private set; }

        // Returns true if DoctorsOffice currently has free spaces for patients
        public bool HasCapacity => PatientsInOffice.Count < Capacity;
        
        // The rating of the DoctorsOffice
        public double Rating { get; private set; }

        public VectorStructuredData VectorStructured { get; set; }

        #endregion

        #region Methods

        // This method can be used by agents who have arrived at the DoctorsOffice to enter it.
        public bool Enter(Patient patient)
        {
            if (!HasCapacity || PatientsInOffice.ContainsKey(patient)) return false;

            return PatientsInOffice.TryAdd(patient, byte.MinValue);
        }

        // This method can be used by agents who are currently at the DoctorsOffice to leave it.
        public bool Leave(Patient patient)
        {
            var success = !PatientsInOffice.ContainsKey(patient) || PatientsInOffice.TryRemove(patient, out _);

            return success;
        }

        public void Update(VectorStructuredData data)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}