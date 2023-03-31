namespace ParsethingCore
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Sources Sources { get; set; } = null!;
        private Thread SourcesCaller { get; set; } = null!;

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            Sources = new();
            SourcesCaller = new(Sources.Enable);
            SourcesCaller.Start();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            SourcesCaller = new(Sources.Disable);
            SourcesCaller.Start();
            _ = MessageBox.Show(Sources.Count.ToString());
            foreach (Source source in Sources)
            {
                try
                {
                    ParsethingContext db = new();
                    Procurement? def = null;
                    try
                    {
                        def = db.Procurements.Where(p => p.Number == source.Number).First();
                    }
                    catch { }
                    if (def == null)
                    {
                        _ = db.Procurements.Add(source);
                    }
                    else
                    {
                        def.LawId = source.LawId;
                        def.Object = source.Object;
                        def.InitialPrice = source.InitialPrice;
                        def.OrganizationId = source.OrganizationId;
                        if (source.IsGetted)
                        {
                            def.MethodId = source.MethodId;
                            def.PlatformId = source.PlatformId;
                            def.Location = source.Location;
                            def.StartDate = source.StartDate;
                            def.Deadline = source.Deadline;
                            def.TimeZoneId = source.TimeZoneId;
                            def.Securing = source.Securing;
                            def.Enforcement = source.Enforcement;
                            def.Warranty = source.Warranty;
                        }
                        _ = db.Procurements.Update(def);
                    }
                    _ = db.SaveChanges();
                }
                catch { }
            }
        }
    }
}