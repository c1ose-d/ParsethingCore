namespace ParsethingCore.Windows.Cards;

public partial class EmployeeCard : Window
{
    public EmployeeCard(Employee? employee = null)
    {
        Employee = employee;

        InitializeComponent();
        Employee_Position.ItemsSource = GET.View.Positions();

        if (Employee != null)
        {
            Subtitle.Visibility = Visibility.Visible;
            Subtitle.Text = Employee.FullName;
            Employee_FullName.Text = Employee.FullName;
            Employee_UserName.Text = Employee.UserName;
            Employee_Password.Text = Employee.Password;
            if (Employee_Position.ItemsSource != null)
                foreach (Position position in Employee_Position.ItemsSource)
                    if (position.Id == Employee.PositionId)
                    {
                        Employee_Position.SelectedItem = position;
                        break;
                    }
            if (Employee.Photo != null)
            {
                Bytes = Convert.FromBase64String(Employee.Photo);
                SetPhoto();
            }
            Employee_IsAvailable.IsChecked = Employee.IsAvailable;
            System_Id.Text = Employee.Id.ToString();
            System_FullName.Text = Employee.FullName;
            System_UserName.Text = Employee.UserName;
            System_Password.Text = Employee.Password;
            System_PositionId.Text = Employee.PositionId.ToString();
            if (Employee.Photo != null)
                System_Photo.Text = Employee.Photo.ToString();
            else System_Photo.Text = "NULL";
            System_IsAvailable.Text = Employee.IsAvailable.ToString();
        }
        else SystemFields.Visibility = Visibility.Collapsed;
    }

    private Employee? Employee { get; set; }
    private BitmapImage? Image { get; set; } = null;
    private byte[]? Bytes { get; set; } = null;
    private string? String { get; set; } = null;

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

    private void Employee_FullName_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Employee_FullName.Text == string.Empty)
            Employee_FullName_Clear.Visibility = Visibility.Collapsed;
        else Employee_FullName_Clear.Visibility = Visibility.Visible;

        Subtitle.Text = Employee_FullName.Text;
    }

    private void Employee_FullName_Clear_Click(object sender, RoutedEventArgs e) =>
        Employee_FullName.Text = string.Empty;

    private void Employee_UserName_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Employee_UserName.Text == string.Empty)
            Employee_UserName_Clear.Visibility = Visibility.Collapsed;
        else Employee_UserName_Clear.Visibility = Visibility.Visible;
    }

    private void Employee_UserName_Clear_Click(object sender, RoutedEventArgs e) =>
        Employee_UserName.Text = string.Empty;

    private void Employee_Password_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Employee_Password.Text == string.Empty)
            Employee_Password_Clear.Visibility = Visibility.Collapsed;
        else Employee_Password_Clear.Visibility = Visibility.Visible;
    }

    private void Employee_Password_Clear_Click(object sender, RoutedEventArgs e) =>
        Employee_Password.Text = string.Empty;

    private void Employee_Photo_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new();
        bool? result = dialog.ShowDialog();
        if (result == true)
        {
            Bytes = File.ReadAllBytes(dialog.FileName);
            SetPhoto();
        }
    }

    private void Employee_Photo_Clear_Click(object sender, RoutedEventArgs e)
    {
        Employee_Photo.Content = "";
        String = null;
    }

    private void Confirm_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Employee == null)
            {
                Employee = new()
                {
                    FullName = Employee_FullName.Text,
                    UserName = Employee_UserName.Text,
                    Password = Employee_Password.Text,
                    PositionId = ((Position)Employee_Position.SelectedItem).Id,
                    Photo = String,
                    IsAvailable = Employee_IsAvailable.IsChecked
                };
                if (PUT.Employee(Employee))
                    DialogResult = true;
            }
            else
            {
                Employee.FullName = Employee_FullName.Text;
                Employee.UserName = Employee_UserName.Text;
                Employee.Password = Employee_Password.Text;
                Employee.PositionId = ((Position)Employee_Position.SelectedItem).Id;
                Employee.Photo = String;
                Employee.IsAvailable = Employee_IsAvailable.IsChecked;
                if (PULL.Employee(Employee))
                    DialogResult = true;
            }
        }
        catch { }
    }

    private void SetPhoto()
    {
        try
        {
            if (Bytes != null)
            {
                Image = new();
                Image.BeginInit();
                Image.StreamSource = new MemoryStream(Bytes);
                Image.EndInit();
                Employee_Photo.Content = new Ellipse()
                {
                    Fill = new ImageBrush { ImageSource = Image },
                    Width = 96,
                    Height = 96
                };
                String = Convert.ToBase64String(Bytes);
            }
        }
        catch { }
    }
}