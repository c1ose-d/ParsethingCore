namespace ParsethingCore.UI.ListView_Custom;

public partial class PredefinedComponentsList : UserControl, IView
{
    public PredefinedComponentsList()
    {
        InitializeComponent();
        GetView();
    }

    public void GetView()
    {
        View.ItemsSource = GET.View.PredefinedComponents();
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        if (new PredefinedComponentCard().ShowDialog() == true)
            GetView();
    }

    public void Delete()
    {
        if (View.SelectedIndex != -1 && new DeleteFlyout(((PredefinedComponent)View.SelectedItem).ComponentName!).ShowDialog() == true)
        {
            DELETE.PredefinedComponent((PredefinedComponent)View.SelectedItem);
            GetView();
        }
    }

    public void Edit()
    {
        if (View.SelectedIndex != -1)
        {
            new PredefinedComponentCard((PredefinedComponent)View.SelectedItem).ShowDialog();
            GetView();
        }
    }

    public void Search(string searchString)
    {
        View.ItemsSource = GET.View.PredefinedComponents()?
            .Where(e => e.ComponentName!.ToLower().Contains(searchString))
            .ToList();
    }

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (View.SelectedIndex != -1)
        {
            new PredefinedComponentCard((PredefinedComponent)View.SelectedItem).ShowDialog();
            GetView();
        }
    }
}