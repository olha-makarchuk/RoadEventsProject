using System;
using System.Collections.Generic;

namespace RoadEventsProject.DAL.Entities;

public partial class RoleUser
{
    public int IdRole { get; set; }

    public string NameRole { get; set; } = null!;

    public virtual ICollection<UserInfo> UserInfos { get; set; } = new List<UserInfo>();
}
