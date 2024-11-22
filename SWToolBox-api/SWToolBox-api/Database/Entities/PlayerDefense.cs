using System;
using System.Collections.Generic;

namespace SWToolBox_api.Database.Entities;

public partial class PlayerDefense
{
    public Guid PlayerId { get; set; }

    public Guid DefenseId { get; set; }

    public short? Wins { get; set; }

    public short? Losses { get; set; }

    public string Name { get; set; } = null!;

    public virtual Defense Defense { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;
}
