namespace ParsethingCore.Windows;

public partial class DeleteConfirmation : Window
{
    public DeleteConfirmation() =>
        InitializeComponent();

    private void Button_Click(object sender, RoutedEventArgs e) =>
        DialogResult = true;
}