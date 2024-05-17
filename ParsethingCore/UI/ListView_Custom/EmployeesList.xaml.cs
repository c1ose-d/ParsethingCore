namespace ParsethingCore.UI.ListView_Custom;

public partial class EmployeesList : UserControl, IView
{
    public EmployeesList()
    {
        InitializeComponent();
        GetView();
    }

    public void GetView()
    {
        View.ItemsSource = GET.View.Employees();
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        if (new EmployeeCard().ShowDialog() == true)
            GetView();
    }

    public void Delete()
    {
        if (View.SelectedIndex != -1 && new DeleteFlyout(((Employee)View.SelectedItem).FullName).ShowDialog() == true)
        {
            DELETE.Employee((Employee)View.SelectedItem);
            GetView();
        }
    }

    public void Edit()
    {
        if (View.SelectedIndex != -1)
        {
            new EmployeeCard((Employee)View.SelectedItem).ShowDialog();
            GetView();
        }
    }

    public void Export() { }

    public void Search(string searchString)
    {
        if (CheckVisibility.IsChecked == true)
        {
            View.ItemsSource = GET.View.Employees()?
                .Where(e => (e.FullName.ToLower().Contains(searchString) ||
                e.Position != null && e.Position.Kind.ToLower().Contains(searchString) ||
                e.UserName.ToLower().Contains(searchString)) &&
                e.IsAvailable == true)
                .ToList();
        }
        else
        {
            View.ItemsSource = GET.View.Employees()?
                .Where(e => e.FullName.ToLower().Contains(searchString) ||
                e.Position != null && e.Position.Kind.ToLower().Contains(searchString) ||
                e.UserName.ToLower().Contains(searchString))
                .ToList();
        }
    }

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (View.SelectedIndex != -1)
        {
            new EmployeeCard((Employee)View.SelectedItem).ShowDialog();
            GetView();
        }
    }

    private void CheckVisibility_Checked(object sender, RoutedEventArgs e)
    {
        if (CheckVisibility.IsChecked == true)
        {
            View.ItemsSource = GET.View.Employees()?
                .Where(e => e.IsAvailable == true)
                .ToList();
        }
        else
        {
            View.ItemsSource = GET.View.Employees()?
                .ToList();
        }
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }

    private void CheckVisibility_Unchecked(object sender, RoutedEventArgs e)
    {
        if (CheckVisibility.IsChecked == true)
        {
            View.ItemsSource = GET.View.Employees()?
                .Where(e => e.IsAvailable == true)
                .ToList();
        }
        else
        {
            View.ItemsSource = GET.View.Employees()?
                .ToList();
        }
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }
}