using RoadEventsProject.DAL.Entities;

namespace RoadEventsProject.Models
{
    public class ViolationAndTypesModel
    {
        public Violation ViolationModel { get; set; }
        public List<TypeViolation> TypesModel { get; set; }
        public List<int> SelectedViolationTypes { get; set; }
        public string NumberCar { get; set; }
    }
}
