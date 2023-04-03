using System.Windows;

namespace ParsingLibrary;

public class Sources : List<Source>
{
    private static EdgeDriver Driver { get; set; } = null!;
    private static IWebElement Element { get; set; } = null!;
    private string Input { get; set; } = string.Empty;
    private static RegexOptions RegexOptions { get; } = RegexOptions.Compiled | RegexOptions.Singleline;
    private Regex UrlRegex { get; set; } = new(@"pageNumber=\d*", RegexOptions);
    private Regex Regex { get; set; } = new(@" *<div class=""search-registry-entry-block box-shadow-search-input"">(?<val>.*?)\n        </div>", RegexOptions);

    public void Enable()
    {
        InitializeDriver();
        using ParsethingContext db = new();
        foreach (Tag tag in db.Tags)
        {
            try
            {
                string url = $"https://zakupki.gov.ru/epz/order/extendedsearch/results.html?searchString={tag.Keyword}&morphology=on&search-filter=%D0%94%D0%B0%D1%82%D0%B5+%D1%80%D0%B0%D0%B7%D0%BC%D0%B5%D1%89%D0%B5%D0%BD%D0%B8%D1%8F&pageNumber=1&sortDirection=false&recordsPerPage=50&showLotsInfoHidden=false&sortBy=UPDATE_DATE&fz44=on&fz223=on&af=on&currencyIdGeneral=-1";
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
                                elements[j].Click();
                                Thread.Sleep(2000);
                                Add(new(procurementCards[j].Value));
                                ReadOnlyCollection<string> tabs = Driver.WindowHandles;
                                if (tabs.Count > 1)
                                {
                                    _ = Driver.SwitchTo().Window(tabs[1]);
                                    Driver.Close();
                                    _ = Driver.SwitchTo().Window(tabs[0]);
                                }
                                Thread.Sleep(2000);
                            }
                        }
                        try
                        {
                            Element = Driver.FindElement(By.ClassName("paginator-button"));
                            Element.Click();
                            Thread.Sleep(5000);
                        }
                        catch { }
                    }
                    catch { }
                }
            }
            catch { }
        }
    }

    private static void InitializeDriver()
    {
        try
        {
            EdgeDriverService driverService = EdgeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            Driver = new EdgeDriver(driverService, new EdgeOptions());
        }
        catch { }
    }

    public static void Disable()
    {
        try
        {
            Driver.Quit();
        }
        catch { }
    }
}