namespace ParsethingCore.Windows.Cards;

public partial class TagExceptionCard : Window
{
    public TagExceptionCard(TagException? tagException = null)
    {
        TagException = tagException;

        InitializeComponent();

        if (TagException != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = TagException.Keyword;
            TagException_Keyword.Text = TagException.Keyword;

            System_Id.Text = TagException.Id.ToString();
            System_Keyword.Text = TagException.Keyword;
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private TagException? TagException { get; set; }

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

    private void TagException_Keyword_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (TagException_Keyword.Text == string.Empty)
            TagException_Keyword_Clear.Visibility = Visibility.Collapsed;
        else TagException_Keyword_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = TagException_Keyword.Text;
    }

    private void TagException_Keyword_Clear_Click(object sender, RoutedEventArgs e) =>
        TagException_Keyword.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (TagException == null)
            {
                TagException = new() { Keyword = TagException_Keyword.Text };
                if (PUT.TagException(TagException))
                    DialogResult = true;
            }
            else
            {
                TagException.Keyword = TagException_Keyword.Text;
                if (PULL.TagException(TagException))
                    DialogResult = true;
            }
        }
        catch { }
    }
}