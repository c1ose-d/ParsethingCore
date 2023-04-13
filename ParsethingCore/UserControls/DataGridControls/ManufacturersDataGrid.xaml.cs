namespace ParsethingCore.UserControls.DataGridControls;

public partial class ManufacturersDataGrid : UserControl, IView
{
    public ManufacturersDataGrid()
    {
        GetManufacturers();
        InitializeComponent();
    }

    private List<Manufacturer>? Manufacturers { get; set; }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) =>
        GetView();

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            ManufacturerCard card = new((Manufacturer)View.SelectedItem);
            card.ShowDialog();
        }
        catch { }
        GetView();
    }

    private void View_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try { ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = ((Manufacturer)View.SelectedItem).Id; }
        catch { }
    }

    private void View_SizeChanged(object sender, SizeChangedEventArgs e) =>
        View.MinColumnWidth = View.ActualWidth / View.Columns.Count;

    private void GetManufacturers()
    {
        Manufacturers = GET.View.Manufacturers();
        if (Manufacturers != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = Manufacturers.Count;
    }

    public void GetView()
    {
        GetManufacturers();
        View.ItemsSource = Manufacturers;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
    }

    public void Add()
    {
        ManufacturerCard card = new();
        card.ShowDialog();
        GetView();
    }

    public void Edit()
    {
        if ((Manufacturer)View.SelectedItem != null)
        {
            ManufacturerCard card = new((Manufacturer)View.SelectedItem);
            card.ShowDialog();
        }
        GetView();
    }

    public void Delete()
    {
        try
        {
            Manufacturer manufacturer = (Manufacturer)View.SelectedItem;
            if (manufacturer != null)
            {
                DeleteConfirmation confirmation = new();
                if (confirmation.ShowDialog() == true)
                    DELETE.Manufacturer(manufacturer);
            }
        }
        catch { }
        GetView();
    }

    public void Export() =>
        ExportInstance.Run(View);
}