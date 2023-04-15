namespace ParsethingCore.Windows.Cards;

public partial class MinopttorgCard : Window
{
    public MinopttorgCard(Minopttorg? minopttorg = null)
    {
        Minopttorg = minopttorg;

        InitializeComponent();

        if (Minopttorg != null)
            MinopttorgName.Text = Minopttorg.Name;
    }

    private Minopttorg? Minopttorg { get; set; }

    private void Approve_Click(object sender, RoutedEventArgs e)
    {
        if (Minopttorg == null)
        {
            Minopttorg = new() { Name = MinopttorgName.Text };
            if (PUT.Minopttorg(Minopttorg))
                DialogResult = true;
        }
        else
        {
            Minopttorg.Name = MinopttorgName.Text;
            if (PULL.Minopttorg(Minopttorg))
                DialogResult = true;
        }
    }
}