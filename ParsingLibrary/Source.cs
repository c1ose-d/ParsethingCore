namespace ParsingLibrary;

public class Source : Procurement
{
    public Source(string procurementCard, string requestUri)
    {
        ProcurementCard = procurementCard;
        RequestUri = requestUri;
        Initialize();
    }

    private static string ProcurementCard { get; set; } = null!;
    public bool IsGetted { get; set; } = false;
    public bool IsCached { get; set; } = false;

    private void Initialize()
    {
        File.Create("test.txt").Close();

        string[] cache = File.ReadAllLines("Cache.txt");
        foreach (string s in cache)
        {
            if (s == RequestUri)
            {
                IsCached = true;
                break;
            }
        }

        if (!IsCached)
        {
            string? number = new GetNumber().Result;
            if (number != null)
            {
                Number ??= number;
            }

            string? lawNumber = new GetLawNumber().Result?.Replace(">", "") + "-ФЗ";
            if (lawNumber != null)
            {
                Law? law = GET.Entry.Law(lawNumber);
                if (law == null)
                {
                    _ = PUT.Law(new() { Number = lawNumber });
                    law = GET.Entry.Law(lawNumber);
                }
                if (law != null)
                {
                    LawId = law.Id;
                }
            }

            string? obj = new GetObject().Result;
            if (obj != null)
            {
                Object = obj;
            }

            InitialPrice = decimal.TryParse(new GetInitialPrice().Result?.Replace(" Российский рубль", ""), out decimal initialPrice) ? initialPrice : 0;

            string? organizationName = new GetOrganizationName().Result?.Split(">")[^1].Trim();

            string? methodText = new GetMethodText().Result?.Split(" <")[0];
            if (methodText != null)
            {
                if (methodText != "223-ФЗ" && methodText.Contains("223-ФЗ"))
                {
                    methodText = "223-ФЗ";
                }
                else if (methodText != "44-ФЗ" && methodText.Contains("44-ФЗ"))
                {
                    methodText = "44-ФЗ";
                }

                Method? method = GET.Entry.Method(methodText);
                if (method == null)
                {
                    _ = PUT.Method(new() { Text = methodText });
                    method = GET.Entry.Method(methodText);
                }
                if (method != null)
                {
                    MethodId = method.Id;
                }
            }

            PostingDate = DateTime.TryParse(new GetPostingDate().Result?.Split(" <")[0], out DateTime postingDate) ? postingDate : null;

            string? platformName = new GetPlatformName().Result?.Split(" <")[0]; ;
            string? platformAddress = new GetPlatformAddress().Result;
            if (platformName != null && platformAddress != null)
            {
                Platform? platform = GET.Entry.Platform(platformName, platformAddress);
                if (platform == null)
                {
                    _ = PUT.Platform(new() { Name = platformName, Address = platformAddress });
                    platform = GET.Entry.Platform(platformName, platformAddress);
                }
                if (platform != null)
                {
                    PlatformId = platform.Id;
                }
            }

            string? organizationPostalAddress = new GetOrganizationPostalAddress().Result;
            if (organizationName != null)
            {
                Organization? organization = organizationPostalAddress != null
                    ? GET.Entry.Organization(organizationName, organizationPostalAddress)
                    : GET.Entry.Organization(organizationName);
                if (organization == null)
                {
                    _ = PUT.Organization(new() { Name = organizationName, PostalAddress = organizationPostalAddress });
                    organization = GET.Entry.Organization(organizationName, organizationPostalAddress);
                }
                if (organization != null)
                {
                    OrganizationId = organization.Id;
                }
            }

            Location = new GetLocation().Result?.Split(" <")[0];

            StartDate = DateTime.TryParse(new GetStartDate().Result?.Split(" <")[0], out DateTime startDate) ? startDate : null;

            Deadline = DateTime.TryParse(new GetDeadline().Result?.Split(" <")[0], out DateTime deadline) ? deadline : null;

            string? timeZoneOffset = new GetTimeZoneOffset().Result;
            if (timeZoneOffset != null)
            {
                TimeZone? timeZone = GET.Entry.TimeZone(timeZoneOffset);
                if (timeZone == null)
                {
                    _ = PUT.TimeZone(new() { Offset = timeZoneOffset });
                    timeZone = GET.Entry.TimeZone(timeZoneOffset);
                }
                if (timeZone != null)
                {
                    TimeZoneId = timeZone.Id;
                }
            }

            Securing = new GetSecuring().Result?.Split(" <")[0];

            Enforcement = new GetEnforcement().Result?.Split(" <")[0];

            Warranty = new GetWarranty().Result?.Split(" <")[0];

            IsGetted = true;
        }
        else
        {
            File.AppendAllText("Cache.txt", $"{RequestUri}\n");
        }
    }

    private class GetNumber : Parse
    {
        public GetNumber() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@">№ (?<val>.*?)\n", RegexOptions) };
    }

    private class GetLawNumber : Parse
    {
        public GetLawNumber() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@"registry-entry__header-top__title"">(?<val>.*?)-ФЗ", RegexOptions), new(@"cardMainInfo__title d-flex text-truncate""(?<val>.*?)-ФЗ", RegexOptions) };
    }

    private class GetObject : Parse
    {
        public GetObject() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Объект закупки</(?<val>.*?)</div>", RegexOptions) };
    }

    private class GetInitialPrice : Parse
    {
        public GetInitialPrice() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@"price-block__value"">(?<val>.*?)</div>", RegexOptions), new(@"cardMainInfo__content cost"">(?<val>.*?)</span>", RegexOptions) };
    }

    private class GetOrganizationName : Parse
    {
        public GetOrganizationName() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Заказчик<br>(?<val>.*?)</a>", RegexOptions), new(@"Заказчик</(?<val>.*?)</a>", RegexOptions), new(@"Организация, осуществляющая размещение\r\n(?<val>.*?)</a>", RegexOptions) };
    }

    private class GetMethodText : Parse
    {
        public GetMethodText() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Способ определения поставщика \(подрядчика, исполнителя\)</(?<val>.*?)</div>", RegexOptions), new(@"Способ осуществления закупки</(?<val>.*?)</div>", RegexOptions) };
    }

    private class GetPostingDate : Parse
    {
        public GetPostingDate() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Размещено</(?<val>.*?)\n</div>", RegexOptions), new(@"Размещено</(?<val>.*?)\n</span>", RegexOptions) };
    }

    private class GetPlatformName : Parse
    {
        public GetPlatformName() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Наименование электронной площадки в информационно-телекоммуникационной сети «Интернет»</(?<val>.*?)</div>", RegexOptions), new(@"Наименование электронной площадки(?<space>.*?)>\n(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
    }

    private class GetPlatformAddress : Parse
    {
        public GetPlatformAddress() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Адрес электронной площадки(?<space>.*?)</span>(?<space>.*?)href=""(?<val>.*?)""", RegexOptions), new(@"Адрес электронной площадки(?<space>.*?)>\n(?<space>.*?)>\n(?<space>.*?)href=""(?<val>.*?)""", RegexOptions) };
    }

    private class GetOrganizationPostalAddress : Parse
    {
        public GetOrganizationPostalAddress() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Почтовый адрес</(?<val>.*?)</div>", RegexOptions) };
    }

    private class GetLocation : Parse
    {
        public GetLocation() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Место нахождения</(?<val>.*?)</div>", RegexOptions) };
    }

    private class GetStartDate : Parse
    {
        public GetStartDate() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Дата начала срока подачи заявок</(?<val>.*?)</div>", RegexOptions), new(@"Дата и время начала срока подачи заявок</(?<val>.*?)</span>", RegexOptions) };
    }

    private class GetDeadline : Parse
    {
        public GetDeadline() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Дата и время окончания срока подачи заявок \(по местному времени заказчика\)</(?<val>.*?)</div>", RegexOptions), new(@"Дата и время окончания срока подачи заявок</(?<val>.*?)</span>", RegexOptions) };
    }

    private class GetTimeZoneOffset : Parse
    {
        public GetTimeZoneOffset() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@" (?<val>МСК.*?) ", RegexOptions) };
    }

    private class GetSecuring : Parse
    {
        public GetSecuring() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Размер обеспечения заявки</(?<val>.*?)</div>", RegexOptions) };
    }

    private class GetEnforcement : Parse
    {
        public GetEnforcement() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Размер обеспечения исполнения контракта</(?<val>.*?)</div>", RegexOptions) };
    }

    private class GetWarranty : Parse
    {
        public GetWarranty() : base(ProcurementCard) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Размер обеспечения гарантийных обязательств</(?<val>.*?)</div>", RegexOptions) };
    }
}