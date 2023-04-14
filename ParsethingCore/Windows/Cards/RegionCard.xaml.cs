using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Text;

namespace ParsethingCore.Windows.Cards;

public partial class RegionCard : Window
{
    public RegionCard(Region? region = null)
    {
        Region = region;

        InitializeComponent();

        if (Region != null)
        {
            RegionTitle.Text = Region.Title;
            Distance.Text = Region.Distance.ToString();
        }
    }

    private Region? Region { get; set; }

    private void Distance_TextChanged(object sender, TextChangedEventArgs e)
    {
        StringBuilder stringBuilder = new(Distance.Text);
        for (int i = stringBuilder.Length - 1; i >= 0; i--)
            if (!char.IsDigit(stringBuilder[i]))
                stringBuilder.Remove(i, 1);
        Distance.Text = stringBuilder.ToString();
        Distance.CaretIndex = Distance.Text.Length;
    }

    private void Approve_Click(object sender, RoutedEventArgs e)
    {
        if (Region == null)
        {
            Region = new()
            {
                Title = RegionTitle.Text,
                Distance = Convert.ToInt32(Distance.Text)
            };
            if (PUT.Region(Region))
                DialogResult = true;
        }
        else
        {
            Region.Title = RegionTitle.Text;
            Region.Distance = Convert.ToInt32(Distance.Text);
            if (PULL.Region(Region))
                DialogResult = true;
        }
    }
}