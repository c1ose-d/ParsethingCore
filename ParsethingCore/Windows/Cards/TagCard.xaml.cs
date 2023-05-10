namespace ParsethingCore.Windows.Cards;

public partial class TagCard : Window
{
    public TagCard(Tag? tag = null)
    {
        Tag = tag;

        InitializeComponent();

        if (Tag != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = Tag.Keyword;
            Tag_Keyword.Text = Tag.Keyword;

            System_Id.Text = Tag.Id.ToString();
            System_Keyword.Text = Tag.Keyword;
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private Tag? Tag { get; set; }

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

    private void Tag_Keyword_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Tag_Keyword.Text == string.Empty)
            Tag_Keyword_Clear.Visibility = Visibility.Collapsed;
        else Tag_Keyword_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = Tag_Keyword.Text;
    }

    private void Tag_Keyword_Clear_Click(object sender, RoutedEventArgs e) =>
        Tag_Keyword.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Tag == null)
            {
                Tag = new() { Keyword = Tag_Keyword.Text };
                if (PUT.Tag(Tag))
                    DialogResult = true;
            }
            else
            {
                Tag.Keyword = Tag_Keyword.Text;
                if (PULL.Tag(Tag))
                    DialogResult = true;
            }
        }
        catch { }
    }
}