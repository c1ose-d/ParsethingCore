namespace ParsethingCore.UI.ListView_Custom;

public partial class ManufacturersList : UserControl, IView
{
    public ManufacturersList()
    {
        InitializeComponent();
        GetView();
    }

    public void GetView()
    {
        View.ItemsSource = GET.View.Manufacturers();
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        if (new ManufacturerCard().ShowDialog() == true)
            GetView();
    }

    public void Delete()
    {
        if (View.SelectedIndex != -1 && new DeleteFlyout(((Manufacturer)View.SelectedItem).Name).ShowDialog() == true)
        {
            DELETE.Manufacturer((Manufacturer)View.SelectedItem);
            GetView();
        }
    }

    public void Edit()
    {
        if (View.SelectedIndex != -1)
        {
            new ManufacturerCard((Manufacturer)View.SelectedItem).ShowDialog();
            GetView();
        }
    }

    public void Export() { }

    public void Search(string searchString)
    {
        View.ItemsSource = GET.View.Manufacturers()?
            .Where(e => e.Name.ToLower().Contains(searchString))
            .ToList();
    }

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (View.SelectedIndex != -1)
        {
            new ManufacturerCard((Manufacturer)View.SelectedItem).ShowDialog();
            GetView();
        }
    }
}