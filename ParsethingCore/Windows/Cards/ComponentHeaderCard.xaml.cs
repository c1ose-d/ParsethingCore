namespace ParsethingCore.Windows.Cards;

public partial class ComponentHeaderCard : Window
{
    public ComponentHeaderCard(ComponentHeaderType? componentHeader = null)
    {
        ComponentHeader = componentHeader;

        InitializeComponent();

        if (ComponentHeader != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = ComponentHeader.Kind;
            ComponentHeader_Kind.Text = ComponentHeader.Kind;

            System_Id.Text = ComponentHeader.Id.ToString();
            System_Kind.Text = ComponentHeader.Kind;
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private ComponentHeaderType? ComponentHeader { get; set; }

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

    private void ComponentHeader_Kind_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (ComponentHeader_Kind.Text == string.Empty)
            ComponentHeader_Kind_Clear.Visibility = Visibility.Collapsed;
        else ComponentHeader_Kind_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = ComponentHeader_Kind.Text;
    }

    private void ComponentHeader_Kind_Clear_Click(object sender, RoutedEventArgs e) =>
        ComponentHeader_Kind.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (ComponentHeader == null)
            {
                ComponentHeader = new() { Kind = ComponentHeader_Kind.Text };
                if (PUT.ComponentHeaderType(ComponentHeader))
                    DialogResult = true;
            }
            else
            {
                ComponentHeader.Kind = ComponentHeader_Kind.Text;
                if (PULL.ComponentHeaderType(ComponentHeader))
                    DialogResult = true;
            }
        }
        catch { }
    }
}