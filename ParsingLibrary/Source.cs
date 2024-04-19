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

    private static Dictionary<string, string> Replacements { get; } = new() { { "«", "\"" }, { "»", "\"" }, { "&nbsp;", " " }, { "&#8381;", "Российский рубль" }, { "₽", "Российский рубль" }, { "&#034;", "\"" }, { "( ", "(" }, { " )", "" } };

    private void Initialize()
    {
        Number = new GetNumber().Result;

        string lawNumber = new GetLawNumber().Result;
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

        Object = new GetObject().Result;

        InitialPrice = Convert.ToDecimal(new GetInitialPrice().Result);

        string organizationName = new GetOrganizationName().Result;
        string organizationPostalAddress = new GetOrganizationPostalAddress().Result;
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

        string methodText = new GetMethodText().Result;
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

        PostingDate = Convert.ToDateTime(new GetPostingDate().Result);

        string platformName = new GetPlatformName().Result;
        string platformAddress = new GetPlatformAddress().Result;
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

        Location = new GetLocation().Result;

        StartDate = Convert.ToDateTime(new GetStartDate().Result);

        Deadline = Convert.ToDateTime(new GetDeadline().Result);

        string timeZoneOffset = new GetTimeZoneOffset().Result;
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

        Securing = new GetSecuring().Result;

        Enforcement = new GetEnforcement().Result;

        Warranty = new GetWarranty().Result;

        List<Region>? regions = GET.View.Regions();
        if (regions != null)
        {
            foreach (Region region in regions)
            {
                if (Location.ToLower().Contains(region.Title.ToLower()))
                {
                    RegionId = region.Id;
                }
            }
        }
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

        public string Result { get; set; } = null!;
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

        public string Result { get; set; } = null!;
    }

    private class GetObject
    {
        public GetObject()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("registry-entry__body-block"));
                Result = elements.Where(x => x.Text.Contains("Объект закупки\r\n")).Single().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("cardMainInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Объект закупки\r\n")).Single().Text;
            }
            catch { }

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

        public string Result { get; set; } = null!;
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

        public string Result { get; set; } = null!;
    }

    private class GetOrganizationName
    {
        public GetOrganizationName()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("registry-entry__body-block"));
                Result = elements.Where(x => x.Text.Contains("Заказчик") || x.Text.Contains("Организация, осуществляющая размещение\r\n")).Single().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("cardMainInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Заказчик") || x.Text.Contains("Организация, осуществляющая размещение\r\n")).Single().Text;
            }
            catch { }

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

        public string Result { get; set; } = null!;
    }

    private class GetMethodText
    {
        public GetMethodText()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Способ определения поставщика (подрядчика, исполнителя)") || x.Text.Contains("Способ осуществления закупки\r\n")).Single().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Способ определения поставщика (подрядчика, исполнителя)") || x.Text.Contains("Способ осуществления закупки\r\n")).Single().Text;
            }
            catch { }

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

        public string Result { get; set; } = null!;
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
                Result = elements.Where(x => x.Text.Contains("Размещено\r\n")).Single().Text;
            }
            catch { }

            foreach (KeyValuePair<string, string> replacement in Replacements)
            {
                Result = Result.Replace(replacement.Key, replacement.Value);
            }

            while (Result.Contains("  "))
            {
                Result = Result.Replace("  ", " ");
            }
            if (Result.Contains("\n"))
            {
                Result = Result.Split("\n")[1];
            }
            Result = Result.Trim();
        }

        public string Result { get; set; } = null!;
    }

    private class GetPlatformName
    {
        public GetPlatformName()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Наименование электронной площадки в информационно-телекоммуникационной сети «Интернет»\r\n")).Single().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Наименование электронной площадки в информационно-телекоммуникационной сети «Интернет»\r\n")).Single().Text;
            }
            catch { }

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

        public string Result { get; set; } = null!;
    }

    private class GetPlatformAddress
    {
        public GetPlatformAddress()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Адрес электронной площадки в информационно-телекоммуникационной сети «Интернет»\r\n")).Single().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Адрес электронной площадки в информационно-телекоммуникационной сети «Интернет»\r\n")).Single().Text;
            }
            catch { }

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

        public string Result { get; set; } = null!;
    }

    private class GetOrganizationPostalAddress
    {
        public GetOrganizationPostalAddress()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Почтовый адрес\r\n")).Single().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Почтовый адрес\r\n")).Single().Text;
            }
            catch { }

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

        public string Result { get; set; } = null!;
    }

    private class GetLocation
    {
        public GetLocation()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Место нахождения\r\n")).Single().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Место нахождения\r\n")).Single().Text;
            }
            catch { }

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

        public string Result { get; set; } = null!;
    }

    private class GetStartDate
    {
        public GetStartDate()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Дата начала срока подачи заявок") || x.Text.Contains("Дата и время начала срока подачи заявок\r\n")).Single().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Дата начала срока подачи заявок") || x.Text.Contains("Дата и время начала срока подачи заявок\r\n")).Single().Text;
            }
            catch { }

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

        public string Result { get; set; } = null!;
    }

    private class GetDeadline
    {
        public GetDeadline()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Дата и время окончания срока подачи заявок (по местному времени заказчика)") || x.Text.Contains("Дата и время окончания срока подачи заявок\r\n")).Single().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Дата и время окончания срока подачи заявок (по местному времени заказчика)") || x.Text.Contains("Дата и время окончания срока подачи заявок\r\n")).Single().Text;
            }
            catch { }

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

        public string Result { get; set; } = null!;
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

            while (Result.Contains("  "))
            {
                Result = Result.Replace("  ", " ");
            }
            Result = Result.Split(" ")[1].Trim();
        }

        public string Result { get; set; } = null!;
    }

    private class GetSecuring
    {
        public GetSecuring()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Размер обеспечения заявки\r\n")).Single().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Размер обеспечения заявки\r\n")).Single().Text;
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

        public string Result { get; set; } = null!;
    }

    private class GetEnforcement
    {
        public GetEnforcement()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Размер обеспечения исполнения контракта\r\n")).Single().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Размер обеспечения исполнения контракта\r\n")).Single().Text;
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

        public string Result { get; set; } = null!;
    }

    private class GetWarranty
    {
        public GetWarranty()
        {
            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("col-9"));
                Result = elements.Where(x => x.Text.Contains("Размер обеспечения гарантийных обязательств\r\n")).Single().Text;
            }
            catch { }

            try
            {
                ReadOnlyCollection<IWebElement> elements = Driver.FindElements(By.ClassName("blockInfo__section"));
                Result = elements.Where(x => x.Text.Contains("Размер обеспечения гарантийных обязательств\r\n")).Single().Text;
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

        public string Result { get; set; } = null!;
    }
}