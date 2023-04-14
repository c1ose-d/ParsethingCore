namespace ParsethingCore.UserControls.DataGridControls;

public partial class ComponentStatesDataGrid : UserControl, IView
{
    public ComponentStatesDataGrid()
    {
        GetComponentStates();
        InitializeComponent();
    }

    private List<ComponentState>? ComponentStates { get; set; }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) =>
        GetView();

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            ComponentStateCard card = new((ComponentState)View.SelectedItem);
            card.ShowDialog();
        }
        catch { }
        GetView();
    }

    private void View_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try { ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = ((ComponentState)View.SelectedItem).Id; }
        catch { }
    }

    private void View_SizeChanged(object sender, SizeChangedEventArgs e) =>
        View.MinColumnWidth = View.ActualWidth / View.Columns.Count;

    private void GetComponentStates()
    {
        ComponentStates = GET.View.ComponentStates();
        if (ComponentStates != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = ComponentStates.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }

    public void GetView()
    {
        GetComponentStates();
        View.ItemsSource = ComponentStates;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        ((TextBox)Application.Current.MainWindow.FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        ComponentStateCard card = new();
        card.ShowDialog();
        GetView();
    }

    public void Edit()
    {
        if ((ComponentState)View.SelectedItem != null)
        {
            ComponentStateCard card = new((ComponentState)View.SelectedItem);
            card.ShowDialog();
        }
        GetView();
    }

    public void Delete()
    {
        try
        {
            ComponentState componentState = (ComponentState)View.SelectedItem;
            if (componentState != null)
            {
                DeleteConfirmation confirmation = new();
                if (confirmation.ShowDialog() == true)
                    DELETE.ComponentState(componentState);
            }
        }
        catch { }
        GetView();
    }

    public void Export() =>
        ExportInstance.Run(View);

    public void Search(string searchString)
    {
        List<ComponentState>? results = ComponentStates?
            .Where(cs => cs.Kind.ToLower().Contains(searchString.ToLower()))
            .ToList();
        View.ItemsSource = results;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        if (results != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = results.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }
}