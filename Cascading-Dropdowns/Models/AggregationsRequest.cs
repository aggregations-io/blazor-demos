using System.Text.Json.Serialization;

namespace Cascading_Dropdowns.Models;

public class AggregationsRequest
{
    public required string filterId { get; set; }

    public int aggregationId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CalculationType calculation { get; set; }

    public DateTime startTime { get; set; }

    public DateTime endTime { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public double? percentile { get; set; }

    public bool excludeEmptyGroupings => true;
}