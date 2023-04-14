namespace ParsethingCore.Windows.Cards;

public partial class ComponentCard : Window
{
    public ComponentCard(Component? component = null)
    {
        Component = component;
        Manufacturers = GET.View.Manufacturers();
        ComponentTypes = GET.View.ComponentTypes();

        InitializeComponent();
        Manufacturer.ItemsSource = Manufacturers;
        ComponentType.ItemsSource = ComponentTypes;

        if (Component != null && Manufacturer.ItemsSource != null && ComponentType.ItemsSource != null)
        {
            ComponentTitle.Text = Component.Title;
            foreach (Manufacturer manufacturer in Manufacturer.ItemsSource)
                if (manufacturer.Id == Component.ManufacturerId)
                {
                    Manufacturer.SelectedItem = manufacturer;
                    break;
                }
            foreach (ComponentType сomponentType in ComponentType.ItemsSource)
                if (сomponentType.Id == Component.ComponentTypeId)
                {
                    ComponentType.SelectedItem = сomponentType;
                    break;
                }
        }
    }

    private Component? Component { get; set; }
    private List<Manufacturer>? Manufacturers { get; set; }
    private List<ComponentType>? ComponentTypes { get; set; }

    private void Approve_Click(object sender, RoutedEventArgs e)
    {
        if (Component == null)
        {
            Component = new()
            {
                Title = ComponentTitle.Text,
                ManufacturerId = ((Manufacturer)Manufacturer.SelectedItem).Id,
                ComponentTypeId = ((ComponentType)ComponentType.SelectedItem).Id
            };
            if (PUT.Component(Component))
                DialogResult = true;
        }
        else
        {
            Component.Title = ComponentTitle.Text;
            Component.ManufacturerId = ((Manufacturer)Manufacturer.SelectedItem).Id;
            Component.ComponentTypeId = ((ComponentType)ComponentType.SelectedItem).Id;
            if (PULL.Component(Component))
                DialogResult = true;
        }
    }
}