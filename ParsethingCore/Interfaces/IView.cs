namespace ParsethingCore.Interfaces;

public interface IView
{
    public void GetView();
    public void Add();
    public void Edit();
    public void Delete();
    public void Export();
    public void Search(string searchString);
}