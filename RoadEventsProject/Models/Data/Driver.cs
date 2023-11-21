using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RoadEventsProject.Models.Data;

public partial class Driver
{
    public int IdDriver { get; set; }

    public string IpnNumber { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
