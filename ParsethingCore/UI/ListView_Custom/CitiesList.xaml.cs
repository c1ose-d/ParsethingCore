﻿namespace ParsethingCore.UI.ListView_Custom;

public partial class CitiesList : UserControl, IView
{
    public CitiesList()
    {
        InitializeComponent();
        GetView();
    }

    public void GetView()
    {
        View.ItemsSource = GET.View.Cities();
        ((TextBox)((TitleBar)Application.Current.MainWindow.FindName("TitleBar")).FindName("Search")).Text = string.Empty;
    }

    public void Add()
    {
        if (new ComponentHeaderCard().ShowDialog() == true)
            GetView();
    }

    public void Delete()
    {
        if (View.SelectedIndex != -1 && new DeleteFlyout(((City)View.SelectedItem).Name).ShowDialog() == true)
        {
            DELETE.City((City)View.SelectedItem);
            GetView();
        }
    }

    public void Edit()
    {
        if (View.SelectedIndex != -1)
        {
            new CityCard((City)View.SelectedItem).ShowDialog();
            GetView();
        }
    }

    public void Search(string searchString)
    {
        View.ItemsSource = GET.View.Cities()?
            .Where(e => e.Name.ToLower().Contains(searchString))
            .ToList();
    }

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (View.SelectedIndex != -1)
        {
            new CityCard((City)View.SelectedItem).ShowDialog();
            GetView();
        }
    }
}