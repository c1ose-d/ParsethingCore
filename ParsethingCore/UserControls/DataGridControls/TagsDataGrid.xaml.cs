using DatabaseLibrary.Entities.ComponentCalculationProperties;

namespace ParsethingCore.UserControls.DataGridControls;

public partial class TagsDataGrid : UserControl, IView
{
    public TagsDataGrid()
    {
        GetTags();
        InitializeComponent();
    }

    private List<Tag>? Tags { get; set; }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) =>
        GetView();

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            TagCard card = new((Tag)View.SelectedItem);
            card.ShowDialog();
        }
        catch { }
        GetView();
    }

    private void View_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try { ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = ((Tag)View.SelectedItem).Id; }
        catch { }
    }

    private void View_SizeChanged(object sender, SizeChangedEventArgs e) =>
        View.MinColumnWidth = View.ActualWidth / View.Columns.Count;

    private void GetTags()
    {
        Tags = GET.View.Tags();
        if (Tags != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = Tags.Count;
    }

    public void GetView()
    {
        GetTags();
        View.ItemsSource = Tags;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
    }

    public void Add()
    {
        TagCard card = new();
        card.ShowDialog();
        GetView();
    }

    public void Edit()
    {
        if ((Tag)View.SelectedItem != null)
        {
            TagCard card = new((Tag)View.SelectedItem);
            card.ShowDialog();
        }
        GetView();
    }

    public void Delete()
    {
        try
        {
            Tag tag = (Tag)View.SelectedItem;
            if (tag != null)
            {
                DeleteConfirmation confirmation = new();
                if (confirmation.ShowDialog() == true)
                    DELETE.Tag(tag);
            }
        }
        catch { }
        GetView();
    }

    public void Export() =>
        ExportInstance.Run(View);
}