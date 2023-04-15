namespace ParsethingCore.Windows.Cards;

public partial class ProcurementStateCard : Window
{
    public ProcurementStateCard(ProcurementState? procurementState = null)
    {
        ProcurementState = procurementState;

        InitializeComponent();

        if (ProcurementState != null)
            Kind.Text = ProcurementState.Kind;
    }

    private ProcurementState? ProcurementState { get; set; }

    private void Approve_Click(object sender, RoutedEventArgs e)
    {
        if (ProcurementState == null)
        {
            ProcurementState = new() { Kind = Kind.Text };
            if (PUT.ProcurementState(ProcurementState))
                DialogResult = true;
        }
        else
        {
            ProcurementState.Kind = Kind.Text;
            if (PULL.ProcurementState(ProcurementState))
                DialogResult = true;
        }
    }
}