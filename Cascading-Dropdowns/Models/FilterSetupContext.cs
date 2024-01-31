using System.Text.Json.Serialization;

namespace Cascading_Dropdowns.Models;

public class FilterSetupContext
{
    public enum SubmitType
    {
        None,
        GoBtn,
        Filter,
        Aggregation,
        Calculation,
        Dates
    }

    [JsonIgnore] public List<FilterDefinition>? Filters { get; set; }
    public FilterDefinition? SelectedFilter => Filters?.FirstOrDefault(x => x.id == CurrentRequest.filterId);

    public Aggregation? SelectedAggregation =>
        SelectedFilter?.aggregations.FirstOrDefault(x => x.id == CurrentRequest.aggregationId);

    public required AggregationsRequest CurrentRequest { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SubmitType Type { get; set; }

    public string? PercentileStr { get; set; }


    public bool Submittable => SelectedFilter != null && SelectedAggregation != null &&
                               SelectedAggregation.calculations.Contains(CurrentRequest.calculation) &&
                               CurrentRequest.startTime < CurrentRequest.endTime;
}