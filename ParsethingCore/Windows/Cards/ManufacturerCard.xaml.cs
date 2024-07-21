namespace ParsethingCore.Windows.Cards;

public partial class ManufacturerCard : Window
{
    public ManufacturerCard(Manufacturer? manufacturer = null)
    {
        Manufacturer = manufacturer;

        InitializeComponent();
        Manufacturer_Country.ItemsSource = GET.View.ManufacturerCountries();

        if (Manufacturer != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = Manufacturer.ManufacturerName;
            Manufacturer_Name.Text = Manufacturer.ManufacturerName;
            Manufacturer_FullName.Text = Manufacturer.FullManufacturerName;
            if (Manufacturer_Country.ItemsSource != null)
            {
                foreach (ManufacturerCountry manufacturerCountry in Manufacturer_Country.ItemsSource)
                {
                    if (manufacturerCountry.Id == Manufacturer.ManufacturerCountryId)
                    {
                        Manufacturer_Country.SelectedItem = manufacturerCountry;
                        break;
                    }
                }
            }

            System_Id.Text = Manufacturer.Id.ToString();
            System_Name.Text = Manufacturer.ManufacturerName;
            System_FullName.Text = Manufacturer.FullManufacturerName;
            System_Country.Text = Manufacturer.ManufacturerCountryId.ToString();
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private Manufacturer? Manufacturer { get; set; }

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

    private void Manufacturer_Name_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Manufacturer_Name.Text == string.Empty)
            Manufacturer_Name_Clear.Visibility = Visibility.Collapsed;
        else Manufacturer_Name_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = Manufacturer_Name.Text;
    }

    private void Manufacturer_Name_Clear_Click(object sender, RoutedEventArgs e) =>
        Manufacturer_Name.Text = string.Empty;

    private void Manufacturer_FullName_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Manufacturer_FullName.Text == string.Empty)
            Manufacturer_FullName_Clear.Visibility = Visibility.Collapsed;
        else Manufacturer_FullName_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = Manufacturer_Name.Text;
    }

    private void Manufacturer_FullName_Clear_Click(object sender, RoutedEventArgs e) =>
        Manufacturer_FullName.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Manufacturer == null)
            {
                Manufacturer = new()
                {
                    ManufacturerName = Manufacturer_Name.Text,
                    FullManufacturerName = Manufacturer_FullName.Text,
                    ManufacturerCountryId = ((ManufacturerCountry)Manufacturer_Country.SelectedItem).Id
                };
                if (PUT.Manufacturer(Manufacturer))
                    DialogResult = true;
            }
            else
            {
                Manufacturer.ManufacturerName = Manufacturer_Name.Text;
                Manufacturer.FullManufacturerName = Manufacturer_FullName.Text;
                Manufacturer.ManufacturerCountryId = ((ManufacturerCountry)Manufacturer_Country.SelectedItem).Id;
                if (PULL.Manufacturer(Manufacturer))
                    DialogResult = true;
            }
        }
        catch { }
    }
}