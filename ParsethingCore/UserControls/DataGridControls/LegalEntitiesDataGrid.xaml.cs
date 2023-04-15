namespace ParsethingCore.UserControls.DataGridControls;

public partial class LegalEntitiesDataGrid : UserControl, IView
{
    public LegalEntitiesDataGrid()
    {
        GetLegalEntities();
        InitializeComponent();
    }

    private List<LegalEntity>? LegalEntities { get; set; }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) =>
        GetView();

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            LegalEntityCard card = new((LegalEntity)View.SelectedItem);
            card.ShowDialog();
        }
        catch { }
        GetView();
    }

    private void View_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try { ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = ((LegalEntity)View.SelectedItem).Id; }
        catch { }
    }

    private void View_SizeChanged(object sender, SizeChangedEventArgs e) =>
        View.MinColumnWidth = View.ActualWidth / View.Columns.Count;

    private void GetLegalEntities()
    {
        LegalEntities = GET.View.LegalEntities();
        if (LegalEntities != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = LegalEntities.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }

    public void GetView()
    {
        GetLegalEntities();
        View.ItemsSource = LegalEntities;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        ((TextBox)Application.Current.MainWindow.FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        LegalEntityCard card = new();
        card.ShowDialog();
        GetView();
    }

    public void Edit()
    {
        if ((LegalEntity)View.SelectedItem != null)
        {
            LegalEntityCard card = new((LegalEntity)View.SelectedItem);
            card.ShowDialog();
        }
        GetView();
    }

    public void Delete()
    {
        try
        {
            LegalEntity legalEntity = (LegalEntity)View.SelectedItem;
            if (legalEntity != null)
            {
                DeleteConfirmation confirmation = new();
                if (confirmation.ShowDialog() == true)
                    DELETE.LegalEntity(legalEntity);
            }
        }
        catch { }
        GetView();
    }

    public void Export() =>
        ExportInstance.Run(View);

    public void Search(string searchString)
    {
        List<LegalEntity>? results = LegalEntities?
            .Where(m => m.Name.ToLower().Contains(searchString.ToLower()))
            .ToList();
        View.ItemsSource = results;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        if (results != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = results.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }
}