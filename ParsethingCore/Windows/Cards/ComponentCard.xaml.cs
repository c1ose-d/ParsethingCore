namespace ParsethingCore.Windows.Cards;

public partial class ComponentCard : Window
{
    public ComponentCard(Component? component = null)
    {
        Component = component;

        InitializeComponent();
        Component_Manufacturer.ItemsSource = GET.View.Manufacturers();
        Component_ComponentType.ItemsSource = GET.View.ComponentTypes();

        if (Component != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = Component.Title;
            Component_Title.Text = Component.Title;
            if (Component_Manufacturer.ItemsSource != null)
                foreach (Manufacturer manufacturer in Component_Manufacturer.ItemsSource)
                    if (manufacturer.Id == Component.ManufacturerId)
                    {
                        Component_Manufacturer.SelectedItem = manufacturer;
                        break;
                    }
            if (Component_ComponentType.ItemsSource != null)
                foreach (ComponentType componentType in Component_ComponentType.ItemsSource)
                    if (componentType.Id == Component.ComponentTypeId)
                    {
                        Component_ComponentType.SelectedItem = componentType;
                        break;
                    }
            System_Id.Text = Component.Id.ToString();
            System_Title.Text = Component.Title;
            System_ManufacturerId.Text = Component.ManufacturerId.ToString();
            System_ComponentTypeId.Text = Component.ComponentTypeId.ToString();
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private Component? Component { get; set; }

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

    private void Component_Title_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Component_Title.Text == string.Empty)
            Component_Title_Clear.Visibility = Visibility.Collapsed;
        else Component_Title_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = Component_Title.Text;
    }

    private void Component_Title_Clear_Click(object sender, RoutedEventArgs e) =>
        Component_Title.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Component == null)
            {
                Component = new()
                {
                    Title = Component_Title.Text,
                    ManufacturerId = ((Manufacturer)Component_Manufacturer.SelectedItem).Id,
                    ComponentTypeId = ((ComponentType)Component_ComponentType.SelectedItem).Id
                };
                if (PUT.Component(Component))
                    DialogResult = true;
            }
            else
            {
                Component.Title = Component_Title.Text;
                Component.ManufacturerId = ((Manufacturer)Component_Manufacturer.SelectedItem).Id;
                Component.ComponentTypeId = ((ComponentType)Component_ComponentType.SelectedItem).Id;
                if (PULL.Component(Component))
                    DialogResult = true;
            }
        }
        catch { }
    }
}