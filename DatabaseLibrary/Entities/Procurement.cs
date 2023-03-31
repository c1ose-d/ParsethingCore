namespace DatabaseLibrary.Entities;

public partial class Procurement
{
    public int Id { get; set; }
    public string RequestUri { get; set; } = null!;
    public string Number { get; set; } = null!;
    public int LawId { get; set; }
    public string Object { get; set; } = null!;
    public decimal InitialPrice { get; set; }
    public int OrganizationId { get; set; }
    public int? MethodId { get; set; }
    public int? PlatformId { get; set; }
    public string? Location { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? Deadline { get; set; }
    public int? TimeZoneId { get; set; }
    public string? Securing { get; set; }
    public string? Enforcement { get; set; }
    public string? Warranty { get; set; }

    public virtual Law? Law { get; set; }
    public virtual Method? Method { get; set; }
    public virtual Organization? Organization { get; set; }
    public virtual Platform? Platform { get; set; }
    public virtual TimeZone? TimeZone { get; set; }

    public void SetNotNullableForeignKeys()
    {
        if (Law != null)
        {
            LawId = GetIdTo.Law(Law);
        }
        Law = null;

        if (Organization != null)
        {
            OrganizationId = GetIdTo.Organization(Organization);
        }
        Organization = null;
    }

    public void SetNullableForeignKeys()
    {
        if (Method != null)
        {
            MethodId = GetIdTo.Method(Method);
        }
        Method = null;

        if (Platform != null)
        {
            PlatformId = GetIdTo.Platform(Platform);
        }
        Platform = null;

        if (TimeZone != null)
        {
            TimeZoneId = GetIdTo.TimeZone(TimeZone);
        }
        TimeZone = null;
    }
}