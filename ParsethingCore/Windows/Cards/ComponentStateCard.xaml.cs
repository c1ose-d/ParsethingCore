namespace ParsethingCore.Windows.Cards;

public partial class ComponentStateCard : Window
{
    public ComponentStateCard(ComponentState? componentState = null)
    {
        ComponentState = componentState;

        InitializeComponent();

        if (ComponentState != null)
            Kind.Text = ComponentState.Kind;
    }

    private ComponentState? ComponentState { get; set; }

    private void Approve_Click(object sender, RoutedEventArgs e)
    {
        if (ComponentState == null)
        {
            ComponentState = new() { Kind = Kind.Text };
            if (PUT.ComponentState(ComponentState))
                DialogResult = true;
        }
        else
        {
            ComponentState.Kind = Kind.Text;
            if (PULL.ComponentState(ComponentState))
                DialogResult = true;
        }
    }
}