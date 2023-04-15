namespace ParsethingCore.Windows.Cards;

public partial class LegalEntityCard : Window
{
    public LegalEntityCard(LegalEntity? legalEntity = null)
    {
        LegalEntity = legalEntity;

        InitializeComponent();

        if (LegalEntity != null)
            LegalEntityName.Text = LegalEntity.Name;
    }

    private LegalEntity? LegalEntity { get; set; }

    private void Approve_Click(object sender, RoutedEventArgs e)
    {
        if (LegalEntity == null)
        {
            LegalEntity = new() { Name = LegalEntityName.Text };
            if (PUT.LegalEntity(LegalEntity))
                DialogResult = true;
        }
        else
        {
            LegalEntity.Name = LegalEntityName.Text;
            if (PULL.LegalEntity(LegalEntity))
                DialogResult = true;
        }
    }
}