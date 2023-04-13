namespace ParsethingCore.UserControls.DataGridControls;

public partial class PositionsDataGrid : UserControl, IView
{
    public PositionsDataGrid()
    {
        GetPositions();
        InitializeComponent();
    }

    private List<Position>? Positions { get; set; }

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
        try { ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = ((Position)View.SelectedItem).Id; }
        catch { }
    }

    private void View_SizeChanged(object sender, SizeChangedEventArgs e) =>
        View.MinColumnWidth = View.ActualWidth / View.Columns.Count;

    private void GetPositions()
    {
        Positions = GET.View.Positions();
        if (Positions != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = Positions.Count;
    }

    public void GetView()
    {
        GetPositions();
        View.ItemsSource = Positions;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
    }

    public void Add()
    {
        PositionCard card = new();
        card.ShowDialog();
        GetView();
    }

    public void Edit()
    {
        if ((Position)View.SelectedItem != null)
        {
            PositionCard card = new((Position)View.SelectedItem);
            card.ShowDialog();
        }
        GetView();
    }

    public void Delete()
    {
        try
        {
            Position position = (Position)View.SelectedItem;
            if (position != null)
            {
                DeleteConfirmation confirmation = new();
                if (confirmation.ShowDialog() == true)
                    DELETE.Position(position);
            }
        }
        catch { }
        GetView();
    }

    public void Export() =>
        ExportInstance.Run(View);
}