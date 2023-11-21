using System;
using System.Collections.Generic;

namespace RoadEventsProject.Models.Data;

public partial class TypeViolation
{
    public int IdType { get; set; }

    public string NameType { get; set; } = null!;

    public virtual ICollection<ViolationTypesConnected> ViolationTypesConnecteds { get; set; } = new List<ViolationTypesConnected>();
}
