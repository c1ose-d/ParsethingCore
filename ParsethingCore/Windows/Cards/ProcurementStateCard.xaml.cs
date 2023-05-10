namespace ParsethingCore.Windows.Cards;

public partial class ProcurementStateCard : Window
{
    public ProcurementStateCard(ProcurementState? procurementState = null)
    {
        ProcurementState = procurementState;

        InitializeComponent();

        if (ProcurementState != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = ProcurementState.Kind;
            ProcurementState_Kind.Text = ProcurementState.Kind;

            System_Id.Text = ProcurementState.Id.ToString();
            System_Kind.Text = ProcurementState.Kind;
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private ProcurementState? ProcurementState { get; set; }

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

    private void ProcurementState_Kind_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (ProcurementState_Kind.Text == string.Empty)
            ProcurementState_Kind_Clear.Visibility = Visibility.Collapsed;
        else ProcurementState_Kind_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = ProcurementState_Kind.Text;
    }

    private void ProcurementState_Kind_Clear_Click(object sender, RoutedEventArgs e) =>
        ProcurementState_Kind.Text = string.Empty;

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (ProcurementState == null)
            {
                ProcurementState = new() { Kind = ProcurementState_Kind.Text };
                if (PUT.ProcurementState(ProcurementState))
                    DialogResult = true;
            }
            else
            {
                ProcurementState.Kind = ProcurementState_Kind.Text;
                if (PULL.ProcurementState(ProcurementState))
                    DialogResult = true;
            }
        }
        catch { }
    }
}