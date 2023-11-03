using RoadEventsProject.Models.Data;

namespace RoadEventsProject.Models
{
    public class Event
    {
        public IFormFile? Photo { get; set; }
        public string? DescriptionEvent { get; set; }
        public string Region { get; set; }
        public string CityVillage { get; set; }
        public DateTime? DateEvent { get; set;}
    }
}
