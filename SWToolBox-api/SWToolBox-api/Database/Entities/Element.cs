using System;
using System.Collections.Generic;

namespace SWToolBox_api.Database.Entities;

public partial class Element
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Monster> Monsters { get; set; } = new List<Monster>();
}
