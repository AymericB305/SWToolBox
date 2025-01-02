using System;
using System.Collections.Generic;

namespace SWToolBox_api.Database.Entities;

public partial class Rank
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<GuildPlayer> GuildPlayers { get; set; } = new List<GuildPlayer>();
}
