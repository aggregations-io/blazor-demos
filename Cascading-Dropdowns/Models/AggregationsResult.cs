namespace Cascading_Dropdowns.Models;

public readonly record struct AggregationsResult(DateTime dt, Dictionary<string, string>? groupings, double value);