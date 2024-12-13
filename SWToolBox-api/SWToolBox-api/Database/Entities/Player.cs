using System;
using System.Collections.Generic;

namespace SWToolBox_api.Database.Entities;

public partial class Player
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<GuildPlayer> GuildPlayers { get; set; } = new List<GuildPlayer>();

    public virtual ICollection<PlayerDefenseTower> PlayerDefenseTowers { get; set; } = new List<PlayerDefenseTower>();
}
