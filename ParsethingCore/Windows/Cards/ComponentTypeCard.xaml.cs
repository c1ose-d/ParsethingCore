namespace ParsethingCore.Windows.Cards;

public partial class ComponentTypeCard : Window
{
    public ComponentTypeCard(ComponentType? componentType = null)
    {
        ComponentType = componentType;

        InitializeComponent();

        if (ComponentType != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = ComponentType.Kind;
            ComponentType_Kind.Text = ComponentType.Kind;

            System_Id.Text = ComponentType.Id.ToString();
            System_Kind.Text = ComponentType.Kind;
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private ComponentType? ComponentType { get; set; }

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

    private void ComponentType_Kind_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (ComponentType_Kind.Text == string.Empty)
            ComponentType_Kind_Clear.Visibility = Visibility.Collapsed;
        else ComponentType_Kind_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = ComponentType_Kind.Text;
    }

    private void ComponentType_Kind_Clear_Click(object sender, RoutedEventArgs e) =>
        ComponentType_Kind.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (ComponentType == null)
            {
                ComponentType = new() { Kind = ComponentType_Kind.Text };
                if (PUT.ComponentType(ComponentType))
                    DialogResult = true;
            }
            else
            {
                ComponentType.Kind = ComponentType_Kind.Text;
                if (PULL.ComponentType(ComponentType))
                    DialogResult = true;
            }
        }
        catch { }
    }
}