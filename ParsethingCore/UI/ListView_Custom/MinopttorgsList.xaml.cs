namespace ParsethingCore.UI.ListView_Custom;

public partial class MinopttorgsList : UserControl, IView
{
    public MinopttorgsList()
    {
        InitializeComponent();
        GetView();
    }

    public void GetView()
    {
        View.ItemsSource = GET.View.Minopttorgs();
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        if (new MinopttorgCard().ShowDialog() == true)
            GetView();
    }

    public void Delete()
    {
        if (View.SelectedIndex != -1 && new DeleteFlyout(((Minopttorg)View.SelectedItem).Name).ShowDialog() == true)
        {
            DELETE.Minopttorg((Minopttorg)View.SelectedItem);
            GetView();
        }
    }

    public void Edit()
    {
        if (View.SelectedIndex != -1)
        {
            new MinopttorgCard((Minopttorg)View.SelectedItem).ShowDialog();
            GetView();
        }
    }

    public void Export() { }

    public void Search(string searchString)
    {
        View.ItemsSource = GET.View.Minopttorgs()?
            .Where(e => e.Name.ToLower().Contains(searchString))
            .ToList();
    }

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (View.SelectedIndex != -1)
        {
            new MinopttorgCard((Minopttorg)View.SelectedItem).ShowDialog();
            GetView();
        }
    }
}