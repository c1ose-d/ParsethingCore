namespace ParsingLibrary;

public class Parse
{
    public Parse(string input)
    {
        foreach (Regex regex in Regexes)
            try
            {
                Result = regex.Split(input)[^2];
                foreach (KeyValuePair<string, string> replacement in Replacements)
                    Result = Result.Replace(replacement.Key, replacement.Value);
                if (Result != string.Empty)
                {
                    RemoveWhitespace();
                    break;
                }
            }
            catch (Exception ex) { LogWriter.Write(ex); }
    }

    public string? Result { get; set; } = null;
    public static RegexOptions RegexOptions { get; } = RegexOptions.Compiled | RegexOptions.Singleline;
    public virtual List<Regex> Regexes { get; } = null!;
    private Dictionary<string, string> Replacements { get; } = new() { { "«", "\"" }, { "»", "\"" }, { "&nbsp;", " " }, { "&#8381;", "Российский рубль" }, { "&#034;", "\"" }, { "\n", "" }, { "&ndash;", "—" }, { "&laquo;", "\"" }, { "&raquo;", "\"" }, { "&quot;", "\"" }, { "&mdash;", "—" }, { "( ", "(" }, { " )", ")" }, { "<span class='highlightColor'>", "" }, { "</span>", "" } };

    private void RemoveWhitespace()
    {
        if (Result != null)
        {
            Regex whitespace = new(@"(\s\s)+");
            Regex whitespaceBetween = new(@"\S(\s\s)+\S");
            Regex whitespacePreview = new(@"^\s+");
            if (whitespaceBetween.IsMatch(Result))
                Result = whitespace.Replace(Result, " ");
            Result = whitespace.Replace(Result, "");
            Result = whitespacePreview.Replace(Result, "");
        }
    }

}