namespace ParsethingCore.UserControls.DataGridControls;

public partial class ComponentsDataGrid : UserControl, IView
{
    public ComponentsDataGrid()
    {
        GetComponents();
        InitializeComponent();
    }

    private List<Component>? Components { get; set; }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) =>
        GetView();

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            ComponentCard card = new((Component)View.SelectedItem);
            card.ShowDialog();
        }
        catch { }
        GetView();
    }

    private void View_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try { ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = ((Component)View.SelectedItem).Id; }
        catch { }
    }

    private void View_SizeChanged(object sender, SizeChangedEventArgs e) =>
        View.MinColumnWidth = View.ActualWidth / View.Columns.Count;

    private void GetComponents()
    {
        Components = GET.View.Components();
        if (Components != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = Components.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }

    public void GetView()
    {
        GetComponents();
        View.ItemsSource = Components;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        ((TextBox)Application.Current.MainWindow.FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        ComponentCard card = new();
        card.ShowDialog();
        GetView();
    }

    public void Edit()
    {
        if ((Component)View.SelectedItem != null)
        {
            ComponentCard card = new((Component)View.SelectedItem);
            card.ShowDialog();
        }
        GetView();
    }

    public void Delete()
    {
        try
        {
            Component component = (Component)View.SelectedItem;
            if (component != null)
            {
                DeleteConfirmation confirmation = new();
                if (confirmation.ShowDialog() == true)
                    DELETE.Component(component);
            }
        }
        catch { }
        GetView();
    }

    public void Export() =>
        ExportInstance.Run(View);

    public void Search(string searchString)
    {
        List<Component>? results = Components?
            .Where(c => c.Title.ToLower().Contains(searchString.ToLower())
            || c.Manufacturer.Name.ToLower().Contains(searchString.ToLower())
            || c.ComponentType.Kind.ToLower().Contains(searchString.ToLower()))
            .ToList();
        View.ItemsSource = results;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        if (results != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = results.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }
}