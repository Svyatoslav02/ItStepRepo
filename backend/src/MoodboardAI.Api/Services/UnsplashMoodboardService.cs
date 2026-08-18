using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using MoodboardAI.Api.Configuration;
using MoodboardAI.Api.Models;

namespace MoodboardAI.Api.Services;

/// <summary>
/// Generates moodboards by searching real photos on Unsplash's free
/// Search Photos API, based on the user's text prompt. No paid API is
/// involved — Unsplash's "Demo" application tier is free (50 requests/hour,
/// no payment method required).
/// </summary>
public class UnsplashMoodboardService : IMoodboardService
{
    private readonly HttpClient _httpClient;
    private readonly UnsplashSettings _settings;
    private readonly ILogger<UnsplashMoodboardService> _logger;

    public UnsplashMoodboardService(
        HttpClient httpClient,
        IOptions<UnsplashSettings> settings,
        ILogger<UnsplashMoodboardService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<MoodboardResponse> GenerateAsync(MoodboardRequest request)
    {
        var perPage = _settings.ResultsPerPage is > 0 and <= 30
            ? _settings.ResultsPerPage
            : 6;

        var requestUri =
            $"{_settings.BaseUrl.TrimEnd('/')}/search/photos" +
            $"?query={Uri.EscapeDataString(request.Prompt)}" +
            $"&per_page={perPage}" +
            "&orientation=squarish";

        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
        httpRequest.Headers.Add("Authorization", $"Client-ID {_settings.AccessKey}");

        UnsplashSearchResponse? searchResult;

        try
        {
            using var httpResponse = await _httpClient.SendAsync(httpRequest);
            httpResponse.EnsureSuccessStatusCode();

            searchResult = await httpResponse.Content.ReadFromJsonAsync<UnsplashSearchResponse>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Unsplash API request failed for prompt '{Prompt}'.", request.Prompt);
            throw new MoodboardGenerationException(
                "Image provider is currently unavailable. Please try again shortly.", ex);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Unsplash API request timed out for prompt '{Prompt}'.", request.Prompt);
            throw new MoodboardGenerationException(
                "Image provider took too long to respond. Please try again.", ex);
        }

        var images = (searchResult?.Results ?? new List<UnsplashPhoto>())
            .Select(photo => new MoodboardImage
            {
                Url = photo.Urls?.Regular ?? photo.Urls?.Small ?? string.Empty,
                Title = string.IsNullOrWhiteSpace(photo.Description)
                    ? request.Prompt
                    : photo.Description!,
                SourceUrl = photo.Links?.Html ?? "https://unsplash.com"
            })
            .Where(image => !string.IsNullOrWhiteSpace(image.Url))
            .ToList();

        return new MoodboardResponse
        {
            Prompt = request.Prompt,
            Images = images
        };
    }

    // ── Unsplash response shapes (only the fields we need) ──────────────

    private class UnsplashSearchResponse
    {
        [JsonPropertyName("results")]
        public List<UnsplashPhoto>? Results { get; set; }
    }

    private class UnsplashPhoto
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("alt_description")]
        public string? AltDescription { get; set; }

        [JsonPropertyName("urls")]
        public UnsplashPhotoUrls? Urls { get; set; }

        [JsonPropertyName("links")]
        public UnsplashPhotoLinks? Links { get; set; }
    }

    private class UnsplashPhotoUrls
    {
        [JsonPropertyName("regular")]
        public string? Regular { get; set; }

        [JsonPropertyName("small")]
        public string? Small { get; set; }
    }

    private class UnsplashPhotoLinks
    {
        [JsonPropertyName("html")]
        public string? Html { get; set; }
    }
}

/// <summary>
/// Thrown when moodboard generation fails due to an upstream (Unsplash)
/// error. Caught by <see cref="Controllers.MoodboardController"/> and
/// turned into a clean 502 response.
/// </summary>
public class MoodboardGenerationException : Exception
{
    public MoodboardGenerationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}