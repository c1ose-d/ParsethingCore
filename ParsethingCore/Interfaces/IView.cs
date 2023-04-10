namespace ParsethingCore.Interfaces;

public interface IView
{
    public IEnumerable<object>? Objects { get; set; }

    public void Add();
    public void Edit();
    public void Delete();
}