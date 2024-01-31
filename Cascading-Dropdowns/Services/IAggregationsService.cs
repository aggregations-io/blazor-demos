using Cascading_Dropdowns.Models;

namespace Cascading_Dropdowns.Services;

public interface IAggregationsService
{
    public IAsyncEnumerable<AggregationsResult> GetResults(AggregationsRequest request);
    public IAsyncEnumerable<FilterDefinition?> GetFilters();
}

public class AggregationsService(IHttpClientFactory factory) : IAggregationsService
{
    public const string HTTP_CLIENT_NAME = "AggregationsClient";

    public async IAsyncEnumerable<AggregationsResult> GetResults(AggregationsRequest request)
    {
        var resp = await factory.CreateClient(HTTP_CLIENT_NAME)
            .PostAsJsonAsync("metrics/results", request);

        resp.EnsureSuccessStatusCode();

        await foreach (var r in resp.Content.ReadFromJsonAsAsyncEnumerable<AggregationsResult>())
        {
            yield return r;
        }
    }

    public IAsyncEnumerable<FilterDefinition?> GetFilters() =>
        factory
            .CreateClient(HTTP_CLIENT_NAME)
            .GetFromJsonAsAsyncEnumerable<FilterDefinition>("filter-definitions");
}