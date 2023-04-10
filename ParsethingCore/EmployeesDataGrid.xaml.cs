namespace ParsethingCore;

public partial class EmployeesDataGrid : UserControl, IView
{
    public EmployeesDataGrid() =>
        InitializeComponent();

    public IEnumerable<object>? Objects { get; set; }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) =>
        View.ItemsSource = Objects?.Cast<Employee>();

    private void View_SizeChanged(object sender, SizeChangedEventArgs e) =>
        View.MinColumnWidth = View.ActualWidth / View.Columns.Count;

    public void Add()
    {

    }

    public void Edit()
    {

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
    }
}