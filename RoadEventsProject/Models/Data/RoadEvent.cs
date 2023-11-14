using System;
using System.Collections.Generic;

namespace RoadEventsProject.Models.Data;

public partial class RoadEvent
{
    public int IdRoadEvent { get; set; }

    public int IdUser { get; set; }

    public int IdStatus { get; set; }

    public int? IdVideo { get; set; }

    public int? IdImage { get; set; }

    public int IdCityVillage { get; set; }

    public DateTime DateEvent { get; set; }

    public string? DescriptionEvent { get; set; }

    public virtual CityVillage IdCityVillageNavigation { get; set; } = null!;

    public virtual Image? IdImageNavigation { get; set; }

    public virtual StatusEvent IdStatusNavigation { get; set; } = null!;

    public virtual UserInfo IdUserNavigation { get; set; } = null!;

    public virtual Video? IdVideoNavigation { get; set; }
    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
