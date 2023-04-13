namespace ParsethingCore.Windows;

public partial class DeleteConfirmation : Window
{
    public DeleteConfirmation() =>
        InitializeComponent();

    private void Approve_Click(object sender, RoutedEventArgs e) =>
        DialogResult = true;
}