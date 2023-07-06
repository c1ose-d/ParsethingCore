namespace ParsethingCore.UI;

public partial class CommandBar : UserControl
{
    public CommandBar() =>
        InitializeComponent();

    private Sources Sources { get; set; } = null!;
    private Thread SourcesCaller { get; set; } = null!;
    private Border DataGridContainer { get; set; } = null!;

    private void Bar_Loaded(object sender, RoutedEventArgs e) =>
        DataGridContainer = (Border)Application.Current.MainWindow.FindName("DataGridContainer");

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        try { ((IView)DataGridContainer.Child).Add(); }
        catch { }
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        try { ((IView)DataGridContainer.Child).Edit(); }
        catch { }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        try { ((IView)DataGridContainer.Child).Delete(); }
        catch { }
    }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
        try { ((IView)DataGridContainer.Child).GetView(); }
        catch { }
    }

    private void Export_Click(object sender, RoutedEventArgs e)
    {
        try { ((IView)DataGridContainer.Child).Export(); }
        catch { }
    }

    private void Run_Click(object sender, RoutedEventArgs e)
    {
        ParsingFlyout parsingFlyout = new();
        if (parsingFlyout.ShowDialog() == true)
        {
            Run.IsEnabled = false;
            Stop.IsEnabled = true;
            Sources = new();
            string minPrice = parsingFlyout.MinPrice.Text;
            string maxPrice = parsingFlyout.MaxPrice.Text;
            List<Region> regions = parsingFlyout
            try
            {
                SourcesCaller = new(() => Sources.Enable(minPrice, maxPrice));
                SourcesCaller.Start();
            }
            catch (Exception ex) { LogWriter.Write(ex); }
        }
    }

    private void Stop_Click(object sender, RoutedEventArgs e)
    {
        Run.IsEnabled = true;
        Stop.IsEnabled = false;
        try { SourcesCaller = new(() => Sources.Disable()); }
        catch { }
    }
}