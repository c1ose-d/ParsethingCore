namespace ParsethingCore.UI.ListView_Custom;

public partial class SellersList : UserControl, IView
{
    public SellersList()
    {
        InitializeComponent();
        GetView();
    }

    public void GetView()
    {
        View.ItemsSource = GET.View.Sellers();
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        if (new SellerCard().ShowDialog() == true)
            GetView();
    }

    public void Delete()
    {
        if (View.SelectedIndex != -1 && new DeleteFlyout(((Seller)View.SelectedItem).Name).ShowDialog() == true)
        {
            DELETE.Seller((Seller)View.SelectedItem);
            GetView();
        }
    }

    public void Edit()
    {
        if (View.SelectedIndex != -1)
        {
            new SellerCard((Seller)View.SelectedItem).ShowDialog();
            GetView();
        }
    }

    public void Export() { }

    public void Search(string searchString)
    {
        View.ItemsSource = GET.View.Sellers()?
            .Where(e => e.Name.ToLower().Contains(searchString))
            .ToList();
    }

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (View.SelectedIndex != -1)
        {
            new SellerCard((Seller)View.SelectedItem).ShowDialog();
            GetView();
        }
    }
}