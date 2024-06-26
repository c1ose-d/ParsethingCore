namespace ParsethingCore.Windows;

public partial class ParsingOnceFlyout : Window
{
    public ParsingOnceFlyout()
    {
        InitializeComponent();
    }

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}