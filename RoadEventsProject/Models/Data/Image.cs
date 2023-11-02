﻿using System;
using System.Collections.Generic;

namespace RoadEventsProject.Models.Data;

public partial class Image
{
    public int IdImage { get; set; }

    public string ImageUrl { get; set; } = null!;

    public virtual ICollection<RoadEvent> RoadEvents { get; set; } = new List<RoadEvent>();
}
