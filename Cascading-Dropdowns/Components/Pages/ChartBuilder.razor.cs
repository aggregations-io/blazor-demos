using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Cascading_Dropdowns.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Cascading_Dropdowns.Components.Pages;

public partial class ChartBuilder
{
    private List<AggregationsResult>? results { get; set; }
    private bool results_loading { get; set; }

    [Parameter, SupplyParameterFromForm(FormName = "charts")]
    public FilterSetupContext? ctx { get; set; }


    protected override async Task OnInitializedAsync()
    {
        await CleanupCtx();
    }

    [MemberNotNull(nameof(ctx))]
    private async Task CleanupCtx()
    {
        if (ctx == null)
        {
            ctx = new()
            {
                CurrentRequest = new()
                {
                    filterId = string.Empty,
                    startTime = DateTime.UtcNow.AddDays(-14), endTime = DateTime.UtcNow
                }
            };
        }

        if (ctx.Filters == null)
        {
            ctx.Filters = new List<FilterDefinition>();
            await foreach (var f in _svc.GetFilters())
            {
                if (f != null)
                {
                    ctx.Filters.Add(f);
                }
            }
        }

        if (ctx.SelectedFilter != null && ctx.SelectedAggregation == null)
        {
            ctx.CurrentRequest.aggregationId = ctx.SelectedFilter.aggregations.First().id;
        }

        if (ctx.SelectedAggregation != null)
        {
            if (!ctx.SelectedAggregation.calculations.Contains(ctx.CurrentRequest.calculation))
            {
                ctx.CurrentRequest.calculation = ctx.SelectedAggregation.calculations.First();
            }

            if (ctx.CurrentRequest.calculation == CalculationType.PERCENTILES)
            {
                if (double.TryParse(ctx.PercentileStr ?? String.Empty, out double _d))
                {
                    ctx.CurrentRequest.percentile = _d;
                }
            }
        }
    }

    private async Task FormSubmitted(EditContext obj)
    {
        if (ctx!=null && ctx.Type == FilterSetupContext.SubmitType.GoBtn && ctx.Submittable)
        {
            results_loading = true;
            await Task.Delay(5);
            results = new List<AggregationsResult>();

            var res = _svc.GetResults(ctx.CurrentRequest);
            await foreach (var r in res)
            {
                results.Add(r);
            }

            results_loading = false;
        }
    }
}