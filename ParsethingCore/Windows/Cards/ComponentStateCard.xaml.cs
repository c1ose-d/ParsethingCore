namespace ParsethingCore.Windows.Cards;

public partial class ComponentStateCard : Window
{
    public ComponentStateCard(ComponentState? componentState = null)
    {
        ComponentState = componentState;

        InitializeComponent();

        if (ComponentState != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = ComponentState.Kind;
            ComponentState_Kind.Text = ComponentState.Kind;

            System_Id.Text = ComponentState.Id.ToString();
            System_Kind.Text = ComponentState.Kind;
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private ComponentState? ComponentState { get; set; }

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

    private void ComponentState_Kind_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (ComponentState_Kind.Text == string.Empty)
            ComponentState_Kind_Clear.Visibility = Visibility.Collapsed;
        else ComponentState_Kind_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = ComponentState_Kind.Text;
    }

    private void ComponentState_Kind_Clear_Click(object sender, RoutedEventArgs e) =>
        ComponentState_Kind.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (ComponentState == null)
            {
                ComponentState = new() { Kind = ComponentState_Kind.Text };
                if (PUT.ComponentState(ComponentState))
                    DialogResult = true;
            }
            else
            {
                ComponentState.Kind = ComponentState_Kind.Text;
                if (PULL.ComponentState(ComponentState))
                    DialogResult = true;
            }
        }
        catch { }
    }
}