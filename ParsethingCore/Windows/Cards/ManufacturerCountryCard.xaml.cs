namespace ParsethingCore.Windows.Cards;

public partial class ManufacturerCountryCard : Window
{
    public ManufacturerCountryCard(ManufacturerCountry? manufacturerCountry = null)
    {
        ManufacturerCountry = manufacturerCountry;

        InitializeComponent();

        if (ManufacturerCountry != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = ManufacturerCountry.Name;
            ManufacturerCountry_Name.Text = ManufacturerCountry.Name;

            System_Id.Text = ManufacturerCountry.Id.ToString();
            System_Name.Text = ManufacturerCountry.Name;
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private ManufacturerCountry? ManufacturerCountry { get; set; }

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

    private void ManufacturerCountry_Name_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (ManufacturerCountry_Name.Text == string.Empty)
            ManufacturerCountry_Name_Clear.Visibility = Visibility.Collapsed;
        else ManufacturerCountry_Name_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = ManufacturerCountry_Name.Text;
    }

    private void ManufacturerCountry_Name_Clear_Click(object sender, RoutedEventArgs e) =>
        ManufacturerCountry_Name.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (ManufacturerCountry == null)
            {
                ManufacturerCountry = new() { Name = ManufacturerCountry_Name.Text };
                if (PUT.ManufacturerCountry(ManufacturerCountry))
                    DialogResult = true;
            }
            else
            {
                ManufacturerCountry.Name = ManufacturerCountry_Name.Text;
                if (PULL.ManufacturerCountry(ManufacturerCountry))
                    DialogResult = true;
            }
        }
        catch { }
    }
}