namespace ParsethingCore.Windows.Cards;

public partial class LegalEntityCard : Window
{
    public LegalEntityCard(LegalEntity? legalEntity = null)
    {
        LegalEntity = legalEntity;

        InitializeComponent();

        if (LegalEntity != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = LegalEntity.Name;
            LegalEntity_Name.Text = LegalEntity.Name;

            System_Id.Text = LegalEntity.Id.ToString();
            System_Name.Text = LegalEntity.Name;
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private LegalEntity? LegalEntity { get; set; }

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

    private void LegalEntity_Name_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (LegalEntity_Name.Text == string.Empty)
            LegalEntity_Name_Clear.Visibility = Visibility.Collapsed;
        else LegalEntity_Name_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = LegalEntity_Name.Text;
    }

    private void LegalEntity_Name_Clear_Click(object sender, RoutedEventArgs e) =>
        LegalEntity_Name.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (LegalEntity == null)
            {
                LegalEntity = new() { Name = LegalEntity_Name.Text };
                if (PUT.LegalEntity(LegalEntity))
                    DialogResult = true;
            }
            else
            {
                LegalEntity.Name = LegalEntity_Name.Text;
                if (PULL.LegalEntity(LegalEntity))
                    DialogResult = true;
            }
        }
        catch { }
    }
}