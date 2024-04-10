namespace ParsethingCore.UI.ListView_Custom;

public partial class TagExceptionsList : UserControl, IView
{
    public TagExceptionsList()
    {
        InitializeComponent();
        GetView();
    }

    public void GetView()
    {
        View.ItemsSource = GET.View.TagExceptions();
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        if (new TagExceptionCard().ShowDialog() == true)
            GetView();
    }

    public void Delete()
    {
        if (View.SelectedIndex != -1 && new DeleteFlyout(((TagException)View.SelectedItem).Keyword).ShowDialog() == true)
        {
            DELETE.TagException((TagException)View.SelectedItem);
            GetView();
        }
    }

    public void Edit()
    {
        if (View.SelectedIndex != -1)
        {
            new TagExceptionCard((TagException)View.SelectedItem).ShowDialog();
            GetView();
        }
    }

    public void Export() { }

    public void Search(string searchString)
    {
        View.ItemsSource = GET.View.TagExceptions()?
            .Where(e => e.Keyword.ToLower().Contains(searchString))
            .ToList();
    }

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (View.SelectedIndex != -1)
        {
            new TagExceptionCard((TagException)View.SelectedItem).ShowDialog();
            GetView();
        }
    }
}