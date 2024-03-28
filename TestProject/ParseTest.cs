namespace TestProject;

[TestClass]
public class ParseTest
{
    static string Input { get; set; } = new GetRequest("https://zakupki.gov.ru/epz/order/notice/ea20/view/common-info.html?regNumber=0134300005224000031").Input;

    [TestMethod]
    public void GetMethodTextTest()
    {
        GetMethodText methodText = new();
        string? excepted = "Электронный аукцион", actual = methodText.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetInitialPriceTest()
    {
        GetInitialPrice initialPrice = new();
        string? excepted = "619 554,00", actual = initialPrice.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetPlatformNameTest()
    {
        GetPlatformName platformName = new();
        string? excepted = "РТС-тендер", actual = platformName.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetPlatformAddressTest()
    {
        GetPlatformAddress platformAddress = new();
        string? excepted = "http://www.rts-tender.ru", actual = platformAddress.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetOrganizationPostalAddressTest()
    {
        GetOrganizationPostalAddress organizationPostalAddress = new();
        string? excepted = "Российская Федерация, 665653, Иркутская обл, Нижнеилимский р-н, Железногорск-Илимский г, КВ-Л 8, Д.20/-, -", actual = organizationPostalAddress.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetLocationTest()
    {
        GetLocation location = new();
        string? excepted = "Российская Федерация, 665653, Иркутская обл, Нижнеилимский р-н, Железногорск-Илимский г, КВ-Л 8, Д.20/-, -", actual = location.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetStartDateTest()
    {
        GetStartDate startDate = new();
        DateTime? excepted = Convert.ToDateTime("20.03.2024 12:11"), actual = Convert.ToDateTime(startDate.Result);
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetDeadlineTest()
    {
        GetDeadline deadline = new();
        DateTime? excepted = Convert.ToDateTime("28.03.2024 10:00"), actual = Convert.ToDateTime(deadline.Result);
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetTimeZoneOffsetTest()
    {
        GetTimeZoneOffset timeZoneOffset = new();
        string? excepted = "МСК+5", actual = timeZoneOffset.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetSecuringTest()
    {
        GetSecuring securing = new();
        string? excepted = null, actual = securing.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetEnforcementTest()
    {
        GetEnforcement enforcement = new();
        string? excepted = "7 %", actual = enforcement.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetWarrantyTest()
    {
        GetWarranty warranty = new();
        string? excepted = null, actual = warranty.Result;
        Assert.AreEqual(excepted, actual);
    }

    private class GetMethodText : Parse
    {
        public GetMethodText() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Способ определения поставщика \(подрядчика, исполнителя\)</(?<space>.*?)>\n *(?<space>.*?)>(?<val>.*?)<", RegexOptions), new(@"Ñïîñîá îñóùåñòâëåíèÿ çàêóïêè</(?<space>.*?)>\n(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
    }

    private class GetInitialPrice : Parse
    {
        public GetInitialPrice() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"<span\s+class=""cardMainInfo__content cost"">\s*([\d\s]+,\d{2})\s*", RegexOptions) };

    }

    private class GetPlatformName : Parse
    {
        public GetPlatformName() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Наименование электронной площадки(?<space>.*?)>\n(?<space>.*?)>(?<val>.*?)</", RegexOptions), new(@"Íàèìåíîâàíèå ýëåêòðîííîé ïëîùàäêè(?<space>.*?)>\n(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
    }

    private class GetPlatformAddress : Parse
    {
        public GetPlatformAddress() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Адрес электронной площадки(?<space>.*?)</span>(?<space>.*?)href=""(?<val>.*?)""", RegexOptions), new(@"Àäðåñ ýëåêòðîííîé ïëîùàäêè(?<space>.*?)>\n(?<space>.*?)>\n(?<space>.*?)href=""(?<val>.*?)""", RegexOptions) };
    }

    private class GetOrganizationPostalAddress : Parse
    {
        public GetOrganizationPostalAddress() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Почтовый адрес(?<space>.*?)>\n(?<space>.*?)\n *(?<val>.*?)\n", RegexOptions) };
    }

    private class GetLocation : Parse
    {
        public GetLocation() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Место нахождения(?<space>.*?)>\n(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
    }

    private class GetStartDate : Parse
    {
        public GetStartDate() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"начала(?<space>.*?)>\n(?<space>.*?)\n *(?<val>..\...\..... ..:..)", RegexOptions) };
    }

    private class GetDeadline : Parse
    {
        public GetDeadline() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"окончания(?<space>.*?)>\n(?<space>.*?)\n *(?<val>..\...\..... ..:..)", RegexOptions) };
    }

    private class GetTimeZoneOffset : Parse
    {
        public GetTimeZoneOffset() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@" (?<val>МСК.*?) ", RegexOptions) };
    }

    private class GetSecuring : Parse
    {
        public GetSecuring() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Размер обеспечения заявки</(?<space>.*?)>\n *<(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
    }

    private class GetEnforcement : Parse
    {
        public GetEnforcement() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Размер обеспечения исполнения контракта</(?<space>.*?)>\s*<(?<space>.*?)>\n(?<val>.*?)</(?<space>.*?)>", RegexOptions) };
    }

    private class GetWarranty : Parse
    {
        public GetWarranty() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Размер обеспечения гарантийных обязательств</(?<space>.*?)>\n *<(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
    }
}