namespace ParsethingCore.Windows.Cards;

public partial class PreferenceCard : Window
{
    public PreferenceCard(Preference? preference = null)
    {
        Preference = preference;

        InitializeComponent();

        if (Preference != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = Preference.Kind;
            Preference_Kind.Text = Preference.Kind;

            System_Id.Text = Preference.Id.ToString();
            System_Kind.Text = Preference.Kind;
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private Preference? Preference { get; set; }

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

    private void Preference_Kind_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Preference_Kind.Text == string.Empty)
            Preference_Kind_Clear.Visibility = Visibility.Collapsed;
        else Preference_Kind_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = Preference_Kind.Text;
    }

    private void Preference_Kind_Clear_Click(object sender, RoutedEventArgs e) =>
        Preference_Kind.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Preference == null)
            {
                Preference = new() { Kind = Preference_Kind.Text };
                if (PUT.Preference(Preference))
                    DialogResult = true;
            }
            else
            {
                Preference.Kind = Preference_Kind.Text;
                if (PULL.Preference(Preference))
                    DialogResult = true;
            }
        }
        catch { }
    }
}