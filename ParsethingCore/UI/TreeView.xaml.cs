namespace ParsethingCore.UI;

public partial class TreeView : UserControl
{
    public TreeView() =>
        InitializeComponent();

    private Border? DataGridContainer { get; set; }

    private void View_Loaded(object sender, RoutedEventArgs e) =>
        DataGridContainer = (Border)Application.Current.MainWindow.FindName("DataGridContainer");

    private void TreeViewItem_GotFocus(object sender, RoutedEventArgs e)
    {
        if (DataGridContainer != null)
            switch (((TreeViewItem)sender).Header.ToString())
            {
                case "Справочник комплектующих":
                    DataGridContainer.Child = new ComponentsList();
                    break;
                case "Справочник сотрудников":
                    DataGridContainer.Child = new EmployeesList();
                    break;
                case "Справочник закупок":
                    DataGridContainer.Child = new ProcurementsList();
                    break;
                case "Справочник регионов":
                    DataGridContainer.Child = new RegionsList();
                    break;
                case "Состояния комплектующих":
                    DataGridContainer.Child = new ComponentStatesList();
                    break;
                case "Типы комплектующих":
                    DataGridContainer.Child = new ComponentTypesList();
                    break;
                case "Производители":
                    DataGridContainer.Child = new ManufacturersList();
                    break;
                case "Поставщики":
                    DataGridContainer.Child = new SellersList();
                    break;
                case "Документы":
                    DataGridContainer.Child = new DocumentsList();
                    break;
                case "Юридические лица":
                    DataGridContainer.Child = new LegalEntitiesList();
                    break;
                case "Миноптторги":
                    DataGridContainer.Child = new MinopttorgsList();
                    break;
                case "Преференции":
                    DataGridContainer.Child = new PreferencesList();
                    break;
                case "Состояния закупок":
                    DataGridContainer.Child = new ProcurementStatesList();
                    break;
                case "Тэги":
                    DataGridContainer.Child = new TagsList();
                    break;
                case "Должности":
                    DataGridContainer.Child = new PositionsList();
                    break;
            }
    }
}