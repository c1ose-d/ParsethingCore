namespace ParsethingCore.UI.ListView_Custom;

public partial class ComponentsList : UserControl, IView
{
    public ComponentsList()
    {
        InitializeComponent();
        GetView();
    }

    public void GetView()
    {
        View.ItemsSource = GET.View.Components();
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        if (new ComponentCard().ShowDialog() == true)
            GetView();
    }

    public void Delete()
    {
        if (View.SelectedIndex != -1 && new DeleteFlyout(((Component)View.SelectedItem).Title).ShowDialog() == true)
        {
            DELETE.Component((Component)View.SelectedItem);
            GetView();
        }
    }

    public void Edit()
    {
        if (View.SelectedIndex != -1)
        {
            new ComponentCard((Component)View.SelectedItem).ShowDialog();
            GetView();
        }
    }

    public void Export() { }

    public void Search(string searchString)
    {
        View.ItemsSource = GET.View.Components()?
            .Where(e => e.Title.ToLower().Contains(searchString) ||
            e.Manufacturer.Name.ToLower().Contains(searchString) ||
            e.ComponentType.Kind.ToLower().Contains(searchString))
            .ToList();
    }

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (View.SelectedIndex != -1)
        {
            new ComponentCard((Component)View.SelectedItem).ShowDialog();
            GetView();
        }
    }
}