namespace ParsethingCore.Windows.Cards;

public partial class PositionCard : Window
{
    public PositionCard(Position? position = null)
    {
        Position = position;

        InitializeComponent();

        if (Position != null)
            Kind.Text = Position.Kind;
    }

    private Position? Position { get; set; }

    private void Approve_Click(object sender, RoutedEventArgs e)
    {
        if (Position == null)
        {
            Position = new() { Kind = Kind.Text };
            if (PUT.Position(Position))
                DialogResult = true;
        }
        else
        {
            Position.Kind = Kind.Text;
            if (PULL.Position(Position))
                DialogResult = true;
        }
    }
}