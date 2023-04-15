namespace ParsethingCore.UserControls.DataGridControls;

public partial class PreferencesDataGrid : UserControl, IView
{
    public PreferencesDataGrid()
    {
        GetPreferences();
        InitializeComponent();
    }

    private List<Preference>? Preferences { get; set; }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) =>
        GetView();

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            PreferenceCard card = new((Preference)View.SelectedItem);
            card.ShowDialog();
        }
        catch { }
        GetView();
    }

    private void View_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try { ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = ((Preference)View.SelectedItem).Id; }
        catch { }
    }

    private void View_SizeChanged(object sender, SizeChangedEventArgs e) =>
        View.MinColumnWidth = View.ActualWidth / View.Columns.Count;

    private void GetPreferences()
    {
        Preferences = GET.View.Preferences();
        if (Preferences != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = Preferences.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }

    public void GetView()
    {
        GetPreferences();
        View.ItemsSource = Preferences;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        ((TextBox)Application.Current.MainWindow.FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        PreferenceCard card = new();
        card.ShowDialog();
        GetView();
    }

    public void Edit()
    {
        if ((Preference)View.SelectedItem != null)
        {
            PreferenceCard card = new((Preference)View.SelectedItem);
            card.ShowDialog();
        }
        GetView();
    }

    public void Delete()
    {
        try
        {
            Preference preference = (Preference)View.SelectedItem;
            if (preference != null)
            {
                DeleteConfirmation confirmation = new();
                if (confirmation.ShowDialog() == true)
                    DELETE.Preference(preference);
            }
        }
        catch { }
        GetView();
    }

    public void Export() =>
        ExportInstance.Run(View);

    public void Search(string searchString)
    {
        List<Preference>? results = Preferences?
            .Where(p => p.Kind.ToLower().Contains(searchString.ToLower()))
            .ToList();
        View.ItemsSource = results;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
        if (results != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = results.Count;
        else ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = string.Empty;
    }
}