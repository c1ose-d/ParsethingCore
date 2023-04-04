namespace DatabaseLibrary.Queries;

public sealed class PUT
{
    public static bool Procurement(Procurement procurement, bool isGetted)
    {
        try
        {
            ParsethingContext db = new();
            Procurement? def = null;
            try
            {
                def = db.Procurements.Where(p => p.Number == procurement.Number).First();
            }
            catch { }
            if (def == null)
            {
                _ = db.Procurements.Add(procurement);
                _ = db.SaveChanges();
            }
            else
            {
                if (!PULL.Procurement(procurement, def, isGetted))
                {
                    throw new Exception();
                }
            }
        }
        catch
        {
            return false;
        }
        return true;
    }
}