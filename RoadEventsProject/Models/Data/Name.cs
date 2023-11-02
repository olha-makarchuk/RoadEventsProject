using System;
using System.Collections.Generic;

namespace RoadEventsProject.Models.Data;

public partial class Name
{
    public int IdName { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

    public virtual ICollection<UserInfo> UserInfos { get; set; } = new List<UserInfo>();
}
