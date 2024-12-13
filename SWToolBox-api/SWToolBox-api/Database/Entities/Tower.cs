using System;
using System.Collections.Generic;

namespace SWToolBox_api.Database.Entities;

public partial class Tower
{
    public long Id { get; set; }

    public int Number { get; set; }

    public long TeamId { get; set; }

    public virtual ICollection<PlayerDefenseTower> PlayerDefenseTowers { get; set; } = new List<PlayerDefenseTower>();

    public virtual Team Team { get; set; } = null!;
}
