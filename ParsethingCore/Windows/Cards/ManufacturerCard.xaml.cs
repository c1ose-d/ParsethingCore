﻿namespace ParsethingCore.Windows.Cards;

public partial class ManufacturerCard : Window
{
    public ManufacturerCard(Manufacturer? manufacturer = null)
    {
        Manufacturer = manufacturer;

        InitializeComponent();

        if (Manufacturer != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = Manufacturer.Name;
            Manufacturer_Name.Text = Manufacturer.Name;

            System_Id.Text = Manufacturer.Id.ToString();
            System_Name.Text = Manufacturer.Name;
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private Manufacturer? Manufacturer { get; set; }

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

    private void Manufacturer_Name_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Manufacturer_Name.Text == string.Empty)
            Manufacturer_Name_Clear.Visibility = Visibility.Collapsed;
        else Manufacturer_Name_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = Manufacturer_Name.Text;
    }

    private void Manufacturer_Name_Clear_Click(object sender, RoutedEventArgs e) =>
        Manufacturer_Name.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Manufacturer == null)
            {
                Manufacturer = new() { Name = Manufacturer_Name.Text };
                if (PUT.Manufacturer(Manufacturer))
                    DialogResult = true;
            }
            else
            {
                Manufacturer.Name = Manufacturer_Name.Text;
                if (PULL.Manufacturer(Manufacturer))
                    DialogResult = true;
            }
        }
        catch { }
    }
}