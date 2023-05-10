namespace ParsethingCore.UI.ListView_Custom;

public partial class ProcurementStatesList : UserControl, IView
{
    public ProcurementStatesList()
    {
        InitializeComponent();
        GetView();
    }

    public void GetView()
    {
        View.ItemsSource = GET.View.ProcurementStates();
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        if (new ProcurementStateCard().ShowDialog() == true)
            GetView();
    }

    public void Delete()
    {
        if (View.SelectedIndex != -1 && new DeleteFlyout(((ProcurementState)View.SelectedItem).Kind).ShowDialog() == true)
        {
            DELETE.ProcurementState((ProcurementState)View.SelectedItem);
            GetView();
        }
    }

    public void Edit()
    {
        if (View.SelectedIndex != -1)
        {
            new ProcurementStateCard((ProcurementState)View.SelectedItem).ShowDialog();
            GetView();
        }
    }

    public void Export() { }

    public void Search(string searchString)
    {
        View.ItemsSource = GET.View.ProcurementStates()?
            .Where(e => e.Kind.ToLower().Contains(searchString))
            .ToList();
    }

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (View.SelectedIndex != -1)
        {
            new ProcurementStateCard((ProcurementState)View.SelectedItem).ShowDialog();
            GetView();
        }
    }
}