using System.Diagnostics;
using System.Windows;

namespace ParsingLibrary;

public class Sources
{
    private EdgeDriver Driver { get; set; } = null!;
    private IWebElement Element { get; set; } = null!;

    private bool IsWorking { get; set; } = true;

    public void Enable(string minPrice, string maxPrice, List<Region> regions)
    {
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

        while (IsWorking)
        {
            List<Tag>? tags = GET.View.Tags();
            List<TagException>? tagExceptions = GET.View.TagExceptions();

            if (tags != null)
            {
                foreach (Tag tag in tags)
                {
                    try
                    {
                        string url = $"https://zakupki.gov.ru/epz/order/extendedsearch/results.html?searchString={tag.Keyword}&morphology=on&sortBy=UPDATE_DATE&pageNumber=1&sortDirection=false&recordsPerPage=_50&showLotsInfoHidden=false&fz44=on&fz223=on&af=on&priceContractAdvantages44IdNameHidden=%7B%7D&priceContractAdvantages94IdNameHidden=%7B%7D&priceFromGeneral={minPrice}&priceToGeneral={maxPrice}&currencyIdGeneral=-1&publishDateFrom={DateTime.Now.AddDays(-1).ToShortDateString()}&customerPlace={regionsString}&customerPlaceCodes=%2C&selectedSubjectsIdNameHidden=%7B%7D&okdpGroupIdsIdNameHidden=%7B%7D&koksIdsIdNameHidden=%7B%7D&OrderPlacementSmallBusinessSubject=on&OrderPlacementRnpData=on&OrderPlacementExecutionRequirement=on&orderPlacement94_0=0&orderPlacement94_1=0&orderPlacement94_2=0&contractPriceCurrencyId=-1&budgetLevelIdNameHidden=%7B%7D&nonBudgetTypesIdNameHidden=%7B%7D&gws=%D0%92%D1%8B%D0%B1%D0%B5%D1%80%D0%B8%D1%82%D0%B5+%D1%82%D0%B8%D0%BF+%D0%B7%D0%B0%D0%BA%D1%83%D0%BF%D0%BA%D0%B8";
                        Driver.Navigate().GoToUrl(url);
                        Thread.Sleep(3000);

                        for (int i = 1; i <= 20; i++)
                        {
                            try
                            {
                                int counter = 1;
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
                                    catch
                                    {
                                        counter += 1;
                                    }
                                    if (counter == 3)
                                    {
                                        break;
                                    }
                                }
                                while (true);

                                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("registry-entry__header-mid__number"));
                                for (int j = 0; j < elements.Count; j++)
                                {
                                    ReadOnlyCollection<string> tabs;
                                    try
                                    {
                                        try
                                        {
                                            elements[j].Click();
                                            Thread.Sleep(3000);
                                        }
                                        catch { }
                                        tabs = Driver.WindowHandles;

                                        try
                                        {
                                            _ = Driver.SwitchTo().Window(tabs[1]);
                                        }
                                        catch { }

                                        Source source = new(Driver);
                                        foreach (TagException tagException in tagExceptions ?? new List<TagException>() { new() { Keyword = "" } })
                                        {
                                            if (!source.Object.ToLower().Contains(tagException.Keyword.ToLower()))
                                            {
                                                int tryCounter = 0;
                                                do
                                                {
                                                    Thread.Sleep(1000);
                                                    tryCounter++;
                                                }
                                                while (PUT.ProcurementSource(source) || tryCounter < 5);
                                            }
                                        }
                                    }
                                    catch { }

                                    tabs = Driver.WindowHandles;
                                    Driver.Close();
                                    _ = Driver.SwitchTo().Window(tabs[0]);
                                    Thread.Sleep(3000);
                                }

                                try
                                {
                                    Element = Driver.FindElement(By.ClassName("paginator-button-next"));
                                    Element.Click();
                                    Thread.Sleep(3000);
                                }
                                catch
                                {
                                    break;
                                }
                            }
                            catch { }
                        }
                    }
                    catch
                    {
                        Disable();
                        IsWorking = false;
                        break;
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
            EdgeOptions edgeOptions = new();
            //edgeOptions.AddArgument("--headless=new");
            Driver = new EdgeDriver(driverService, edgeOptions);
        }
        catch
        {
            _ = MessageBox.Show("Ошибка!");

        }
    }

    public void Disable()
    {
        try
        {
            Driver.Close();
            Thread.Sleep(5000);
            Driver.Quit();
            Thread.Sleep(5000);
            foreach (Process process in Process.GetProcessesByName("msedgedriver"))
            {
                process.Kill();
                Thread.Sleep(5000);
            }
        }
        catch { }
    }
}