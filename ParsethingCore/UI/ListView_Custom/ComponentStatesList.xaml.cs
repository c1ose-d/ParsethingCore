namespace ParsethingCore.UI.ListView_Custom;

public partial class ComponentStatesList : UserControl, IView
{
    public ComponentStatesList()
    {
        InitializeComponent();
        GetView();
    }

    public void GetView()
    {
        View.ItemsSource = GET.View.ComponentStates();
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        if (new ComponentStateCard().ShowDialog() == true)
            GetView();
    }

    public void Delete()
    {
        if (View.SelectedIndex != -1 && new DeleteFlyout(((ComponentState)View.SelectedItem).Kind).ShowDialog() == true)
        {
            DELETE.ComponentState((ComponentState)View.SelectedItem);
            GetView();
        }
    }

    public void Edit()
    {
        if (View.SelectedIndex != -1)
        {
            new ComponentStateCard((ComponentState)View.SelectedItem).ShowDialog();
            GetView();
        }
    }

    public void Export() { }

    public void Search(string searchString)
    {
        View.ItemsSource = GET.View.ComponentStates()?
            .Where(e => e.Kind.ToLower().Contains(searchString))
            .ToList();
    }

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (View.SelectedIndex != -1)
        {
            new ComponentStateCard((ComponentState)View.SelectedItem).ShowDialog();
            GetView();
        }
    }
}