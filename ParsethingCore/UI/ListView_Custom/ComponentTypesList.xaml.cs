namespace ParsethingCore.UI.ListView_Custom;

public partial class ComponentTypesList : UserControl, IView
{
    public ComponentTypesList()
    {
        InitializeComponent();
        GetView();
    }

    public void GetView()
    {
        View.ItemsSource = GET.View.ComponentTypes();
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        if (new ComponentTypeCard().ShowDialog() == true)
            GetView();
    }

    public void Delete()
    {
        if (View.SelectedIndex != -1 && new DeleteFlyout(((ComponentType)View.SelectedItem).Kind).ShowDialog() == true)
        {
            DELETE.ComponentType((ComponentType)View.SelectedItem);
            GetView();
        }
    }

    public void Edit()
    {
        if (View.SelectedIndex != -1)
        {
            new ComponentTypeCard((ComponentType)View.SelectedItem).ShowDialog();
            GetView();
        }
    }

    public void Export() { }

    public void Search(string searchString)
    {
        View.ItemsSource = GET.View.ComponentTypes()?
            .Where(e => e.Kind.ToLower().Contains(searchString))
            .ToList();
    }

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (View.SelectedIndex != -1)
        {
            new ComponentTypeCard((ComponentType)View.SelectedItem).ShowDialog();
            GetView();
        }
    }
}