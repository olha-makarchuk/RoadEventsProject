using System;
using System.Collections.Generic;

namespace RoadEventsProject.DAL.Entities;

public partial class Violation
{
    public int IdViolation { get; set; }

    public int IdUser { get; set; }

    public decimal Fine { get; set; }

    public int IdCityVillage { get; set; }

    public int IdDriver { get; set; }

    public int IdVehicle { get; set; }

    public int IdRoadEvent { get; set; }

    public DateTime DateEvent { get; set; }

    public virtual CityVillage IdCityVillageNavigation { get; set; } = null!;

    public virtual Driver IdDriverNavigation { get; set; } = null!;

    public virtual RoadEvent IdRoadEventNavigation { get; set; } = null!;

    public virtual UserInfo IdUserNavigation { get; set; } = null!;

    public virtual Vehicle IdVehicleNavigation { get; set; } = null!;

    public virtual ICollection<ViolationTypesConnected> ViolationTypesConnecteds { get; set; } = new List<ViolationTypesConnected>();
}
