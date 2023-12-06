using System;
using System.Collections.Generic;

namespace RoadEventsProject.DAL.Entities;

public partial class Region
{
    public int IdRegion { get; set; }

    public string NameRegion { get; set; } = null!;

    public virtual ICollection<CityVillage> CityVillages { get; set; } = new List<CityVillage>();
}
