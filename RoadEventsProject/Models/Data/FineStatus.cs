using System;
using System.Collections.Generic;

namespace RoadEventsProject.Models.Data;

public partial class FineStatus
{
    public int IdFineStatus { get; set; }

    public string NameFine { get; set; } = null!;

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
