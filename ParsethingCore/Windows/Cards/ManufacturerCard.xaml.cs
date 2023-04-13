namespace ParsethingCore.Windows.Cards;

public partial class ManufacturerCard : Window
{
    public ManufacturerCard(Manufacturer? manufacturer = null)
    {
        Manufacturer = manufacturer;

        InitializeComponent();

        if (Manufacturer != null)
            Name.Text = Manufacturer.Name;
    }

    private Manufacturer? Manufacturer { get; set; }

    private void Approve_Click(object sender, RoutedEventArgs e)
    {
        if (Manufacturer == null)
        {
            Manufacturer = new() { Name = Name.Text };
            if (PUT.Manufacturer(Manufacturer))
                DialogResult = true;
        }
        else
        {
            Manufacturer.Name = Name.Text;
            if (PULL.Manufacturer(Manufacturer))
                DialogResult = true;
        }
    }
}