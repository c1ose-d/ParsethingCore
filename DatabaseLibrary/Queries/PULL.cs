namespace DatabaseLibrary.Queries;

public sealed class PULL
{
    public static bool Procurement(Procurement procurement, Procurement def, bool isGetted)
    {
        try
        {
            ParsethingContext db = new();
            def.LawId = procurement.LawId;
            def.Object = procurement.Object;
            def.InitialPrice = procurement.InitialPrice;
            def.OrganizationId = procurement.OrganizationId;
            if (isGetted)
            {
                def.MethodId = procurement.MethodId;
                def.PlatformId = procurement.PlatformId;
                def.Location = procurement.Location;
                def.StartDate = procurement.StartDate;
                def.Deadline = procurement.Deadline;
                def.TimeZoneId = procurement.TimeZoneId;
                def.Securing = procurement.Securing;
                def.Enforcement = procurement.Enforcement;
                def.Warranty = procurement.Warranty;
            }
            _ = db.Procurements.Update(def);
            _ = db.SaveChanges();
        }
        catch
        {
            return false;
        }
        return true;
    }
}