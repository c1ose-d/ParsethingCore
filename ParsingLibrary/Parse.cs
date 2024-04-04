namespace ParsingLibrary;

public class Parse
{
    public Parse(string input)
    {
        foreach (Regex regex in Regexes)
            try
            {
                string[] res = regex.Split(input);
                Result = regex.Split(input)[^2];
                foreach (KeyValuePair<string, string> replacement in Replacements)
                    Result = Result.Replace(replacement.Key, replacement.Value);
                File.AppendAllText("test.txt", Result);
                if (Result != string.Empty)
                {
                    RemoveWhitespace();
                    break;
                }
            }
            catch { }
        if (Result == string.Empty)
            Result = null;
    }

    public string? Result { get; set; } = null;
    public static RegexOptions RegexOptions { get; } = RegexOptions.Compiled | RegexOptions.Singleline;
    public virtual List<Regex> Regexes { get; } = null!;
    private Dictionary<string, string> Replacements { get; } = new() { { "«", "\"" }, { "»", "\"" }, { "&nbsp;", " " }, { "&#8381;", "Российский рубль" }, { "₽", "Российский рубль" }, { "&#034;", "\"" }, { "\n", "" }, { "\r", "" }, { "&ndash;", "—" }, { "&laquo;", "\"" }, { "&raquo;", "\"" }, { "&quot;", "\"" }, { "&mdash;", "—" }, { "( ", "(" }, { " )", ")" }, { "<span class='highlightColor'>", "" }, { "</span>", "" }, { "div>", "" }, { "<div class=\"registry-entry__body-value\">", "" }, { "<div class=\"common-text__value  \">", "" }, { "<div class=\"data-block__value\">", "" }, { "<div class=\"common-text__value\">", "" }, { "</a>", "" }, { "<span class=\"cardMainInfo__content\">", "" }, { "<span class=\"section__info\">", "" }, { "span>", "" } };

    private void RemoveWhitespace()
    {
        if (Result != null)
        {
            Result = Result.Trim();
            while (Result.Contains("  "))
            {
                Result = Result.Replace("  ", " ");
            }
        }
    }
}