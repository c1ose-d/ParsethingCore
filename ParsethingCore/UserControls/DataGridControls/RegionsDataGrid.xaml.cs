namespace ParsethingCore.UserControls.DataGridControls;

public partial class RegionsDataGrid : UserControl, IView
{
    public RegionsDataGrid()
    {
        GetRegions();
        InitializeComponent();
    }

    private List<Region>? Regions { get; set; }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) =>
        GetView();

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            RegionCard card = new((Region)View.SelectedItem);
            card.ShowDialog();
        }
        catch { }
        GetView();
    }

    private void View_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try { ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = ((Region)View.SelectedItem).Id; }
        catch { }
    }

    private void View_SizeChanged(object sender, SizeChangedEventArgs e) =>
        View.MinColumnWidth = View.ActualWidth / View.Columns.Count;

    private void GetRegions()
    {
        Regions = GET.View.Regions();
        if (Regions != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = Regions.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }

    public void GetView()
    {
        GetRegions();
        View.ItemsSource = Regions;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        ((TextBox)Application.Current.MainWindow.FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        RegionCard card = new();
        card.ShowDialog();
        GetView();
    }

    public void Edit()
    {
        if ((Region)View.SelectedItem != null)
        {
            RegionCard card = new((Region)View.SelectedItem);
            card.ShowDialog();
        }
        GetView();
    }

    public void Delete()
    {
        try
        {
            Region region = (Region)View.SelectedItem;
            if (region != null)
            {
                DeleteConfirmation confirmation = new();
                if (confirmation.ShowDialog() == true)
                    DELETE.Region(region);
            }
        }
        catch { }
        GetView();
    }

    public void Export() =>
        ExportInstance.Run(View);

    public void Search(string searchString)
    {
        List<Region>? results = Regions?
            .Where(c => c.Title.ToLower().Contains(searchString.ToLower()))
            .ToList();
        View.ItemsSource = results;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        if (results != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = results.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }
}