namespace ParsethingCore.Windows;

public partial class ApprovalFlyout : Window
{
    public ApprovalFlyout(string elementName)
    {
        InitializeComponent();

        ElementName.Text = elementName;
    }

    private void Confirm_Click(object sender, RoutedEventArgs e) =>
        DialogResult = true;
}