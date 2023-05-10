namespace ParsethingCore.Windows;

public partial class MessageFlyout : Window
{
    public MessageFlyout(string title, string content)
    {
        InitializeComponent();

        Title = title;
        TitleBlock.Text = title;
        ContentBlock.Text = content;
    }
}