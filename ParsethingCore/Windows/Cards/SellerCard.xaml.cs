namespace ParsethingCore.Windows.Cards;

public partial class SellerCard : Window
{
    public SellerCard(Seller? seller = null)
    {
        Seller = seller;

        InitializeComponent();

        if (Seller != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = Seller.Name;
            Seller_Name.Text = Seller.Name;

            System_Id.Text = Seller.Id.ToString();
            System_Name.Text = Seller.Name;
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private Seller? Seller { get; set; }

    private void SideNav_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            foreach (Grid grid in CardView.Children)
                grid.Visibility = Visibility.Collapsed;
            CardView.Children[SideNav.SelectedIndex].Visibility = Visibility.Visible;
        }
        catch { }
    }

    private void Seller_Name_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Seller_Name.Text == string.Empty)
            Seller_Name_Clear.Visibility = Visibility.Collapsed;
        else Seller_Name_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = Seller_Name.Text;
    }

    private void Seller_Name_Clear_Click(object sender, RoutedEventArgs e) =>
        Seller_Name.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Seller == null)
            {
                Seller = new() { Name = Seller_Name.Text };
                if (PUT.Seller(Seller))
                    DialogResult = true;
            }
            else
            {
                Seller.Name = Seller_Name.Text;
                if (PULL.Seller(Seller))
                    DialogResult = true;
            }
        }
        catch { }
    }
}