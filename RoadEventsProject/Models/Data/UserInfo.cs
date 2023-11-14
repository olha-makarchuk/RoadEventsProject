using System;
using System.Collections.Generic;

namespace RoadEventsProject.Models.Data;

public partial class UserInfo
{
    public int IdUser { get; set; }

    public int IdName { get; set; }

    public string LoginUser { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int IdRole { get; set; }

    public bool Blocked { get; set; }

    public virtual Name IdNameNavigation { get; set; } = null!;

    public virtual RoleUser IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<RoadEvent> RoadEvents { get; set; } = new List<RoadEvent>();

    public virtual ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
