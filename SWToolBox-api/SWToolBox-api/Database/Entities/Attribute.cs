using System;
using System.Collections.Generic;

namespace SWToolBox_api.Database.Entities;

public partial class Attribute
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? AdvantageAgainstId { get; set; }

    public virtual Attribute? AdvantageAgainst { get; set; }

    public virtual ICollection<Attribute> InverseAdvantageAgainst { get; set; } = new List<Attribute>();

    public virtual ICollection<Monster> Monsters { get; set; } = new List<Monster>();
}
