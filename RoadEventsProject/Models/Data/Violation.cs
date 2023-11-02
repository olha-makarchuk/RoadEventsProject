using System;
using System.Collections.Generic;

namespace RoadEventsProject.Models.Data;

public partial class Violation
{
    public int IdViolation { get; set; }

    public int IdUser { get; set; }

    public int IdFineStatus { get; set; }

    public decimal Fine { get; set; }

    public int IdCityVillage { get; set; }

    public int IdDriver { get; set; }

    public int IdVehicle { get; set; }

    public DateTime DateEvent { get; set; }

    public virtual CityVillage IdCityVillageNavigation { get; set; } = null!;

    public virtual Driver IdDriverNavigation { get; set; } = null!;

    public virtual FineStatus IdFineStatusNavigation { get; set; } = null!;

    public virtual UserInfo IdUserNavigation { get; set; } = null!;

    public virtual Vehicle IdVehicleNavigation { get; set; } = null!;

    public virtual ICollection<TypeViolation> TypeViolations { get; set; } = new List<TypeViolation>();
}
