using System.Diagnostics;
using System.Windows;

namespace ParsingLibrary;

public class Sources
{
    private EdgeDriver Driver { get; set; } = null!;
    private IWebElement Element { get; set; } = null!;
    private string Input { get; set; } = string.Empty;
    private static RegexOptions RegexOptions { get; } = RegexOptions.Compiled | RegexOptions.Singleline;
    private Regex UrlRegex { get; set; } = new(@"pageNumber=\d*", RegexOptions);
    private Regex Regex { get; set; } = new(@" *<div class=""search-registry-entry-block box-shadow-search-input"">(?<val>.*?)\n        </div>", RegexOptions);

    public void Enable(string minPrice, string maxPrice, List<Region> regions)
    {
        try
        {
            string[] cache = File.ReadAllLines("Cache.txt");
            if (Convert.ToDateTime(cache[0]).ToShortDateString() != DateTime.Now.ToShortDateString())
            {
                throw new Exception();
            }
        }
        catch
        {
            File.Create("Cache.txt").Close();
            File.AppendAllText("Cache.txt", $"{DateTime.Now.AddDays(-1).ToShortDateString()}\n");
        }

        InitializeDriver();

        string regionsString = "";
        if (regions.Count > 0)
        {
            regionsString += regions[0].RegionCode;
            for (int i = 1; i < regions.Count; i++)
            {
                regionsString += $"%2C{regions[i].RegionCode}";
            }
        }

        while (true)
        {
            List<Tag>? tags = GET.View.Tags();

            if (tags != null)
            {
                foreach (Tag tag in tags)
                {
                    try
                    {
                        string url = $"https://zakupki.gov.ru/epz/order/extendedsearch/results.html?searchString={tag.Keyword}&morphology=on&sortBy=UPDATE_DATE&pageNumber=1&sortDirection=false&recordsPerPage=_50&showLotsInfoHidden=false&fz44=on&fz223=on&af=on&priceContractAdvantages44IdNameHidden=%7B%7D&priceContractAdvantages94IdNameHidden=%7B%7D&priceFromGeneral={minPrice}&priceToGeneral={maxPrice}&currencyIdGeneral=-1&publishDateFrom={DateTime.Now.AddDays(-1).ToShortDateString()}&customerPlace={regionsString}&customerPlaceCodes=%2C&selectedSubjectsIdNameHidden=%7B%7D&okdpGroupIdsIdNameHidden=%7B%7D&koksIdsIdNameHidden=%7B%7D&OrderPlacementSmallBusinessSubject=on&OrderPlacementRnpData=on&OrderPlacementExecutionRequirement=on&orderPlacement94_0=0&orderPlacement94_1=0&orderPlacement94_2=0&contractPriceCurrencyId=-1&budgetLevelIdNameHidden=%7B%7D&nonBudgetTypesIdNameHidden=%7B%7D&gws=%D0%92%D1%8B%D0%B1%D0%B5%D1%80%D0%B8%D1%82%D0%B5+%D1%82%D0%B8%D0%BF+%D0%B7%D0%B0%D0%BA%D1%83%D0%BF%D0%BA%D0%B8";
                        Driver.Navigate().GoToUrl(url);
                        Thread.Sleep(10000);

                        for (int i = 1; i <= 20; i++)
                        {
                            try
                            {
                                Input = Driver.PageSource;

                                do
                                {
                                    DateTime dateTime = DateTime.Now;
                                    try
                                    {
                                        Element = Driver.FindElement(By.ClassName("btn-close"));
                                        Element.Click();
                                        Thread.Sleep(1000);
                                        break;
                                    }
                                    catch { }
                                    if (DateTime.Now -  dateTime > TimeSpan.FromSeconds(10))
                                    {
                                        break;
                                    }
                                }
                                while (true);

                                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("registry-entry__header-mid__number"));
                                MatchCollection procurementCards = Regex.Matches(Input);
                                for (int j = 0; j < procurementCards.Count; j++)
                                {

                                    try
                                    {
                                        elements[j].Click();
                                        Thread.Sleep(5000);

                                        ReadOnlyCollection<string> tabs = Driver.WindowHandles;
                                        _ = Driver.SwitchTo().Window(tabs[1]);

                                        Source source = new(Driver.PageSource);

                                        Driver.Close();
                                        _ = Driver.SwitchTo().Window(tabs[0]);
                                        Thread.Sleep(5000);
                                    }
                                    catch { }
                                }

                                try
                                {
                                    Element = Driver.FindElement(By.ClassName("paginator-button"));
                                    Element.Click();
                                    Thread.Sleep(5000);
                                }
                                catch { break; }
                            }
                            catch { }
                        }
                    }
                    catch (Exception ex)
                    {
                        Disable();
                        LogWriter.Write(ex);
                    }
                }
            }
        }
    }

    private void InitializeDriver()
    {
        try
        {
            EdgeDriverService driverService = EdgeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            Driver = new EdgeDriver(driverService, new EdgeOptions());
        }
        catch (Exception ex)
        {
            _ = MessageBox.Show("Ошибка!");
            LogWriter.Write(ex);
        }
    }

    public void Disable()
    {
        try
        {
            foreach (Process process in Process.GetProcessesByName("msedgedriver"))
            {
                process.Kill();
                Thread.Sleep(5000);
            }
        }
        catch (Exception ex) { LogWriter.Write(ex); }
    }
}