namespace ParsethingCore.UI;

public partial class TitleBar : UserControl
{
    public TitleBar()
    {
        InitializeComponent();

        MainWindow = Application.Current.MainWindow;
        MainWindow.SizeChanged += MainWindow_SizeChanged;
    }

    private Window MainWindow { get; set; } = null!;
    private Border ListViewContainer { get; set; } = null!;

    private void Surface_Loaded(object sender, RoutedEventArgs e) =>
        ListViewContainer = (Border)Application.Current.MainWindow.FindName("DataGridContainer");

    private void Surface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        MainWindow.WindowState = WindowState.Normal;
        MainWindow.DragMove();
    }

    private void Min_Click(object sender, RoutedEventArgs e) =>
        MainWindow.WindowState = WindowState.Minimized;

    private void Max_Click(object sender, RoutedEventArgs e)
    {
        switch (MainWindow.WindowState)
        {
            case WindowState.Normal:
                try
                {
                    MainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                    MainWindow.WindowState = WindowState.Maximized;
                    MainWindow.WindowStyle = WindowStyle.None;
                }
                catch
                {
                    MainWindow.WindowState = WindowState.Maximized;
                }
                break;
            case WindowState.Maximized:
                MainWindow.WindowState = WindowState.Normal;
                break;
        }
    }

    private void Search_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Search.Text == string.Empty)
            Clear.Visibility = Visibility.Collapsed;
        else Clear.Visibility = Visibility.Visible;
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        foreach (Process process in Process.GetProcessesByName("msedgedriver"))
        {
            process.Kill();
            Thread.Sleep(5000);
        }
        Process.GetCurrentProcess().Kill();
    }

    private void Execute_Click(object sender, RoutedEventArgs e)
    {
        try { ((IView)ListViewContainer.Child).Search(Search.Text.ToLower()); }
        catch { }
    }

    private void Clear_Click(object sender, RoutedEventArgs e) =>
        Search.Text = string.Empty;

    private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        switch (MainWindow.WindowState)
        {
            case WindowState.Normal:
                Max.Content = "";
                break;
            case WindowState.Maximized:
                Max.Content = "";
                break;
        }
    }
}