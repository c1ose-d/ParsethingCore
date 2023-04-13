namespace ParsethingCore.Windows.Cards;

public partial class EmployeeCard : Window
{
    public EmployeeCard(Employee? employee = null)
    {
        Employee = employee;
        Positions = GET.View.Positions();

        InitializeComponent();
        Position.ItemsSource = Positions;

        if (Employee != null && Position.ItemsSource != null)
        {
            FullName.Text = Employee.FullName;
            UserName.Text = Employee.UserName;
            Password.Text = Employee.Password;
            foreach (Position position in Position.ItemsSource)
                if (position.Id == Employee.PositionId)
                {
                    Position.SelectedItem = position;
                    break;
                }
            if (Employee.Photo != null)
            {
                Bytes = Convert.FromBase64String(Employee.Photo);
                Image = new();
                Image.BeginInit();
                Image.StreamSource = new MemoryStream(Bytes);
                Image.EndInit();
                Photo.Content = new Image { Source = Image };
                String = Convert.ToBase64String(Bytes);
            }
        }
    }

    private Employee? Employee { get; set; }
    private List<Position>? Positions { get; set; }
    private BitmapImage? Image { get; set; } = null;
    private byte[]? Bytes { get; set; } = null;
    private string? String { get; set; } = null;

    private void Photo_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new();
        bool? result = dialog.ShowDialog();
        if (result == true)
        {
            Bytes = File.ReadAllBytes(dialog.FileName);
            Image = new();
            Image.BeginInit();
            Image.StreamSource = new MemoryStream(Bytes);
            Image.EndInit();
            Photo.Content = new Image { Source = Image };
            String = Convert.ToBase64String(Bytes);
        }
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        Photo.Content = "";
        String = null;
    }

    private void Approve_Click(object sender, RoutedEventArgs e)
    {
        if (Employee == null)
        {
            Employee = new()
            {
                FullName = FullName.Text,
                UserName = UserName.Text,
                Password = Password.Text,
                PositionId = ((Position)Position.SelectedItem).Id,
                Photo = String
            };
            if (PUT.Employee(Employee))
                DialogResult = true;
        }
        else
        {
            Employee.FullName = FullName.Text;
            Employee.UserName = UserName.Text;
            Employee.Password = Password.Text;
            Employee.PositionId = ((Position)Position.SelectedItem).Id;
            Employee.Photo = String;
            if (PULL.Employee(Employee))
                DialogResult = true;
        }
    }
}