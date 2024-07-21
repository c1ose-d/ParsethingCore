namespace ParsethingCore.Windows.Cards;

public partial class CityCard : Window
{
    public CityCard(City? city = null)
    {
        City = city;

        InitializeComponent();

        if (City != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = City.Name;
            City_Name.Text = City.Name;

            System_Id.Text = City.Id.ToString();
            System_Name.Text = City.Name;
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private City? City { get; set; }

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

    private void City_Name_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (City_Name.Text == string.Empty)
            City_Name_Clear.Visibility = Visibility.Collapsed;
        else City_Name_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = City_Name.Text;
    }

    private void City_Name_Clear_Click(object sender, RoutedEventArgs e) =>
        City_Name.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (City == null)
            {
                City = new() { Name = City_Name.Text };
                if (PUT.City(City))
                    DialogResult = true;
            }
            else
            {
                City.Name = City_Name.Text;
                if (PULL.City(City))
                    DialogResult = true;
            }
        }
        catch { }
    }
}