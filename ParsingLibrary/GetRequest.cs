namespace ParsingLibrary;

public class GetRequest
{
    public GetRequest(string requestUri)
    {
        RequestUri = requestUri;
        GetRequestAsync().Wait();
    }

    private string RequestUri { get; set; } = null!;
    public string Input { get; set; } = string.Empty;

    private async Task GetRequestAsync()
    {
        using HttpClient httpClient = new() { Timeout = TimeSpan.FromSeconds(5) };
        using HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(RequestUri);
        using HttpContent httpContent = httpResponseMessage.Content;
        Input = await httpContent.ReadAsStringAsync();
    }
}