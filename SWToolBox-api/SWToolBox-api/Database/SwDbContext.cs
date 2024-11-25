using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SWToolBox_api.Database.Entities;
using Attribute = SWToolBox_api.Database.Entities.Attribute;
using Type = SWToolBox_api.Database.Entities.Type;

namespace SWToolBox_api.Database;

public partial class SwDbContext : DbContext
{
    public SwDbContext(DbContextOptions<SwDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attribute> Attributes { get; set; }

    public virtual DbSet<Defense> Defenses { get; set; }

    public virtual DbSet<Guild> Guilds { get; set; }

    public virtual DbSet<GuildPlayer> GuildPlayers { get; set; }

    public virtual DbSet<LeaderSkill> LeaderSkills { get; set; }

    public virtual DbSet<LeaderType> LeaderTypes { get; set; }

    public virtual DbSet<Monster> Monsters { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayerDefense> PlayerDefenses { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("pgsodium", "key_status", new[] { "default", "valid", "invalid", "expired" })
            .HasPostgresEnum("pgsodium", "key_type", new[] { "aead-ietf", "aead-det", "hmacsha512", "hmacsha256", "auth", "shorthash", "generichash", "kdf", "secretbox", "secretstream", "stream_xchacha20" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "pgjwt")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("pgsodium", "pgsodium")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Attribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("attribute_pkey");

            entity.ToTable("attribute");

            entity.HasIndex(e => e.Id, "attribute_id_key").IsUnique();

            entity.HasIndex(e => e.Name, "attribute_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Defense>(entity =>
        {
            entity.HasKey(e => e.Uid).HasName("defense_pkey");

            entity.ToTable("defense");

            entity.Property(e => e.Uid)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("uid");
            entity.Property(e => e.Monster2Id).HasColumnName("monster_2_id");
            entity.Property(e => e.Monster3Id).HasColumnName("monster_3_id");
            entity.Property(e => e.MonsterLeadId).HasColumnName("monster_lead_id");

            entity.HasOne(d => d.Monster2).WithMany(p => p.DefenseMonster2s)
                .HasForeignKey(d => d.Monster2Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("defense_monster_2_id_fkey");

            entity.HasOne(d => d.Monster3).WithMany(p => p.DefenseMonster3s)
                .HasForeignKey(d => d.Monster3Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("defense_monster_3_id_fkey");

            entity.HasOne(d => d.MonsterLead).WithMany(p => p.DefenseMonsterLeads)
                .HasForeignKey(d => d.MonsterLeadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("defense_monster_1_id_fkey");
        });

        modelBuilder.Entity<Guild>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("guild_pkey");

            entity.ToTable("guild");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<GuildPlayer>(entity =>
        {
            entity.HasKey(e => new { e.GuildId, e.PlayerId }).HasName("guild_player_pkey");

            entity.ToTable("guild_player");

            entity.Property(e => e.GuildId).HasColumnName("guild_id");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("joined_at");
            entity.Property(e => e.LeftAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("left_at");

            entity.HasOne(d => d.Guild).WithMany(p => p.GuildPlayers)
                .HasForeignKey(d => d.GuildId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("guild_player_guild_id_fkey");

            entity.HasOne(d => d.Player).WithMany(p => p.GuildPlayers)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("guild_player_player_id_fkey");
        });

        modelBuilder.Entity<LeaderSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("leader_skill_pkey");

            entity.ToTable("leader_skill");

            entity.HasIndex(e => e.Id, "leader_skill_newId_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.LeaderTypeId).HasColumnName("leader_type_id");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.LeaderType).WithMany(p => p.LeaderSkills)
                .HasForeignKey(d => d.LeaderTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("leader_skill_leader_type_id_fkey");

            entity.HasOne(d => d.Type).WithMany(p => p.LeaderSkills)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("leader_skill_type_fkey");
        });

        modelBuilder.Entity<LeaderType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("leader_type_pkey");

            entity.ToTable("leader_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Monster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("monster_pkey");

            entity.ToTable("monster");

            entity.HasIndex(e => e.Id, "monster_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AttributeId).HasColumnName("attribute_id");
            entity.Property(e => e.BaseName).HasColumnName("base_name");
            entity.Property(e => e.IsNat5)
                .HasDefaultValue(false)
                .HasColumnName("is_nat_5");
            entity.Property(e => e.LeaderId).HasColumnName("leader_id");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.Attribute).WithMany(p => p.Monsters)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("monster_attribute_id_fkey");

            entity.HasOne(d => d.Leader).WithMany(p => p.Monsters)
                .HasForeignKey(d => d.LeaderId)
                .HasConstraintName("monster_leader_id_fkey");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("player_pkey");

            entity.ToTable("player");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<PlayerDefense>(entity =>
        {
            entity.HasKey(e => new { e.PlayerId, e.DefenseId, e.Name }).HasName("player_defense_pkey");

            entity.ToTable("player_defense");

            entity.Property(e => e.PlayerId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("player_id");
            entity.Property(e => e.DefenseId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("defense_id");
            entity.Property(e => e.Name)
                .HasDefaultValueSql("''::text")
                .HasColumnName("name");
            entity.Property(e => e.Losses)
                .HasDefaultValueSql("'0'::smallint")
                .HasColumnName("losses");
            entity.Property(e => e.Wins)
                .HasDefaultValueSql("'0'::smallint")
                .HasColumnName("wins");

            entity.HasOne(d => d.Defense).WithMany(p => p.PlayerDefenses)
                .HasForeignKey(d => d.DefenseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("player_defense_defense_id_fkey");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerDefenses)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("player_defense_player_id_fkey");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("type_pkey");

            entity.ToTable("type");

            entity.HasIndex(e => e.Id, "type_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
