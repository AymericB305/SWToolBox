using System;
using System.Collections.Generic;

namespace SWToolBox_api.Database.Entities;

public partial class LeaderType
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<LeaderSkill> LeaderSkills { get; set; } = new List<LeaderSkill>();
}
