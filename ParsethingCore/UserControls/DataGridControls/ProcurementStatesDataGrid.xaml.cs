namespace ParsethingCore.UserControls.DataGridControls;

public partial class ProcurementStatesDataGrid : UserControl, IView
{
    public ProcurementStatesDataGrid()
    {
        GetProcurementStates();
        InitializeComponent();
    }

    private List<ProcurementState>? ProcurementStates { get; set; }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) =>
        GetView();

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            ProcurementStateCard card = new((ProcurementState)View.SelectedItem);
            card.ShowDialog();
        }
        catch { }
        GetView();
    }

    private void View_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try { ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = ((ProcurementState)View.SelectedItem).Id; }
        catch { }
    }

    private void View_SizeChanged(object sender, SizeChangedEventArgs e) =>
        View.MinColumnWidth = View.ActualWidth / View.Columns.Count;

    private void GetProcurementStates()
    {
        ProcurementStates = GET.View.ProcurementStates();
        if (ProcurementStates != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = ProcurementStates.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }

    public void GetView()
    {
        GetProcurementStates();
        View.ItemsSource = ProcurementStates;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        ((TextBox)Application.Current.MainWindow.FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        ProcurementStateCard card = new();
        card.ShowDialog();
        GetView();
    }

    public void Edit()
    {
        if ((ProcurementState)View.SelectedItem != null)
        {
            ProcurementStateCard card = new((ProcurementState)View.SelectedItem);
            card.ShowDialog();
        }
        GetView();
    }

    public void Delete()
    {
        try
        {
            ProcurementState procurementState = (ProcurementState)View.SelectedItem;
            if (procurementState != null)
            {
                DeleteConfirmation confirmation = new();
                if (confirmation.ShowDialog() == true)
                    DELETE.ProcurementState(procurementState);
            }
        }
        catch { }
        GetView();
    }

    public void Export() =>
        ExportInstance.Run(View);

    public void Search(string searchString)
    {
        List<ProcurementState>? results = ProcurementStates?
            .Where(p => p.Kind.ToLower().Contains(searchString.ToLower()))
            .ToList();
        View.ItemsSource = results;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        if (results != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = results.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }
}