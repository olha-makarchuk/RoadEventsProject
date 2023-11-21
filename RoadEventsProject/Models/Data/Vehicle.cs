using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RoadEventsProject.Models.Data;

public partial class Vehicle
{
    public int IdVehicle { get; set; }

    public int IdDriver { get; set; }

    public string NumberCar { get; set; } = null!;
    [JsonIgnore]
    public virtual Driver IdDriverNavigation { get; set; } = null!;

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
