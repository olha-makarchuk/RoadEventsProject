using RoadEventsProject.DAL.Entities;

namespace RoadEventsProject.Models
{
    public class AllViolationsTypesModel
    {
        public AllViolationsTypesModel()
        {
            Types = new List<ViolationTypesConnected>();
            Violations = new List<Violation>();
        }
        public List<Violation> Violations { get; set; }
        public List<ViolationTypesConnected> Types { get; set; }
    }
}
