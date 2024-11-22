using System;
using System.Collections.Generic;

namespace SWToolBox_api.Database.Entities;

public partial class LeaderSkill
{
    public long Id { get; set; }

    public int Value { get; set; }

    public long TypeId { get; set; }

    public virtual ICollection<Monster> Monsters { get; set; } = new List<Monster>();

    public virtual Type Type { get; set; } = null!;
}
