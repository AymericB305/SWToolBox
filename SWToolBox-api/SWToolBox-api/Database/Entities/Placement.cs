using System;
using System.Collections.Generic;

namespace SWToolBox_api.Database.Entities;

public partial class Placement
{
    public Guid PlayerId { get; set; }

    public Guid DefenseId { get; set; }

    public short Wins { get; set; }

    public short Losses { get; set; }

    public string Comment { get; set; } = null!;

    public long TowerId { get; set; }

    public Guid Id { get; set; }

    public virtual Defense Defense { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;

    public virtual Tower Tower { get; set; } = null!;
}
