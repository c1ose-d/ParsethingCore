namespace ParsethingCore.Windows;

public partial class ParsingFlyout : Window
{
    public ParsingFlyout()
    {
        InitializeComponent();

        List<Region>? regions = GET.View.Regions();
        if (regions != null)
            foreach(Region region in regions)
                View.Items.Add(region);
    }

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

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            Selection.Items.Add((Region)View.SelectedItem);
            View.Items.Remove((Region)View.SelectedItem);
        }
        catch { }
    }

    private void Selection_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            View.Items.Add((Region)Selection.SelectedItem);
            Selection.Items.Remove((Region)Selection.SelectedItem);
        }
        catch { }
    }

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        if (MinPrice.Text.Length == 0)
            MinPrice.Text = "200000";
        if (MaxPrice.Text.Length == 0)
            MaxPrice.Text = "9000000";
        DialogResult = true;
    }
}