using System.Text.Json.Serialization;

namespace Cascading_Dropdowns.Models;

public record Aggregation(
    int id,
    string name,
    [property: JsonConverter(typeof(CalculationTypeListConverter))]
    List<CalculationType> calculations);