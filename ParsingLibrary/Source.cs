namespace ParsingLibrary;

public class Source : Procurement
{
    public Source(EdgeDriver driver)
    {
        Driver = driver;
        RequestUri = driver.Url;
        Initialize();
    }

    private static EdgeDriver Driver { get; set; } = null!;

    private static Dictionary<string, string> Replacements { get; } = new() { { "«", "\"" }, { "»", "\"" }, { "&nbsp;", " " }, { "&#8381;", "Российский рубль" }, { "₽", "Российский рубль" }, { "&#034;", "\"" }, { "( ", "(" }, { " )", "" }, { "–", "—" } };

    private void Initialize()
    {
        string? number = new GetNumber().Result;
        if (number != null)
        {
            Number = number;
        }

        string? lawNumber = new GetLawNumber().Result;
        if (lawNumber != null)
        {
            Law? law = GET.Entry.Law(lawNumber);
            if (law != null)
            {
                LawId = law.Id;
            }
            else
            {
                _ = PUT.Law(new()
                {
                    Number = lawNumber
                });
                law = GET.Entry.Law(lawNumber);
                LawId = law != null ? law.Id : 0;
            }
        }

        string? obj = new GetObject().Result;
        if (obj != null)
        {
            Object = obj;
        }

        InitialPrice = Convert.ToDecimal(new GetInitialPrice().Result);

        string? organizationName = new GetOrganizationName().Result;
        string? organizationPostalAddress = new GetOrganizationPostalAddress().Result;
        if (organizationName != null && organizationPostalAddress != null)
        {
            Organization? organization = GET.Entry.Organization(organizationName, organizationPostalAddress);
            if (organization != null)
            {
                OrganizationId = organization.Id;
            }
            else
            {
                _ = PUT.Organization(new()
                {
                    Name = organizationName,
                    PostalAddress = organizationPostalAddress
                });
                organization = GET.Entry.Organization(organizationName, organizationPostalAddress);
                OrganizationId = organization != null ? organization.Id : 0;
            }
        }

        string? methodText = new GetMethodText().Result;
        if (methodText != null)
        {
            Method? method = GET.Entry.Method(methodText);
            if (method != null)
            {
                MethodId = method.Id;
            }
            else
            {
                _ = PUT.Method(new()
                {
                    Text = methodText
                });
                method = GET.Entry.Method(methodText);
                MethodId = method != null ? method.Id : 0;
            }
        }

        PostingDate = Convert.ToDateTime(new GetPostingDate().Result);

        string? platformName = new GetPlatformName().Result;
        string? platformAddress = new GetPlatformAddress().Result;
        if (platformName != null && platformAddress != null)
        {
            Platform? platform = GET.Entry.Platform(platformName, platformAddress);
            if (platform != null)
            {
                PlatformId = platform.Id;
            }
            else
            {
                _ = PUT.Platform(new()
                {
                    Name = platformName,
                    Address = platformAddress
                });
                platform = GET.Entry.Platform(platformName, platformAddress);
                PlatformId = platform != null ? platform.Id : 0;
            }
        }

        Location = new GetLocation().Result;

        string? startDate = new GetStartDate().Result;
        StartDate = startDate != null ? Convert.ToDateTime(startDate) : StartDate = DateTime.Now;

        Deadline = Convert.ToDateTime(new GetDeadline().Result);

        string? timeZoneOffset = new GetTimeZoneOffset().Result;
        if (timeZoneOffset != null)
        {
            TimeZone? timeZone = GET.Entry.TimeZone(timeZoneOffset);
            if (timeZone != null)
            {
                TimeZoneId = timeZone.Id;
            }
            else
            {
                _ = PUT.TimeZone(new()
                {
                    Offset = timeZoneOffset
                });
                timeZone = GET.Entry.TimeZone(timeZoneOffset);
                TimeZoneId = timeZone != null ? timeZone.Id : 0;
            }
        }

        Securing = new GetSecuring().Result;

        Enforcement = new GetEnforcement().Result;

        Warranty = new GetWarranty().Result;

        if (Location != null)
        {
            List<Region>? regions = GET.View.Regions();
            if (regions != null)
            {
                foreach (Region region in regions)
                {
                    if (Location.Contains(region.Title, StringComparison.CurrentCultureIgnoreCase))
                    {
                        RegionId = region.Id;
                    }
                }
            }
        }

        string? resultDate = new GetResultDate().Result;
        ResultDate = resultDate != null ? Convert.ToDateTime(resultDate) : null;
    }

    private class GetNumber
    {
        public GetNumber()
        {
            try
            {
                IWebElement element = Driver.FindElement(By.ClassName("registry-entry__header-mid__number"));
                Result = element.Text;
            }
            catch { }

            try
            {
                IWebElement element = Driver.FindElement(By.ClassName("cardMainInfo__purchaseLink"));
                Result = element.Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Split(" ")[1].Trim();
            }
        }

        public string? Result { get; set; }
    }

    private class GetLawNumber
    {
        public GetLawNumber()
        {
            try
            {
                IWebElement element = Driver.FindElement(By.ClassName("registry-entry__header-top__title"));
                Result = element.Text;
            }
            catch { }

            try
            {
                IWebElement element = Driver.FindElement(By.ClassName("cardMainInfo__title"));
                Result = element.Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Split(" ")[0].Split("\n")[0].Trim();
            }
        }

        public string? Result { get; set; }
    }

    private class GetObject
    {
        public GetObject()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("registry-entry__body-block"));
                Result = elements.Where(x => x.Text.Contains("Объект закупки\r\n")).First().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("cardMainInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Объект закупки\r\n")).First().Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Split("\n")[1].Trim();
            }
        }

        public string? Result { get; set; }
    }

    private class GetInitialPrice
    {
        public GetInitialPrice()
        {
            try
            {
                IWebElement element = Driver.FindElement(By.ClassName("price-block__value"));
                Result = element.Text;
            }
            catch { }

            try
            {
                IWebElement element = Driver.FindElement(By.ClassName("cost"));
                Result = element.Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Replace("Российский рубль", "").Trim();
            }
        }

        public string? Result { get; set; }
    }

    private class GetOrganizationName
    {
        public GetOrganizationName()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("registry-entry__body-block"));
                Result = elements.Where(x => x.Text.Contains("Заказчик") || x.Text.Contains("Организация, осуществляющая размещение\r\n")).First().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("cardMainInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Заказчик") || x.Text.Contains("Организация, осуществляющая размещение\r\n")).First().Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Split("\n")[1].Trim();
            }
        }

        public string? Result { get; set; }
    }

    private class GetMethodText
    {
        public GetMethodText()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Способ определения поставщика (подрядчика, исполнителя)") || x.Text.Contains("Способ осуществления закупки\r\n")).First().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Способ определения поставщика (подрядчика, исполнителя)") || x.Text.Contains("Способ осуществления закупки\r\n")).First().Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Split("\n")[1].Trim();
            }
        }

        public string? Result { get; set; }
    }

    private class GetPostingDate
    {
        public GetPostingDate()
        {
            try
            {
                IWebElement element = Driver.FindElement(By.ClassName("data-block__value"));
                Result = element.Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("cardMainInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Размещено\r\n")).First().Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                if (Result.Contains('\n'))
                {
                    Result = Result.Split("\n")[1];
                }
                Result = Result.Trim();
            }
        }

        public string? Result { get; set; }
    }

    private class GetPlatformName
    {
        public GetPlatformName()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Наименование электронной площадки в информационно-телекоммуникационной сети «Интернет»\r\n")).First().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Наименование электронной площадки в информационно-телекоммуникационной сети «Интернет»\r\n")).First().Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Split("\n")[1].Trim();
            }
        }

        public string? Result { get; set; }
    }

    private class GetPlatformAddress
    {
        public GetPlatformAddress()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Адрес электронной площадки в информационно-телекоммуникационной сети «Интернет»\r\n")).First().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Адрес электронной площадки в информационно-телекоммуникационной сети «Интернет»\r\n")).First().Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Split("\n")[1].Trim();
                if (!Result.Contains("http"))
                {
                    Result = "https://" + Result;
                }
                else if (!Result.Contains("https"))
                {
                    Result = Result.Replace("http", "https");
                }
            }
        }

        public string? Result { get; set; }
    }

    private class GetOrganizationPostalAddress
    {
        public GetOrganizationPostalAddress()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Почтовый адрес\r\n")).First().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Почтовый адрес\r\n")).First().Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Split("\n")[1].Trim();
            }
        }

        public string? Result { get; set; }
    }

    private class GetLocation
    {
        public GetLocation()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Место нахождения\r\n")).First().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Место нахождения\r\n")).First().Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Split("\n")[1].Trim();
            }
        }

        public string? Result { get; set; }
    }

    private class GetStartDate
    {
        public GetStartDate()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Дата начала срока подачи заявок") || x.Text.Contains("Дата и время начала срока подачи заявок\r\n") || x.Text.Contains("Дата проведения процедуры подачи предложений о цене контракта либо о сумме цен единиц товара, работы, услуги")).First().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Дата начала срока подачи заявок") || x.Text.Contains("Дата и время начала срока подачи заявок\r\n") || x.Text.Contains("Дата проведения процедуры подачи предложений о цене контракта либо о сумме цен единиц товара, работы, услуги")).First().Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Split("\n")[1].Split("(")[0].Trim();
            }
        }

        public string? Result { get; set; }
    }

    private class GetDeadline
    {
        public GetDeadline()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Дата и время окончания срока подачи заявок (по местному времени заказчика)") || x.Text.Contains("Дата и время окончания срока подачи заявок\r\n")).First().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Дата и время окончания срока подачи заявок (по местному времени заказчика)") || x.Text.Contains("Дата и время окончания срока подачи заявок\r\n")).First().Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Split("\n")[1].Split("(")[0].Trim();
            }
        }

        public string? Result { get; set; }
    }

    private class GetTimeZoneOffset
    {
        public GetTimeZoneOffset()
        {
            IWebElement element = Driver.FindElement(By.ClassName("time-zone__value"));
            Result = element.Text;

            foreach (KeyValuePair<string, string> replacement in Replacements)
            {
                Result = Result.Replace(replacement.Key, replacement.Value);
            }

            if (Result != null)
            {
                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Regex.Match(Result, @"МСК(\+||-)*\d*").Value;
            }
        }

        public string? Result { get; set; }
    }

    private class GetSecuring
    {
        public GetSecuring()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Размер обеспечения заявки\r\n")).First().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Размер обеспечения заявки\r\n")).First().Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Split("\n")[1].Trim();
            }
        }

        public string? Result { get; set; }
    }

    private class GetEnforcement
    {
        public GetEnforcement()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Размер обеспечения исполнения контракта\r\n")).First().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Размер обеспечения исполнения контракта\r\n")).First().Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Split("\n")[1].Trim();
            }
        }

        public string? Result { get; set; }
    }

    private class GetWarranty
    {
        public GetWarranty()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Размер обеспечения гарантийных обязательств\r\n")).First().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Размер обеспечения гарантийных обязательств\r\n")).First().Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Split("\n")[1].Trim();
            }
        }

        public string? Result { get; set; }
    }

    private class GetResultDate
    {
        public GetResultDate()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Дата подведения итогов определения поставщика (подрядчика, исполнителя)\r\n") || x.Text.Contains("Дата подведения итогов\r\n")).First().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Дата подведения итогов определения поставщика (подрядчика, исполнителя)\r\n") || x.Text.Contains("Дата подведения итогов\r\n")).First().Text;
            }
            catch { }

            if (Result != null)
            {
                foreach (KeyValuePair<string, string> replacement in Replacements)
                {
                    Result = Result.Replace(replacement.Key, replacement.Value);
                }

                while (Result.Contains("  "))
                {
                    Result = Result.Replace("  ", " ");
                }
                Result = Result.Split("\n")[1].Trim();
            }
        }

        public string? Result { get; set; }
    }
}