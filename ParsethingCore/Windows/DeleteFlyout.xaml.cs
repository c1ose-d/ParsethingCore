namespace ParsethingCore.Windows;

public partial class DeleteFlyout : Window
{
    public DeleteFlyout(string elementName)
    {
        InitializeComponent();

        ElementName.Text = elementName;
    }

    private void Confirm_Click(object sender, RoutedEventArgs e) =>
        DialogResult = true;
}