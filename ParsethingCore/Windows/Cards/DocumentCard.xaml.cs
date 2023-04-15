namespace ParsethingCore.Windows.Cards;

public partial class DocumentCard : Window
{
    public DocumentCard(Document? document = null)
    {
        Document = document;

        InitializeComponent();

        if (Document != null)
            DocumentTitle.Text = Document.Title;
    }

    private Document? Document { get; set; }

    private void Approve_Click(object sender, RoutedEventArgs e)
    {
        if (Document == null)
        {
            Document = new() { Title = DocumentTitle.Text };
            if (PUT.Document(Document))
                DialogResult = true;
        }
        else
        {
            Document.Title = DocumentTitle.Text;
            if (PULL.Document(Document))
                DialogResult = true;
        }
    }
}