using DatabaseLibrary.Entities.ComponentCalculationProperties;
using ExportLibrary;

namespace ParsethingCore.UserControls.DataGridControls;

public partial class EmployeesDataGrid : UserControl, IView
{
    public EmployeesDataGrid()
    {
        GetEmployees();
        InitializeComponent();
    }

    private List<Employee>? Employees { get; set; }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) =>
        GetView();

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            EmployeeCard card = new((Employee)View.SelectedItem);
            card.ShowDialog();
        }
        catch { }
        GetView();
    }

    private void View_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try { ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = ((Employee)View.SelectedItem).Id; }
        catch { }
    }

    private void View_SizeChanged(object sender, SizeChangedEventArgs e) =>
        View.MinColumnWidth = View.ActualWidth / View.Columns.Count;

    private void GetEmployees()
    {
        Employees = GET.View.Employees();
        if (Employees != null)
            ((Label)Application.Current.MainWindow.FindName("EntriesCount")).Content = Employees.Count;
    }

    public void GetView()
    {
        GetEmployees();
        View.ItemsSource = Employees;
        ((Label)Application.Current.MainWindow.FindName("CurrentId")).Content = string.Empty;
    }

    public void Add()
    {
        EmployeeCard card = new();
        card.ShowDialog();
        GetView();
    }

    public void Edit()
    {
        if ((Employee)View.SelectedItem != null)
        {
            EmployeeCard card = new((Employee)View.SelectedItem);
            card.ShowDialog();
        }
        GetView();
    }

    public void Delete()
    {
        try
        {
            Employee employee = (Employee)View.SelectedItem;
            if (employee != null)
            {
                DeleteConfirmation confirmation = new();
                if (confirmation.ShowDialog() == true)
                    DELETE.Employee(employee);
            }
        }
        catch { }
        GetView();
    }

    public void Export() =>
        ExportInstance.Run(View);
}