namespace ParsethingCore.Windows.Cards;

public partial class TagCard : Window
{
    public TagCard(Tag? tag = null)
    {
        Tag = tag;

        InitializeComponent();

        if (Tag != null)
            Keyword.Text = Tag.Keyword;
    }

    private new Tag? Tag { get; set; }

    private void Approve_Click(object sender, RoutedEventArgs e)
    {
        if (Tag == null)
        {
            Tag = new() { Keyword = Keyword.Text };
            if (PUT.Tag(Tag))
                DialogResult = true;
        }
        else
        {
            Tag.Keyword = Keyword.Text;
            if (PULL.Tag(Tag))
                DialogResult = true;
        }
    }
}