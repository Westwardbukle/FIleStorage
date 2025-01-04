using FileStorage.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.Database.Context;

public sealed class CommonContext : DbContext
{
    public DbSet<FileEntity> Files { get; set; }

    public CommonContext(DbContextOptions<CommonContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<FileEntity>(new FileEntity().Configure);
    }
}