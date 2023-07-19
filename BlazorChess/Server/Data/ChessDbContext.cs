using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using BlazorChess.Shared.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using MySql.EntityFrameworkCore.Extensions;

namespace BlazorChess.Server.Data;

public class ChessDbContext : DbContext
{
	public DbSet<OnlineGameInfo> Games { get; set; }


	public ChessDbContext(DbContextOptions<ChessDbContext> options) : base(options)
	{
		if (!Database.IsMySql()) return;

		//Database.EnsureDeleted();
		Database.EnsureCreated();
		Database.Migrate();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<OnlineGameInfo>()
			.Property(e => e.MovesHistory)
			.HasConversion(
				str => string.Join(',', str),
				str => str.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

		var valueComparer = new ValueComparer<List<string>>(
			(c1, c2) => c1.SequenceEqual(c2),
			c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
			c => c.ToList());

		modelBuilder
			.Entity<OnlineGameInfo>()
			.Property(e => e.MovesHistory)
			.Metadata
			.SetValueComparer(valueComparer);
	}
}



