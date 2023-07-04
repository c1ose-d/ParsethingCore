namespace TestProject;

[TestClass]
public class ParseTest
{
    static string Input { get; set; } = new GetRequest("https://zakupki.gov.ru/epz/order/notice/notice223/common-info.html?noticeInfoId=15305793").Input;

    [TestMethod]
    public void GetMethodTextTest()
    {
        GetMethodText methodText = new();
        string? excepted = "Аукцион в электронной форме, участниками которого могут быть только субъекты малого и среднего предпринимательства", actual = methodText.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetPlatformNameTest()
    {
        GetPlatformName platformName = new();
        string? excepted = "АКЦИОНЕРНОЕ ОБЩЕСТВО \"СБЕРБАНК-АВТОМАТИЗИРОВАННАЯ СИСТЕМА ТОРГОВ\"", actual = platformName.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetPlatformAddressTest()
    {
        GetPlatformAddress platformAddress = new();
        string? excepted = "http://www.sberbank-ast.ru/", actual = platformAddress.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetOrganizationPostalAddressTest()
    {
        GetOrganizationPostalAddress organizationPostalAddress = new();
        string? excepted = "689000, АО. Чукотский, г. Анадырь, ул. Южная", actual = organizationPostalAddress.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetLocationTest()
    {
        GetLocation location = new();
        string? excepted = "689000, АВТОНОМНЫЙ ОКРУГ ЧУКОТСКИЙ, Г. АНАДЫРЬ, УЛ. ЮЖНАЯ, дом Д. 4", actual = location.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetStartDateTest()
    {
        GetStartDate startDate = new();
        DateTime? excepted = Convert.ToDateTime("14.06.2023 14:20"), actual = Convert.ToDateTime(startDate.Result);
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetDeadlineTest()
    {
        GetDeadline deadline = new();
        DateTime? excepted = Convert.ToDateTime("14.06.2023 14:20"), actual = Convert.ToDateTime(deadline.Result);
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetTimeZoneOffsetTest()
    {
        GetTimeZoneOffset timeZoneOffset = new();
        string? excepted = "МСК", actual = timeZoneOffset.Result;
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
        string? excepted = null, actual = enforcement.Result;
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

        public override List<Regex> Regexes { get; } = new() { new(@"Способ определения поставщика \(подрядчика, исполнителя\)</(?<space>.*?)>\n *(?<space>.*?)>(?<val>.*?)<", RegexOptions), new(@"Способ осуществления закупки</(?<space>.*?)>\n(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
    }

    private class GetPlatformName : Parse
    {
        public GetPlatformName() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Наименование электронной площадки(?<space>.*?)>\n(?<space>.*?)>(?<val>.*?)</", RegexOptions), new(@"Наименование электронной площадки(?<space>.*?)>\n(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
    }

    private class GetPlatformAddress : Parse
    {
        public GetPlatformAddress() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"Адрес электронной площадки(?<space>.*?)</span>(?<space>.*?)href=""(?<val>.*?)""", RegexOptions), new(@"Адрес электронной площадки(?<space>.*?)>\n(?<space>.*?)>\n(?<space>.*?)href=""(?<val>.*?)""", RegexOptions) };
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