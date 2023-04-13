using ParsethingCore.Windows.Cards;

namespace ParsethingCore;

public partial class EmployeesDataGrid : UserControl, IView
{
    public EmployeesDataGrid()
    {
        GetEmployees();
        InitializeComponent();
    }

    private List<Employee>? Employees;
    public int Count { get; private set; }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) =>
        GetView();

    private void View_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        EmployeeCard card = new((Employee)View.SelectedItem);
        card.ShowDialog();
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
            Count = Employees.Count;
    }

    private void GetView()
    {
        GetEmployees();
        View.ItemsSource = Employees;
    }

    public void Add()
    {
        EmployeeCard card = new();
        card.ShowDialog();
        GetView();
    }

    public void Edit()
    {
        try
        {
            EmployeeCard card = new((Employee)View.SelectedItem);
            card.ShowDialog();
        }
        catch { }
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
}