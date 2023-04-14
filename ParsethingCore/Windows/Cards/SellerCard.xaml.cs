namespace ParsethingCore.Windows.Cards;

public partial class SellerCard : Window
{
    public SellerCard(Seller? seller = null)
    {
        Seller = seller;

        InitializeComponent();

        if (Seller != null)
            SellerName.Text = Seller.Name;
    }

    private Seller? Seller { get; set; }

    private void Approve_Click(object sender, RoutedEventArgs e)
    {
        if (Seller == null)
        {
            Seller = new() { Name = SellerName.Text };
            if (PUT.Seller(Seller))
                DialogResult = true;
        }
        else
        {
            Seller.Name = SellerName.Text;
            if (PULL.Seller(Seller))
                DialogResult = true;
        }
    }
}