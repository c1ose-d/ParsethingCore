namespace ParsethingCore
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Sources Sources { get; set; } = null!;
        private Thread SourcesCaller { get; set; } = null!;

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

        private void MinimizeAction_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

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
            try
            {
                SourcesCaller = new(Sources.Disable);
                SourcesCaller.Start();
            }
            catch { }
            Application.Current.Shutdown();
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            Sources = new();
            SourcesCaller = new(Sources.Enable);
            SourcesCaller.Start();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SourcesCaller = new(Sources.Disable);
                SourcesCaller.Start();
            }
            catch { }
        }
    }
}