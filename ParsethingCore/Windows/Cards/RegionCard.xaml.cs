namespace ParsethingCore.Windows.Cards;

public partial class RegionCard : Window
{
    public RegionCard(Region? region = null)
    {
        Region = region;

        InitializeComponent();

        if (Region != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = Region.Title;
            Region_Title.Text = Region.Title;
            Region_Distance.Text = Region.Distance.ToString();

            System_Id.Text = Region.Id.ToString();
            System_Title.Text = Region.Title;
            System_Distance.Text = Region.Distance.ToString();
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private Region? Region { get; set; }

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

    private void Region_Title_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Region_Title.Text == string.Empty)
            Region_Title_Clear.Visibility = Visibility.Collapsed;
        else Region_Title_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = Region_Title.Text;
    }

    private void Region_Title_Clear_Click(object sender, RoutedEventArgs e) =>
        Region_Title.Text = string.Empty;

    private void Region_Distance_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Region_Distance.Text == string.Empty)
            Region_Distance_Clear.Visibility = Visibility.Collapsed;
        else Region_Distance_Clear.Visibility = Visibility.Visible;

        StringBuilder stringBuilder = new(Region_Distance.Text);
        for (int i = stringBuilder.Length - 1; i >= 0; i--)
            if (!char.IsDigit(stringBuilder[i]))
                stringBuilder.Remove(i, 1);
        Region_Distance.Text = stringBuilder.ToString();
        Region_Distance.CaretIndex = Region_Distance.Text.Length;
    }

    private void Region_Distance_Clear_Click(object sender, RoutedEventArgs e) =>
        Region_Distance.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Region == null)
            {
                Region = new()
                {
                    Title = Region_Title.Text,
                    Distance = Convert.ToInt32(Region_Distance.Text)
                };
                if (PUT.Region(Region))
                    DialogResult = true;
            }
            else
            {
                Region.Title = Region_Title.Text;
                Region.Distance = Convert.ToInt32(Region_Distance.Text);
                if (PULL.Region(Region))
                    DialogResult = true;
            }
        }
        catch { }
    }
}