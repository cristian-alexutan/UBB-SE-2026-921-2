using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace AirportWebApp.Services.Proxies;

public abstract class RepositoryProxyBase
{
    protected static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected RepositoryProxyBase(HttpClient httpClient)
    {
        this.HttpClient = httpClient;
    }

    protected HttpClient HttpClient { get; }

    protected List<T> GetList<T>(string requestUri)
    {
        return this.GetRequired<List<T>>(requestUri);
    }

    protected T? GetOptional<T>(string requestUri)
        where T : class
    {
        using HttpResponseMessage response = this.HttpClient.GetAsync(requestUri).GetAwaiter().GetResult();
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        EnsureSuccessStatusCode(response, requestUri);
        return ReadContent<T>(response.Content);
    }

    protected T GetRequired<T>(string requestUri)
    {
        using HttpResponseMessage response = this.HttpClient.GetAsync(requestUri).GetAwaiter().GetResult();
        EnsureSuccessStatusCode(response, requestUri);
        return ReadContent<T>(response.Content)
            ?? throw new InvalidOperationException($"Empty response from '{requestUri}'.");
    }

    protected TResult PostForResult<TValue, TResult>(string requestUri, TValue value)
    {
        using HttpResponseMessage response = this.HttpClient.PostAsJsonAsync(requestUri, value, JsonOptions).GetAwaiter().GetResult();
        EnsureSuccessStatusCode(response, requestUri);
        return ReadContent<TResult>(response.Content)
            ?? throw new InvalidOperationException($"Empty response from '{requestUri}'.");
    }

    protected void Post<TValue>(string requestUri, TValue value)
    {
        using HttpResponseMessage response = this.HttpClient.PostAsJsonAsync(requestUri, value, JsonOptions).GetAwaiter().GetResult();
        EnsureSuccessStatusCode(response, requestUri);
    }

    protected void Put<TValue>(string requestUri, TValue value)
    {
        using HttpResponseMessage response = this.HttpClient.PutAsJsonAsync(requestUri, value, JsonOptions).GetAwaiter().GetResult();
        EnsureSuccessStatusCode(response, requestUri);
    }

    protected void Delete(string requestUri)
    {
        using HttpResponseMessage response = this.HttpClient.DeleteAsync(requestUri).GetAwaiter().GetResult();
        EnsureSuccessStatusCode(response, requestUri);
    }

    protected T? DeleteForResult<T>(string requestUri)
        where T : class
    {
        using HttpResponseMessage response = this.HttpClient.DeleteAsync(requestUri).GetAwaiter().GetResult();
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        EnsureSuccessStatusCode(response, requestUri);
        return ReadContent<T>(response.Content);
    }

    private static void EnsureSuccessStatusCode(HttpResponseMessage response, string requestUri)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        string errorBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult().Trim();
        string message = string.IsNullOrWhiteSpace(errorBody)
            ? $"Request to '{requestUri}' failed with status code {(int)response.StatusCode} ({response.ReasonPhrase})."
            : errorBody;

        throw new InvalidOperationException(message);
    }

    private static T? ReadContent<T>(HttpContent content)
    {
        if (typeof(T) == typeof(string))
        {
            string raw = content.ReadAsStringAsync().GetAwaiter().GetResult();
            if (raw.Length > 0 && raw.TrimStart().StartsWith('"'))
            {
                return (T?)(object?)(JsonSerializer.Deserialize<string>(raw, JsonOptions) ?? string.Empty);
            }

            return (T?)(object?)raw;
        }

        return content.ReadFromJsonAsync<T>(JsonOptions).GetAwaiter().GetResult();
    }
}
