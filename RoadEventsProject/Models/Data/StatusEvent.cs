using System;
using System.Collections.Generic;

namespace RoadEventsProject.Models.Data;

public partial class StatusEvent
{
    public int IdStatus { get; set; }

    public string NameStatus { get; set; } = null!;

    public virtual ICollection<RoadEvent> RoadEvents { get; set; } = new List<RoadEvent>();
}
