namespace ParsethingCore.Windows.Cards;

public partial class PositionCard : Window
{
    public PositionCard(Position? position = null)
    {
        Position = position;

        InitializeComponent();

        if (Position != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = Position.Kind;
            Position_Kind.Text = Position.Kind;

            System_Id.Text = Position.Id.ToString();
            System_Kind.Text = Position.Kind;
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private Position? Position { get; set; }

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

    private void Position_Kind_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Position_Kind.Text == string.Empty)
            Position_Kind_Clear.Visibility = Visibility.Collapsed;
        else Position_Kind_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = Position_Kind.Text;
    }

    private void Position_Kind_Clear_Click(object sender, RoutedEventArgs e) =>
        Position_Kind.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Position == null)
            {
                Position = new() { Kind = Position_Kind.Text };
                if (PUT.Position(Position))
                    DialogResult = true;
            }
            else
            {
                Position.Kind = Position_Kind.Text;
                if (PULL.Position(Position))
                    DialogResult = true;
            }
        }
        catch { }
    }
}