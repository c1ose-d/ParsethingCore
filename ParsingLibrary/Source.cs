﻿using DatabaseLibrary.Entities.ProcurementProperties;
using DatabaseLibrary.Queries;
using OpenQA.Selenium;

namespace ParsingLibrary;

public class Source : Procurement
{
    public Source(string procurementCard)
    {
        ProcurementCard = procurementCard;
        Initialize();
    }

    private static string ProcurementCard { get; set; } = null!;
    private static string Input { get; set; } = string.Empty;
    public bool IsGetted { get; set; } = false;

    private void Initialize()
    {
        RequestUri = $"https://zakupki.gov.ru/epz/order/notice{new GetRequestUri().Result}";

        string? number = new GetNumber().Result;
        if (number != null)
            Number ??= number;

        string? lawNumber = new GetLawNumber().Result;
        if (lawNumber != null)
        {
            Law? law = GET.Entry.Law(lawNumber);
            if (law == null)
            {
                PUT.Law(new() { Number = lawNumber});
                law = GET.Entry.Law(lawNumber);
            }
            if (law != null)
                LawId = law.Id;
        }

        string? obj = new GetObject().Result;
        if (obj != null)
            Object = obj;

        if (decimal.TryParse(new GetInitialPrice().Result, out decimal initialPrice))
            InitialPrice = initialPrice;
        else InitialPrice = 0;

        string? organizationName = new GetOrganizationName().Result;

        GetInput();
        if (Input != string.Empty)
        {
            string? methodText = new GetLawNumber().Result;
            if (methodText != null)
            {
                Method? method = GET.Entry.Method(methodText);
                if (method == null)
                {
                    PUT.Method(new() { Text = methodText });
                    method = GET.Entry.Method(methodText);
                }
                if (method != null)
                    MethodId = method.Id;
            }

            string? platformName = new GetPlatformName().Result;
            string? platformAddress = new GetPlatformAddress().Result;
            if (platformName != null && platformAddress != null)
            {
                Platform? platform = GET.Entry.Platform(platformName, platformAddress);
                if (platform == null)
                {
                    PUT.Platform(new() { Name = platformName, Address = platformAddress });
                    platform = GET.Entry.Platform(platformName, platformAddress);
                }
                if (platform != null)
                    PlatformId = platform.Id;
            }

            string? organizationPostalAddress = new GetOrganizationPostalAddress().Result;
            if (organizationName != null)
            {
                Organization? organization;
                if (organizationPostalAddress != null)
                    organization = GET.Entry.Organization(organizationName, organizationPostalAddress);
                else organization = GET.Entry.Organization(organizationName);
                if (organization == null)
                {
                    PUT.Organization(new() { Name = organizationName, PostalAddress = organizationPostalAddress });
                    organization = GET.Entry.Organization(organizationName, organizationPostalAddress);
                }
                if (organization != null)
                    OrganizationId = organization.Id;
            }

            Location = new GetLocation().Result;

            if (DateTime.TryParse(new GetStartDate().Result, out DateTime startDate))
                StartDate = startDate;
            else StartDate = null;

            if (DateTime.TryParse(new GetDeadline().Result, out DateTime deadline))
                Deadline = deadline;
            else Deadline = null;

            string? timeZoneOffset = new GetTimeZoneOffset().Result;
            if (timeZoneOffset != null)
            {
                TimeZone? timeZone = GET.Entry.TimeZone(timeZoneOffset);
                if (timeZone == null)
                {
                    PUT.TimeZone(new() { Offset = timeZoneOffset });
                    timeZone = GET.Entry.TimeZone(timeZoneOffset);
                }
                if (timeZone != null)
                    PlatformId = timeZone.Id;
            }

            Securing = new GetSecuring().Result;

            Enforcement = new GetEnforcement().Result;

            Warranty = new GetWarranty().Result;

            IsGetted = true;
        }
    }

    private void GetInput()
    {
        GetRequest request = new(RequestUri);
        Input = request.Input;
    }

    private class GetRequestUri : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"<a target=""_blank"" href=""/epz/order/notice(?<val>.*?)"">", RegexOptions) };
        public GetRequestUri() : base(ProcurementCard) { }
    }

    private class GetNumber : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"<a target=""_blank"" href=""/epz/order/notice(?<space>.*?)"">\n *№ (?<val>.*?)\n", RegexOptions) };
        public GetNumber() : base(ProcurementCard) { }
    }

    private class GetSourceState : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"<div class=""registry-entry__header-mid__title text-normal"">(?<val>.*?)</div>", RegexOptions) };
        public GetSourceState() : base(ProcurementCard) { }
    }

    private class GetLawNumber : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"<div class=""col-9 p-0 registry-entry__header-top__title text-truncate""(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
        public GetLawNumber() : base(ProcurementCard) { }
    }

    private class GetObject : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"Объект закупки</(?<space>.*?)>\n(?<space>.*?)>(?<val>.*?)</div>", RegexOptions) };
        public GetObject() : base(ProcurementCard) { }
    }

    private class GetInitialPrice : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"Начальная цена(?<space>.*?)value"">(?<val>.*,..?) ", RegexOptions) };
        public GetInitialPrice() : base(ProcurementCard) { }
    }

    private class GetOrganizationName : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"href=""/epz/organization/view(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
        public GetOrganizationName() : base(ProcurementCard) { }
    }

    private class GetMethodText : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"Способ определения поставщика \(подрядчика, исполнителя\)</(?<space>.*?)>\n *(?<space>.*?)>(?<val>.*?)<", RegexOptions), new(@"Способ осуществления закупки</(?<space>.*?)>\n(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
        public GetMethodText() : base(Input) { }
    }

    private class GetPlatformName : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"Наименование электронной площадки(?<space>.*?)>\n(?<space>.*?)info"">(?<val>.*?)</", RegexOptions), new(@"Наименование электронной площадки(?<space>.*?)>\n(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
        public GetPlatformName() : base(Input) { }
    }

    private class GetPlatformAddress : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"Адрес электронной площадки(?<space>.*?)</span>(?<space>.*?)href=""(?<val>.*?)""", RegexOptions), new(@"Адрес электронной площадки(?<space>.*?)>\n(?<space>.*?)>\n(?<space>.*?)href=""(?<val>.*?)""", RegexOptions) };
        public GetPlatformAddress() : base(Input) { }
    }

    private class GetOrganizationPostalAddress : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"Почтовый адрес(?<space>.*?)>\n(?<space>.*?)\n *(?<val>.*?)\n", RegexOptions) };
        public GetOrganizationPostalAddress() : base(Input) { }
    }

    private class GetLocation : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"Место нахождения(?<space>.*?)>\n(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
        public GetLocation() : base(Input) { }
    }

    private class GetStartDate : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"начала(?<space>.*?)>\n(?<space>.*?)\n *(?<val>..\...\..... ..:..)", RegexOptions) };
        public GetStartDate() : base(Input) { }
    }

    private class GetDeadline : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"окончания(?<space>.*?)>\n(?<space>.*?)\n *(?<val>..\...\..... ..:..)", RegexOptions) };
        public GetDeadline() : base(Input) { }
    }

    private class GetTimeZoneOffset : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@" (?<val>МСК.*?) ", RegexOptions) };
        public GetTimeZoneOffset() : base(Input) { }
    }

    private class GetSecuring : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"Размер обеспечения заявки</(?<space>.*?)>\n *<(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
        public GetSecuring() : base(Input) { }
    }

    private class GetEnforcement : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"Размер обеспечения исполнения контракта</(?<space>.*?)>\s*<(?<space>.*?)>\n(?<val>.*?)</(?<space>.*?)>", RegexOptions) };
        public GetEnforcement() : base(Input) { }
    }

    private class GetWarranty : Parse
    {
        public override List<Regex> Regexes { get; } = new() { new(@"Размер обеспечения гарантийных обязательств</(?<space>.*?)>\n *<(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
        public GetWarranty() : base(Input) { }
    }
}