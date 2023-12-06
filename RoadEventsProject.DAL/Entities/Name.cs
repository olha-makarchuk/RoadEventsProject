using System;
using System.Collections.Generic;

namespace RoadEventsProject.DAL.Entities;

public partial class Name
{
    public int IdName { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual ICollection<UserInfo> UserInfos { get; set; } = new List<UserInfo>();
}
