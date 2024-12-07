using System;
using System.Collections.Generic;

namespace SWToolBox_api.Database.Entities;

public partial class GuildDefense
{
    public Guid GuildId { get; set; }

    public Guid DefenseId { get; set; }

    public string Description { get; set; } = null!;

    public virtual Defense Defense { get; set; } = null!;

    public virtual Guild Guild { get; set; } = null!;
}
