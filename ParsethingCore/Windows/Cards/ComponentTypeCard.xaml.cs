namespace ParsethingCore.Windows.Cards;

public partial class ComponentTypeCard : Window
{
    public ComponentTypeCard(ComponentType? componentType = null)
    {
        ComponentType = componentType;

        InitializeComponent();

        if (ComponentType != null)
            Kind.Text = ComponentType.Kind;
    }

    private ComponentType? ComponentType { get; set; }

    private void Approve_Click(object sender, RoutedEventArgs e)
    {
        if (ComponentType == null)
        {
            ComponentType = new() { Kind = Kind.Text };
            if (PUT.ComponentType(ComponentType))
                DialogResult = true;
        }
        else
        {
            ComponentType.Kind = Kind.Text;
            if (PULL.ComponentType(ComponentType))
                DialogResult = true;
        }
    }
}