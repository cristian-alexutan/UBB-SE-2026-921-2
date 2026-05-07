using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace AirportApp.Data.Repositories.Proxies;

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

        response.EnsureSuccessStatusCode();
        return response.Content.ReadFromJsonAsync<T>(JsonOptions).GetAwaiter().GetResult();
    }

    protected T GetRequired<T>(string requestUri)
    {
        using HttpResponseMessage response = this.HttpClient.GetAsync(requestUri).GetAwaiter().GetResult();
        response.EnsureSuccessStatusCode();
        return response.Content.ReadFromJsonAsync<T>(JsonOptions).GetAwaiter().GetResult()
            ?? throw new InvalidOperationException($"Empty response from '{requestUri}'.");
    }

    protected TResult PostForResult<TValue, TResult>(string requestUri, TValue value)
    {
        using HttpResponseMessage response = this.HttpClient.PostAsJsonAsync(requestUri, value, JsonOptions).GetAwaiter().GetResult();
        response.EnsureSuccessStatusCode();
        return response.Content.ReadFromJsonAsync<TResult>(JsonOptions).GetAwaiter().GetResult()
            ?? throw new InvalidOperationException($"Empty response from '{requestUri}'.");
    }

    protected void Post<TValue>(string requestUri, TValue value)
    {
        using HttpResponseMessage response = this.HttpClient.PostAsJsonAsync(requestUri, value, JsonOptions).GetAwaiter().GetResult();
        response.EnsureSuccessStatusCode();
    }

    protected void Put<TValue>(string requestUri, TValue value)
    {
        using HttpResponseMessage response = this.HttpClient.PutAsJsonAsync(requestUri, value, JsonOptions).GetAwaiter().GetResult();
        response.EnsureSuccessStatusCode();
    }

    protected void Delete(string requestUri)
    {
        using HttpResponseMessage response = this.HttpClient.DeleteAsync(requestUri).GetAwaiter().GetResult();
        response.EnsureSuccessStatusCode();
    }

    protected T? DeleteForResult<T>(string requestUri)
        where T : class
    {
        using HttpResponseMessage response = this.HttpClient.DeleteAsync(requestUri).GetAwaiter().GetResult();
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return response.Content.ReadFromJsonAsync<T>(JsonOptions).GetAwaiter().GetResult();
    }
}
