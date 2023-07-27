namespace ParsethingCore.Windows;

public partial class ParsingFlyout : Window
{
    public ParsingFlyout()
    {
        InitializeComponent();

        ViewList = GET.View.Regions()?.
            OrderBy(r => r.Title).
            ToList();
        View.ItemsSource = ViewList;
        Selection.ItemsSource = SelectionList;

        try
        {
            string[] config = File.ReadAllLines("Config.txt");
            MinPrice.Text = config[0];
            MaxPrice.Text = config[1];
            List<string> regions = config[2].Split(";").ToList();
            regions.RemoveAt(regions.Count - 1);
            foreach (string regionId in regions)
            {
                Region? region = ViewList?.
                    Where(r => r.Id == Convert.ToInt32(regionId)).
                    First();
                Selection.ItemsSource = null;
                View.ItemsSource = null;

                if (region != null)
                {
                    SelectionList.Add(region);
                    SelectionList = SelectionList.
                    OrderBy(r => r.Title).
                    ToList();
                    Selection.ItemsSource = SelectionList.
                    Where(r => r.Title.ToLower().Contains(Search.Text.ToLower())).
                    ToList();

                    ViewList?.Remove(region);
                    ViewList = ViewList?.
                    OrderBy(r => r.Title).
                    ToList();
                    View.ItemsSource = ViewList?.
                    Where(r => r.Title.ToLower().Contains(Search.Text.ToLower())).
                    ToList();
                }
            }
        }
        catch { }
    }

    private List<Region>? ViewList { get; set; } = new();
    private List<Region> SelectionList { get; set; } = new();

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

    private void ClearConfig_Click(object sender, RoutedEventArgs e)
    {
        File.Create("Config.txt").Close();
        ViewList = GET.View.Regions()?.
            OrderBy(r => r.Title).
            ToList();
        View.ItemsSource = null;
        View.ItemsSource = ViewList;
        SelectionList = new();
        Selection.ItemsSource = null;
        Selection.ItemsSource = SelectionList;
        MinPrice.Text = "";
        MaxPrice.Text = "";
    }

    private void Search_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Search.Text == string.Empty)
            Clear.Visibility = Visibility.Collapsed;
        else Clear.Visibility = Visibility.Visible;

        try
        {
            Selection.ItemsSource = null;
            View.ItemsSource = null;

            Selection.ItemsSource = SelectionList.
            Where(r => r.Title.ToLower().Contains(Search.Text.ToLower())).
            ToList();

            View.ItemsSource = ViewList?.
            Where(r => r.Title.ToLower().Contains(Search.Text.ToLower())).
            ToList();
        }
        catch { }
    }

    private void Clear_Click(object sender, RoutedEventArgs e) =>
        Search.Text = string.Empty;

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            List<Region> regions = View.SelectedItems.Cast<Region>().ToList();
            Selection.ItemsSource = null;
            View.ItemsSource = null;

            foreach (Region region in regions)
                SelectionList.Add(region);
            SelectionList = SelectionList.
            OrderBy(r => r.Title).
            ToList();
            Selection.ItemsSource = SelectionList.
            Where(r => r.Title.ToLower().Contains(Search.Text.ToLower())).
            ToList();

            foreach (Region region in regions)
                ViewList?.Remove(region);
            ViewList = ViewList?.
            OrderBy(r => r.Title).
            ToList();
            View.ItemsSource = ViewList?.
            Where(r => r.Title.ToLower().Contains(Search.Text.ToLower())).
            ToList();
        }
        catch { }
    }

    private void Selection_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            List<Region> regions = Selection.SelectedItems.Cast<Region>().ToList();
            View.ItemsSource = null;
            Selection.ItemsSource = null;

            foreach (Region region in regions)
                ViewList?.Add(region);
            ViewList = ViewList?.OrderBy(r => r.Title).ToList();
            View.ItemsSource = ViewList?.
            Where(r => r.Title.ToLower().Contains(Search.Text.ToLower())).
            ToList();

            foreach (Region region in regions)
                SelectionList.Remove(region);
            SelectionList = SelectionList.OrderBy(r => r.Title).ToList();
            Selection.ItemsSource = SelectionList.
            Where(r => r.Title.ToLower().Contains(Search.Text.ToLower())).
            ToList();
        }
        catch { }
    }

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        string configString = $"{MinPrice.Text}\n{MaxPrice.Text}\n";
        foreach (Region region in SelectionList)
            configString += $"{region.Id};";
        File.Create("Config.txt").Close();
        File.WriteAllText("Config.txt", configString);

        if (MinPrice.Text.Length == 0)
            MinPrice.Text = "200000";
        if (MaxPrice.Text.Length == 0)
            MaxPrice.Text = "9000000";
        DialogResult = true;
    }
}