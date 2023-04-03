namespace DatabaseLibrary;

public partial class ParsethingContext : DbContext
{
    public ParsethingContext() { }

    public virtual DbSet<Law> Laws { get; set; }
    public virtual DbSet<Method> Methods { get; set; }
    public virtual DbSet<Organization> Organizations { get; set; }
    public virtual DbSet<Platform> Platforms { get; set; }
    public virtual DbSet<Procurement> Procurements { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }
    public virtual DbSet<TimeZone> TimeZones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _ = optionsBuilder.UseSqlServer(Resources.ConnectionString);
    }
}