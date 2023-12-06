using System;
using System.Collections.Generic;

namespace RoadEventsProject.DAL.Entities;

public partial class Driver
{
    public int IdDriver { get; set; }

    public string IpnNumber { get; set; } = null!;

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
