using System;
using System.Collections.Generic;

namespace SWToolBox_api.Database.Entities;

public partial class Defense
{
    public long MonsterLeadId { get; set; }

    public long Monster2Id { get; set; }

    public long Monster3Id { get; set; }

    public Guid Id { get; set; }

    public string Description { get; set; } = null!;

    public Guid GuildId { get; set; }

    public virtual Guild Guild { get; set; } = null!;

    public virtual Monster Monster2 { get; set; } = null!;

    public virtual Monster Monster3 { get; set; } = null!;

    public virtual Monster MonsterLead { get; set; } = null!;

    public virtual ICollection<Placement> Placements { get; set; } = new List<Placement>();
}
