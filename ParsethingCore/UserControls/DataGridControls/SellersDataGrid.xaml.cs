namespace ParsethingCore.UserControls.DataGridControls;

public partial class SellersDataGrid : UserControl, IView
{
    public SellersDataGrid()
    {
        GetSellers();
        InitializeComponent();
    }

    private List<Seller>? Sellers { get; set; }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) =>
        GetView();

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            SellerCard card = new((Seller)View.SelectedItem);
            card.ShowDialog();
        }
        catch { }
        GetView();
    }

    private void View_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try { ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = ((Seller)View.SelectedItem).Id; }
        catch { }
    }

    private void View_SizeChanged(object sender, SizeChangedEventArgs e) =>
        View.MinColumnWidth = View.ActualWidth / View.Columns.Count;

    private void GetSellers()
    {
        Sellers = GET.View.Sellers();
        if (Sellers != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = Sellers.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }

    public void GetView()
    {
        GetSellers();
        View.ItemsSource = Sellers;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        ((TextBox)Application.Current.MainWindow.FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        SellerCard card = new();
        card.ShowDialog();
        GetView();
    }

    public void Edit()
    {
        if ((Seller)View.SelectedItem != null)
        {
            SellerCard card = new((Seller)View.SelectedItem);
            card.ShowDialog();
        }
        GetView();
    }

    public void Delete()
    {
        try
        {
            Seller seller = (Seller)View.SelectedItem;
            if (seller != null)
            {
                DeleteConfirmation confirmation = new();
                if (confirmation.ShowDialog() == true)
                    DELETE.Seller(seller);
            }
        }
        catch { }
        GetView();
    }

    public void Export() =>
        ExportInstance.Run(View);

    public void Search(string searchString)
    {
        List<Seller>? results = Sellers?
            .Where(m => m.Name.ToLower().Contains(searchString.ToLower()))
            .ToList();
        View.ItemsSource = results;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        if (results != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = results.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }
}