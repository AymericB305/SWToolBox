using System;
using System.Collections.Generic;

namespace SWToolBox_api.Database.Entities;

public partial class LeaderSkill
{
    public int Value { get; set; }

    public long TypeId { get; set; }

    public long LeaderTypeId { get; set; }

    public Guid Id { get; set; }

    public virtual LeaderType LeaderType { get; set; } = null!;

    public virtual ICollection<Monster> Monsters { get; set; } = new List<Monster>();

    public virtual Type Type { get; set; } = null!;
}
