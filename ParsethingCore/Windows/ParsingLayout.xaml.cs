namespace ParsethingCore.Windows;

public partial class ParsingLayout : Window
{
    public ParsingLayout()
    {
        InitializeComponent();
    }

    private void MinPrice_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (MinPrice.Text == string.Empty)
            MinPrice_Clear.Visibility = Visibility.Collapsed;
        else MinPrice_Clear.Visibility = Visibility.Visible;

        StringBuilder stringBuilder = new(MinPrice.Text);
        for (int i = stringBuilder.Length - 1; i >= 0; i--)
            if (!char.IsDigit(stringBuilder[i]))
                stringBuilder.Remove(i, 1);
        MinPrice.Text = stringBuilder.ToString();
        MinPrice.CaretIndex = MinPrice.Text.Length;
    }

    private void MinPrice_Clear_Click(object sender, RoutedEventArgs e) =>
        MinPrice.Text = string.Empty;

    private void MaxPrice_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (MaxPrice.Text == string.Empty)
            MaxPrice_Clear.Visibility = Visibility.Collapsed;
        else MaxPrice_Clear.Visibility = Visibility.Visible;

        StringBuilder stringBuilder = new(MaxPrice.Text);
        for (int i = stringBuilder.Length - 1; i >= 0; i--)
            if (!char.IsDigit(stringBuilder[i]))
                stringBuilder.Remove(i, 1);
        MaxPrice.Text = stringBuilder.ToString();
        MaxPrice.CaretIndex = MaxPrice.Text.Length;
    }

    private void MaxPrice_Clear_Click(object sender, RoutedEventArgs e) =>
        MaxPrice.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        if (MinPrice.Text.Length == 0)
            MinPrice.Text = "200000";
        if (MaxPrice.Text.Length == 0)
            MaxPrice.Text = "9000000";
        DialogResult = true;
    }
}