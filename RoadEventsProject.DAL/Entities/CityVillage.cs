using System;
using System.Collections.Generic;

namespace RoadEventsProject.DAL.Entities;

public partial class CityVillage
{
    public int IdCityVillage { get; set; }

    public int IdRegion { get; set; }

    public string NameCityVillage { get; set; } = null!;

    public virtual Region IdRegionNavigation { get; set; } = null!;

    public virtual ICollection<RoadEvent> RoadEvents { get; set; } = new List<RoadEvent>();

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
