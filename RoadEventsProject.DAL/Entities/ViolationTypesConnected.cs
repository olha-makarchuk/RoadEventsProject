using System;
using System.Collections.Generic;

namespace RoadEventsProject.DAL.Entities;

public partial class ViolationTypesConnected
{
    public int IdViolationTypesConnected { get; set; }

    public int IdType { get; set; }

    public int IdViolation { get; set; }

    public virtual TypeViolation IdTypeNavigation { get; set; } = null!;

    public virtual Violation IdViolationNavigation { get; set; } = null!;
}
