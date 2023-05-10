using DatabaseLibrary.Entities.ProcurementProperties;

namespace ParsethingCore.Windows.Cards;

public partial class ProcurementCard : Window
{
    public ProcurementCard(Procurement? procurement = null)
    {
        Procurement = procurement;

        InitializeComponent();

        if (Procurement != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = Procurement.Number;

            Procurement_RequestUri.DataContext = Procurement.RequestUri;
            Procurement_Number.Text = Procurement.Number;
            Procurement_Law.Text = Procurement.Law?.Number;
            Procurement_Object.Text = Procurement.Object;
            Procurement_InitialPrice.Text = Procurement.InitialPrice.ToString();
            Procurement_Organization.Text = Procurement.Organization?.Name;
            Procurement_Organization_PostalAddress.Text = Procurement.Organization?.PostalAddress;
            Procurement_Method.Text = Procurement.Method?.Text;
            Procurement_Platform.Text = Procurement.Platform?.Name;
            Procurement_Platform_Address.DataContext = Procurement.Platform?.Address;
            Procurement_Location.Text = Procurement.Location;
            Procurement_StartDate.Text = $"{Procurement.StartDate?.ToString()} ({Procurement.TimeZone?.Offset})";
            Procurement_Deadline.Text = $"{Procurement.Deadline?.ToString()} ({Procurement.TimeZone?.Offset})";
            Procurement_Securing.Text = Procurement.Securing?.ToString();
            Procurement_Enforcement.Text = Procurement.Enforcement?.ToString();
            Procurement_Warranty.Text = Procurement.Warranty?.ToString();

            System_Id.Text = Procurement.Id.ToString();
            System_RequestUri.Text = Procurement.RequestUri;
            System_Number.Text = Procurement.Number;
            System_LawId.Text = Procurement.LawId.ToString();
            System_Object.Text = Procurement.Object;
            System_InitialPrice.Text = Procurement.InitialPrice.ToString();
            System_OrganizationId.Text = Procurement.OrganizationId.ToString();
            System_MethodId.Text = Procurement.MethodId.ToString();
            System_PlatformId.Text = Procurement.PlatformId.ToString();
            System_Location.Text = Procurement.Location;
            System_StartDate.Text = Procurement.StartDate?.ToString();
            System_Deadline.Text = Procurement.Deadline?.ToString();
            System_TimeZoneId.Text = Procurement.TimeZoneId.ToString();
            System_Securing.Text = Procurement.Securing?.ToString();
            System_Enforcement.Text = Procurement.Enforcement?.ToString();
            System_Warranty.Text = Procurement.Warranty?.ToString();
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private Procurement? Procurement { get; set; }

    private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
    {
        string? fileName = ((Button)sender).DataContext.ToString();
        if (fileName != null)
        {
            _ = Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });
            e.Handled = true;
        }
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
}