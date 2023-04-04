namespace ParsingLibrary;

public class Sources
{
    private EdgeDriver Driver { get; set; } = null!;
    private IWebElement Element { get; set; } = null!;
    private string Input { get; set; } = string.Empty;
    private static RegexOptions RegexOptions { get; } = RegexOptions.Compiled | RegexOptions.Singleline;
    private Regex UrlRegex { get; set; } = new(@"pageNumber=\d*", RegexOptions);
    private Regex Regex { get; set; } = new(@" *<div class=""search-registry-entry-block box-shadow-search-input"">(?<val>.*?)\n        </div>", RegexOptions);

    public void Enable()
    {
        try
        {
            InitializeDriver();
            using ParsethingContext db = new();
            while (true)
            {
                foreach (Tag tag in db.Tags)
                {
                    try
                    {
                        string url = $"https://zakupki.gov.ru/epz/order/extendedsearch/results.html?searchString={tag.Keyword}&morphology=on&search-filter=Дате+размещения&pageNumber=1&sortDirection=false&recordsPerPage=_10&showLotsInfoHidden=false&sortBy=UPDATE_DATE&fz44=on&fz223=on&af=on&priceFromGeneral=200000&priceToGeneral=9000000&currencyIdGeneral=-1&publishDateFrom={DateTime.Now.ToShortDateString()}";
                        Driver.Navigate().GoToUrl(url);
                        Thread.Sleep(10000);

                        for (int i = 1; i <= 20; i++)
                        {
                            try
                            {
                                GetRequest request = new(UrlRegex.Replace(url, $"pageNumber={i}"));
                                Input = request.Input;

                                try
                                {
                                    Element = Driver.FindElement(By.ClassName("btn-close"));
                                    Element.Click();
                                    Thread.Sleep(5000);
                                }
                                catch { }

                                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("registry-entry__header-mid__number"));

                                if (Input != null)
                                {
                                    MatchCollection procurementCards = Regex.Matches(Input);
                                    for (int j = 0; j < procurementCards.Count; j++)
                                    {
                                        try
                                        {
                                            elements[j].Click();
                                            Thread.Sleep(5000);
                                            Source source = new(procurementCards[j].Value);
                                            _ = PUT.Procurement(source, source.IsGetted);
                                            ReadOnlyCollection<string> tabs = Driver.WindowHandles;
                                            if (tabs.Count > 1)
                                            {
                                                _ = Driver.SwitchTo().Window(tabs[1]);
                                                Driver.Close();
                                                _ = Driver.SwitchTo().Window(tabs[0]);
                                            }
                                            Thread.Sleep(5000);
                                        }
                                        catch { }
                                    }
                                }

                                try
                                {
                                    Element = Driver.FindElement(By.ClassName("paginator-button"));
                                    Element.Click();
                                    Thread.Sleep(5000);
                                }
                                catch
                                {
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                _ = MessageBox.Show(ex.Message, ex.Source);
                                Disable();
                            }
                        }
                    }
                    catch { }
                }
            }
        }
        catch { }
    }

    private void InitializeDriver()
    {
        try
        {
            EdgeDriverService driverService = EdgeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            Driver = new EdgeDriver(driverService, new EdgeOptions());
        }
        catch { }
    }

    public void Disable()
    {
        try
        {
            Driver.Quit();
        }
        catch { }
    }
}