namespace ParsethingCore;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        LogWriter.Initialize();
        InitializeComponent();
    }

    private Sources Sources { get; set; } = null!;
    private Thread SourcesCaller { get; set; } = null!;
    private UserControl DataGrid { get; set; } = null!;

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        switch (WindowState)
        {
            case WindowState.Normal:
                MaximizeAction.Content = "";
                break;
            case WindowState.Maximized:
                MaximizeAction.Content = "";
                break;
        }
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        WindowState = WindowState.Normal;
        DragMove();
    }

    private void MinimizeAction_Click(object sender, RoutedEventArgs e) =>
        WindowState = WindowState.Minimized;

    private void MaximizeAction_Click(object sender, RoutedEventArgs e)
    {
        switch (WindowState)
        {
            case WindowState.Normal:
                WindowState = WindowState.Maximized;
                break;
            case WindowState.Maximized:
                WindowState = WindowState.Normal;
                break;
        }
    }

    private void CloseAction_Click(object sender, RoutedEventArgs e)
    {
        try { SourceCallerRun(Sources.Disable); }
        catch { }
        Process.GetCurrentProcess().Kill();
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        if (DataGrid != null)
            ((IView)DataGrid).Add();
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        if (DataGrid != null)
            ((IView)DataGrid).Edit();
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (DataGrid != null)
            ((IView)DataGrid).Delete();
    }

    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
        if (DataGrid != null)
            ((IView)DataGrid).GetView();
    }

    private void Export_Click(object sender, RoutedEventArgs e)
    {
        if (DataGrid != null)
            ((IView)DataGrid).Export();
    }

    private void Run_Click(object sender, RoutedEventArgs e)
    {
        Run.IsEnabled = false;
        Stop.IsEnabled = true;
        Sources = new();
        SourceCallerRun(Sources.Enable);
    }

    private void Stop_Click(object sender, RoutedEventArgs e)
    {
        Run.IsEnabled = true;
        Stop.IsEnabled = false;
        try { SourceCallerRun(Sources.Disable); }
        catch { }
    }

    private void SourceCallerRun(ThreadStart start)
    {
        try
        {
            SourcesCaller = new(start);
            SourcesCaller.Start();
        }
        catch (Exception ex) { LogWriter.Write(ex); }
    }

    private void Search_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (DataGrid != null)
            ((IView)DataGrid).Search(Search.Text);
    }

    private void Components_GotFocus(object sender, RoutedEventArgs e) =>
        SetDataGrid(new ComponentsDataGrid());

    private void Employees_GotFocus(object sender, RoutedEventArgs e) =>
        SetDataGrid(new EmployeesDataGrid());

    private void Regions_GotFocus(object sender, RoutedEventArgs e) =>
        SetDataGrid(new RegionsDataGrid());

    private void ComponentStates_GotFocus(object sender, RoutedEventArgs e) =>
        SetDataGrid(new ComponentStatesDataGrid());

    private void ComponentTypes_GotFocus(object sender, RoutedEventArgs e) =>
        SetDataGrid(new ComponentTypesDataGrid());

    private void Manufacturers_GotFocus(object sender, RoutedEventArgs e) =>
        SetDataGrid(new ManufacturersDataGrid());

    private void Sellers_GotFocus(object sender, RoutedEventArgs e) =>
        SetDataGrid(new SellersDataGrid());

    private void Tags_GotFocus(object sender, RoutedEventArgs e) =>
        SetDataGrid(new TagsDataGrid());

    private void Positions_GotFocus(object sender, RoutedEventArgs e) =>
        SetDataGrid(new PositionsDataGrid());

    private void SetDataGrid(UserControl view)
    {
        if (DataGrid != null)
            Container.Children.Remove(DataGrid);
        DataGrid = view;
        Grid.SetColumn(DataGrid, 2);
        Container.Children.Add(DataGrid);
    }
}