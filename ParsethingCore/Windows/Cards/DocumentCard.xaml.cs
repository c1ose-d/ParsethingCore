namespace ParsethingCore.Windows.Cards;

public partial class DocumentCard : Window
{
    public DocumentCard(Document? document = null)
    {
        Document = document;

        InitializeComponent();

        if (Document != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = Document.Title;
            Document_Title.Text = Document.Title;

            System_Id.Text = Document.Id.ToString();
            System_Title.Text = Document.Title;
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private Document? Document { get; set; }

    private void SideNav_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            foreach (Grid grid in CardView.Children)
                grid.Visibility = Visibility.Collapsed;
            CardView.Children[SideNav.SelectedIndex].Visibility = Visibility.Visible;
        }
        catch { }
    }

    private void Document_Title_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Document_Title.Text == string.Empty)
            Document_Title_Clear.Visibility = Visibility.Collapsed;
        else Document_Title_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = Document_Title.Text;
    }

    private void Document_Title_Clear_Click(object sender, RoutedEventArgs e) =>
        Document_Title.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Document == null)
            {
                Document = new() { Title = Document_Title.Text };
                if (PUT.Document(Document))
                    DialogResult = true;
            }
            else
            {
                Document.Title = Document_Title.Text;
                if (PULL.Document(Document))
                    DialogResult = true;
            }
        }
        catch { }
    }
}