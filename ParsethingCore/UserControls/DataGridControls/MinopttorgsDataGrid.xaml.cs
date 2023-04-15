namespace ParsethingCore.UserControls.DataGridControls;

public partial class MinopttorgsDataGrid : UserControl, IView
{
    public MinopttorgsDataGrid()
    {
        GetMinopttorgs();
        InitializeComponent();
    }

    private List<Minopttorg>? Minopttorgs { get; set; }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) =>
        GetView();

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            MinopttorgCard card = new((Minopttorg)View.SelectedItem);
            card.ShowDialog();
        }
        catch { }
        GetView();
    }

    private void View_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try { ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = ((Minopttorg)View.SelectedItem).Id; }
        catch { }
    }

    private void View_SizeChanged(object sender, SizeChangedEventArgs e) =>
        View.MinColumnWidth = View.ActualWidth / View.Columns.Count;

    private void GetMinopttorgs()
    {
        Minopttorgs = GET.View.Minopttorgs();
        if (Minopttorgs != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = Minopttorgs.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }

    public void GetView()
    {
        GetMinopttorgs();
        View.ItemsSource = Minopttorgs;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        ((TextBox)Application.Current.MainWindow.FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        MinopttorgCard card = new();
        card.ShowDialog();
        GetView();
    }

    public void Edit()
    {
        if ((Minopttorg)View.SelectedItem != null)
        {
            MinopttorgCard card = new((Minopttorg)View.SelectedItem);
            card.ShowDialog();
        }
        GetView();
    }

    public void Delete()
    {
        try
        {
            Minopttorg minopttorg = (Minopttorg)View.SelectedItem;
            if (minopttorg != null)
            {
                DeleteConfirmation confirmation = new();
                if (confirmation.ShowDialog() == true)
                    DELETE.Minopttorg(minopttorg);
            }
        }
        catch { }
        GetView();
    }

    public void Export() =>
        ExportInstance.Run(View);

    public void Search(string searchString)
    {
        List<Minopttorg>? results = Minopttorgs?
            .Where(m => m.Name.ToLower().Contains(searchString.ToLower()))
            .ToList();
        View.ItemsSource = results;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        if (results != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = results.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }
}