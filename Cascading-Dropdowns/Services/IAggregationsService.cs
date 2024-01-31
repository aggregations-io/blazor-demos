using Cascading_Dropdowns.Models;

namespace Cascading_Dropdowns.Services;

public interface IAggregationsService
{
    public IAsyncEnumerable<AggregationsResult> GetResults(AggregationsRequest request);
    public IAsyncEnumerable<FilterDefinition?> GetFilters();
}



public class AggregationsService(HttpClient client) : IAggregationsService
{
    public async IAsyncEnumerable<AggregationsResult> GetResults(AggregationsRequest request)
    {
        var resp = await client
            .PostAsJsonAsync("metrics/results", request);

        resp.EnsureSuccessStatusCode();
        
        await foreach (var r in resp.Content.ReadFromJsonAsAsyncEnumerable<AggregationsResult>())
        {
            yield return r;
        }
    }

    public IAsyncEnumerable<FilterDefinition?> GetFilters() =>
        client
            .GetFromJsonAsAsyncEnumerable<FilterDefinition>("filter-definitions");
}