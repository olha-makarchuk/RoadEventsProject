using System;
using System.Collections.Generic;

namespace RoadEventsProject.Models.Data;

public partial class Video
{
    public int IdVideo { get; set; }

    public string VideoUrl { get; set; } = null!;

    public virtual ICollection<RoadEvent> RoadEvents { get; set; } = new List<RoadEvent>();
}
