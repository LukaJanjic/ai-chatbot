using System.Text.Json;

namespace backend.Services;

public class GeminiService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private readonly string _model;

    public GeminiService(HttpClient http, IConfiguration cfg)
    {
        _http = http;
        _apiKey = cfg["Gemini:ApiKey"]!;
        _model = cfg["Gemini:Model"]!;
    }

    public async Task<string> AskAsync(string userMessage)
    {
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";

        var payload = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = userMessage }
                    }
                }
            }
        };

        var resp = await _http.PostAsJsonAsync(url, payload);
        var responseBody = await resp.Content.ReadAsStringAsync();

        if (!resp.IsSuccessStatusCode)
        {
            throw new Exception($"Gemini API error {(int)resp.StatusCode}: {responseBody}");
        }

        using var doc = JsonDocument.Parse(responseBody);
        return doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString() ?? "";
    }
}