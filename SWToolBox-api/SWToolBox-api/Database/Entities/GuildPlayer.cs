﻿using System;
using System.Collections.Generic;

namespace SWToolBox_api.Database.Entities;

public partial class GuildPlayer
{
    public Guid GuildId { get; set; }

    public Guid PlayerId { get; set; }

    public DateTime JoinedAt { get; set; }

    public DateTime? LeftAt { get; set; }

    public virtual Guild Guild { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;
}