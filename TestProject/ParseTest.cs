namespace TestProject;

[TestClass]
public class ParseTest
{
    static string Input { get; set; } = new GetRequest("https://zakupki.gov.ru/epz/order/notice/notice223/common-info.html?noticeInfoId=15158506").Input;

    [TestMethod]
    public void GetMethodTextTest()
    {
        GetMethodText methodText = new();
        string? excepted = "����������� �������", actual = methodText.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetPlatformNameTest()
    {
        GetPlatformName platformName = new();
        string? excepted = "��� ���-����", actual = platformName.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetPlatformAddressTest()
    {
        GetPlatformAddress platformAddress = new();
        string? excepted = "http://www.tektorg.ru/", actual = platformAddress.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetOrganizationPostalAddressTest()
    {
        GetOrganizationPostalAddress organizationPostalAddress = new();
        string? excepted = "614042 �������� ���� � ����� �� ������, 41", actual = organizationPostalAddress.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetLocationTest()
    {
        GetLocation location = new();
        string? excepted = "614042 �������� ���� � ����� �� ������, 41", actual = location.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetStartDateTest()
    {
        GetStartDate startDate = new();
        DateTime? excepted = Convert.ToDateTime("13.04.2023 08:19"), actual = Convert.ToDateTime(startDate.Result);
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetDeadlineTest()
    {
        GetDeadline deadline = new();
        DateTime? excepted = Convert.ToDateTime("28.04.2023 07:00"), actual = Convert.ToDateTime(deadline.Result);
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetTimeZoneOffsetTest()
    {
        GetTimeZoneOffset timeZoneOffset = new();
        string? excepted = "���+2", actual = timeZoneOffset.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetSecuringTest()
    {
        GetSecuring securing = new();
        string? excepted = "11 927,43 ���������� �����", actual = securing.Result;
        Assert.AreEqual(excepted, actual);
    }

    [TestMethod]
    public void GetEnforcementTest()
    {
        GetEnforcement enforcement = new();
        string? excepted = "119 274,32 ���������� ����� (10 %)", actual = enforcement.Result;
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

        public override List<Regex> Regexes { get; } = new() { new(@"������ ����������� ���������� \(����������, �����������\)</(?<space>.*?)>\n *(?<space>.*?)>(?<val>.*?)<", RegexOptions), new(@"������ ������������� �������</(?<space>.*?)>\n(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
    }

    private class GetPlatformName : Parse
    {
        public GetPlatformName() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"������������ ����������� ��������(?<space>.*?)>\n(?<space>.*?)>(?<val>.*?)</", RegexOptions), new(@"������������ ����������� ��������(?<space>.*?)>\n(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
    }

    private class GetPlatformAddress : Parse
    {
        public GetPlatformAddress() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"����� ����������� ��������(?<space>.*?)</span>(?<space>.*?)href=""(?<val>.*?)""", RegexOptions), new(@"����� ����������� ��������(?<space>.*?)>\n(?<space>.*?)>\n(?<space>.*?)href=""(?<val>.*?)""", RegexOptions) };
    }

    private class GetOrganizationPostalAddress : Parse
    {
        public GetOrganizationPostalAddress() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"�������� �����(?<space>.*?)>\n(?<space>.*?)\n *(?<val>.*?)\n", RegexOptions) };
    }

    private class GetLocation : Parse
    {
        public GetLocation() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"����� ����������(?<space>.*?)>\n(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
    }

    private class GetStartDate : Parse
    {
        public GetStartDate() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"������(?<space>.*?)>\n(?<space>.*?)\n *(?<val>..\...\..... ..:..)", RegexOptions) };
    }

    private class GetDeadline : Parse
    {
        public GetDeadline() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"���������(?<space>.*?)>\n(?<space>.*?)\n *(?<val>..\...\..... ..:..)", RegexOptions) };
    }

    private class GetTimeZoneOffset : Parse
    {
        public GetTimeZoneOffset() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@" (?<val>���.*?) ", RegexOptions) };
    }

    private class GetSecuring : Parse
    {
        public GetSecuring() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"������ ����������� ������</(?<space>.*?)>\n *<(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
    }

    private class GetEnforcement : Parse
    {
        public GetEnforcement() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"������ ����������� ���������� ���������</(?<space>.*?)>\s*<(?<space>.*?)>\n(?<val>.*?)</(?<space>.*?)>", RegexOptions) };
    }

    private class GetWarranty : Parse
    {
        public GetWarranty() : base(Input) { }

        public override List<Regex> Regexes { get; } = new() { new(@"������ ����������� ����������� ������������</(?<space>.*?)>\n *<(?<space>.*?)>\n *(?<val>.*?)\n", RegexOptions) };
    }
}