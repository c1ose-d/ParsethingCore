namespace ParsethingCore.Windows.Cards;

public partial class PreferenceCard : Window
{
    public PreferenceCard(Preference? preference = null)
    {
        Preference = preference;

        InitializeComponent();

        if (Preference != null)
            Kind.Text = Preference.Kind;
    }

    private Preference? Preference { get; set; }

    private void Approve_Click(object sender, RoutedEventArgs e)
    {
        if (Preference == null)
        {
            Preference = new() { Kind = Kind.Text };
            if (PUT.Preference(Preference))
                DialogResult = true;
        }
        else
        {
            Preference.Kind = Kind.Text;
            if (PULL.Preference(Preference))
                DialogResult = true;
        }
    }
}