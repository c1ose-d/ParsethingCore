namespace ParsethingCore.Windows.Cards;

public partial class PredefinedComponentCard : Window
{
    public PredefinedComponentCard(PredefinedComponent? predefinedComponent = null)
    {
        PredefinedComponent = predefinedComponent;

        InitializeComponent();
        PredefinedComponent_Manufacturer.ItemsSource = GET.View.Manufacturers();
        PredefinedComponent_ComponentType.ItemsSource = GET.View.ComponentTypes();

        if (PredefinedComponent != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = PredefinedComponent.ComponentName;
            PredefinedComponent_Name.Text = PredefinedComponent.ComponentName;
            if (PredefinedComponent_Manufacturer.ItemsSource != null)
            {
                foreach (Manufacturer manufacturer in PredefinedComponent_Manufacturer.ItemsSource)
                {
                    if (manufacturer.Id == PredefinedComponent.ManufacturerId)
                    {
                        PredefinedComponent_Manufacturer.SelectedItem = manufacturer;
                        break;
                    }
                }
            }
            if (PredefinedComponent_ComponentType.ItemsSource != null)
            {
                foreach (ComponentType componentType in PredefinedComponent_ComponentType.ItemsSource)
                {
                    if (componentType.Id == PredefinedComponent.ComponentTypeId)
                    {
                        PredefinedComponent_ComponentType.SelectedItem = componentType;
                        break;
                    }
                }
            }
            PredefinedComponent_Price.Text = PredefinedComponent.Price.ToString();

            System_Id.Text = PredefinedComponent.Id.ToString();
            System_Name.Text = PredefinedComponent.ComponentName;
            System_ManufacturerId.Text = PredefinedComponent.ManufacturerId.ToString();
            System_ComponentTypeId.Text = PredefinedComponent.ComponentTypeId.ToString();
            System_Price.Text = PredefinedComponent.Price.ToString();
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private PredefinedComponent? PredefinedComponent { get; set; }

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

    private void PredefinedComponent_Name_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (PredefinedComponent_Name.Text == string.Empty)
            PredefinedComponent_Name_Clear.Visibility = Visibility.Collapsed;
        else PredefinedComponent_Name_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = PredefinedComponent_Name.Text;
    }

    private void PredefinedComponent_Name_Clear_Click(object sender, RoutedEventArgs e) =>
        PredefinedComponent_Name.Text = string.Empty;

    private void PredefinedComponent_Price_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (PredefinedComponent_Price.Text == string.Empty)
            PredefinedComponent_Price_Clear.Visibility = Visibility.Collapsed;
        else PredefinedComponent_Price_Clear.Visibility = Visibility.Visible;
    }

    private void PredefinedComponent_Price_Clear_Click(object sender, RoutedEventArgs e) =>
        PredefinedComponent_Price.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (PredefinedComponent == null)
            {
                PredefinedComponent = new()
                {
                    ComponentName = PredefinedComponent_Name.Text,
                    ManufacturerId = ((Manufacturer)PredefinedComponent_Manufacturer.SelectedItem).Id,
                    ComponentTypeId = ((ComponentType)PredefinedComponent_ComponentType.SelectedItem).Id,
                    Price = decimal.TryParse(PredefinedComponent_Price.Text, out decimal result) ? result : 0
                };
                if (PUT.PredefinedComponent(PredefinedComponent))
                    DialogResult = true;
            }
            else
            {
                PredefinedComponent.ComponentName = PredefinedComponent_Name.Text;
                PredefinedComponent.ManufacturerId = ((Manufacturer)PredefinedComponent_Manufacturer.SelectedItem).Id;
                PredefinedComponent.ComponentTypeId = ((ComponentType)PredefinedComponent_ComponentType.SelectedItem).Id;
                PredefinedComponent.Price = decimal.TryParse(PredefinedComponent_Price.Text, out decimal result) ? result : 0;
                if (PULL.PredefinedComponent(PredefinedComponent))
                    DialogResult = true;
            }
        }
        catch { }
    }
}