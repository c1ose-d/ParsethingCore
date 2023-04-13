namespace ParsethingCore.Interfaces;

public interface IView
{
    public int Count { get; }

    public void Add();
    public void Edit();
    public void Delete();
}