﻿namespace RoadEventsProject.Models
{
    public class EventModel
    {
        public IFormFile? Photo { get; set; }
        public IFormFile? Video { get; set; }

        public string? DescriptionEvent { get; set; }
        public int IdRegion { get; set; }
        public int IdCityVillage { get; set; }
        public DateTime DateEvent { get; set;}
    }
}
