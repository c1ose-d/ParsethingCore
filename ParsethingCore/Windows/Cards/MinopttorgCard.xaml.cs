namespace ParsethingCore.Windows.Cards;

public partial class MinopttorgCard : Window
{
    public MinopttorgCard(Minopttorg? minopttorg = null)
    {
        Minopttorg = minopttorg;

        InitializeComponent();

        if (Minopttorg != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = Minopttorg.Name;
            Minopttorg_Name.Text = Minopttorg.Name;

            System_Id.Text = Minopttorg.Id.ToString();
            System_Name.Text = Minopttorg.Name;
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private Minopttorg? Minopttorg { get; set; }

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

    private void Minopttorg_Name_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Minopttorg_Name.Text == string.Empty)
            Minopttorg_Name_Clear.Visibility = Visibility.Collapsed;
        else Minopttorg_Name_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = Minopttorg_Name.Text;
    }

    private void Minopttorg_Name_Clear_Click(object sender, RoutedEventArgs e) =>
        Minopttorg_Name.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Minopttorg == null)
            {
                Minopttorg = new() { Name = Minopttorg_Name.Text };
                if (PUT.Minopttorg(Minopttorg))
                    DialogResult = true;
            }
            else
            {
                Minopttorg.Name = Minopttorg_Name.Text;
                if (PULL.Minopttorg(Minopttorg))
                    DialogResult = true;
            }
        }
        catch { }
    }
}