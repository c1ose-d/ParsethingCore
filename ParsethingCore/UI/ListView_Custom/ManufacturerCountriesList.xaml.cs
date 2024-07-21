namespace ParsethingCore.UI.ListView_Custom;

public partial class ManufacturerCountriesList : UserControl, IView
{
    public ManufacturerCountriesList()
    {
        InitializeComponent();
        GetView();
    }

    public void GetView()
    {
        View.ItemsSource = GET.View.ManufacturerCountries();
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        if (new ManufacturerCountryCard().ShowDialog() == true)
            GetView();
    }

    public void Delete()
    {
        if (View.SelectedIndex != -1 && new DeleteFlyout(((ComponentHeaderType)View.SelectedItem).Kind).ShowDialog() == true)
        {
            DELETE.ManufacturerCountry((ManufacturerCountry)View.SelectedItem);
            GetView();
        }
    }

    public void Edit()
    {
        if (View.SelectedIndex != -1)
        {
            new ManufacturerCountryCard((ManufacturerCountry)View.SelectedItem).ShowDialog();
            GetView();
        }
    }

    public void Export() { }

    public void Search(string searchString)
    {
        View.ItemsSource = GET.View.ComponentHeaderTypes()?
            .Where(e => e.Kind.ToLower().Contains(searchString))
            .ToList();
    }

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (View.SelectedIndex != -1)
        {
            new ManufacturerCountryCard((ManufacturerCountry)View.SelectedItem).ShowDialog();
            GetView();
        }
    }
}