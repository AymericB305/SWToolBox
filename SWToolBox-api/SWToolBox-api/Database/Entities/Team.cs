using System;
using System.Collections.Generic;

namespace SWToolBox_api.Database.Entities;

public partial class Team
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<Tower> Towers { get; set; } = new List<Tower>();
}
