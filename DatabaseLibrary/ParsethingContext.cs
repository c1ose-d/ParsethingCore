namespace DatabaseLibrary;

public partial class ParsethingContext : DbContext
{
    public ParsethingContext() { }

    public virtual DbSet<Law> Laws { get; set; } = null!;
    public virtual DbSet<Method> Methods { get; set; } = null!;
    public virtual DbSet<Organization> Organizations { get; set; } = null!;
    public virtual DbSet<Platform> Platforms { get; set; } = null!;
    public virtual DbSet<Procurement> Procurements { get; set; } = null!;
    public virtual DbSet<Tag> Tags { get; set; } = null!;
    public virtual DbSet<TimeZone> TimeZones { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _ = optionsBuilder.UseSqlServer(Resources.ConnectionString);
    }
}