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
        if (Sources != null)
            SourceCallerRun(Sources.Disable);
        Application.Current.Shutdown();
    }

    private void Run_Click(object sender, RoutedEventArgs e)
    {
        Sources = new();
        SourceCallerRun(Sources.Enable);
    }

    private void Stop_Click(object sender, RoutedEventArgs e)
    {
        if (Sources != null)
            SourceCallerRun(Sources.Disable);
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

    private void TreeViewItem_GotFocus(object sender, RoutedEventArgs e)
    {
        Container.Children.Remove(DataGrid);
        DataGrid = new EmployeesDataGrid();
        IEnumerable<object>? objects = GET.Employees()?.Cast<object>();
        ((IView)DataGrid).Objects = objects;
        EntriesCount.Content = objects?.Count();
        Grid.SetColumn(DataGrid, 2);
        Container.Children.Add(DataGrid);
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (DataGrid != null)
            ((IView)DataGrid).Delete();
    }
}