namespace ParsethingCore.UI.ListView_Custom;

public partial class LegalEntitiesList : UserControl, IView
{
    public LegalEntitiesList()
    {
        InitializeComponent();
        GetView();
    }

    public void GetView()
    {
        View.ItemsSource = GET.View.LegalEntities();
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        if (new LegalEntityCard().ShowDialog() == true)
            GetView();
    }

    public void Delete()
    {
        if (View.SelectedIndex != -1 && new DeleteFlyout(((LegalEntity)View.SelectedItem).Name).ShowDialog() == true)
        {
            DELETE.LegalEntity((LegalEntity)View.SelectedItem);
            GetView();
        }
    }

    public void Edit()
    {
        if (View.SelectedIndex != -1)
        {
            new LegalEntityCard((LegalEntity)View.SelectedItem).ShowDialog();
            GetView();
        }
    }

    public void Export() { }

    public void Search(string searchString)
    {
        View.ItemsSource = GET.View.LegalEntities()?
            .Where(e => e.Name.ToLower().Contains(searchString))
            .ToList();
    }

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (View.SelectedIndex != -1)
        {
            new LegalEntityCard((LegalEntity)View.SelectedItem).ShowDialog();
            GetView();
        }
    }
}