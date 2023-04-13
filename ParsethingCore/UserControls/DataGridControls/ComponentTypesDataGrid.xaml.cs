namespace ParsethingCore.UserControls.DataGridControls;

public partial class ComponentTypesDataGrid : UserControl, IView
{
    public ComponentTypesDataGrid()
    {

        GetComponentTypes();
        InitializeComponent();
    }

    private List<ComponentType>? ComponentTypes { get; set; }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) =>
        GetView();

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            ComponentTypeCard card = new((ComponentType)View.SelectedItem);
            card.ShowDialog();
        }
        catch { }
        GetView();
    }

    private void View_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try { ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = ((ComponentType)View.SelectedItem).Id; }
        catch { }
    }

    private void View_SizeChanged(object sender, SizeChangedEventArgs e) =>
        View.MinColumnWidth = View.ActualWidth / View.Columns.Count;

    private void GetComponentTypes()
    {
        ComponentTypes = GET.View.ComponentTypes();
        if (ComponentTypes != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = ComponentTypes.Count;
    }

    public void GetView()
    {
        GetComponentTypes();
        View.ItemsSource = ComponentTypes;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
    }

    public void Add()
    {
        ComponentTypeCard card = new();
        card.ShowDialog();
        GetView();
    }

    public void Edit()
    {
        if ((ComponentType)View.SelectedItem != null)
        {
            ComponentTypeCard card = new((ComponentType)View.SelectedItem);
            card.ShowDialog();
        }
        GetView();
    }

    public void Delete()
    {
        try
        {
            ComponentType componentType = (ComponentType)View.SelectedItem;
            if (componentType != null)
            {
                DeleteConfirmation confirmation = new();
                if (confirmation.ShowDialog() == true)
                    DELETE.ComponentType(componentType);
            }
        }
        catch { }
        GetView();
    }

    public void Export() =>
        ExportInstance.Run(View);
}