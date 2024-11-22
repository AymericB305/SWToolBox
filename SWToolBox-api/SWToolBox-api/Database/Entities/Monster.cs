using System;
using System.Collections.Generic;

namespace SWToolBox_api.Database.Entities;

public partial class Monster
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? LeaderId { get; set; }

    public long AttributeId { get; set; }

    public virtual Attribute Attribute { get; set; } = null!;

    public virtual ICollection<Defense> DefenseMonster2s { get; set; } = new List<Defense>();

    public virtual ICollection<Defense> DefenseMonster3s { get; set; } = new List<Defense>();

    public virtual ICollection<Defense> DefenseMonsterLeads { get; set; } = new List<Defense>();

    public virtual LeaderSkill? Leader { get; set; }
}
