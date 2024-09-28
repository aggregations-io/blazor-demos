using Cascading_Dropdowns.Models;
using Highsoft.Web.Mvc.Charts;
using Highsoft.Web.Mvc.Charts.Rendering;
using Microsoft.AspNetCore.Components;

namespace Cascading_Dropdowns.Components.Pages;

public partial class HighchartsContainer : ComponentBase
{
    [Parameter, EditorRequired] public List<AggregationsResult> Results { get; set; } = null!;
    [Parameter, EditorRequired] public FilterSetupContext ctx { get; set; } = null!;
    private HighchartsRenderer? renderer { get; set; }

    protected override void OnInitialized()
    {
        var groupedData = Results.GroupBy(aggResult =>
        {
            if (aggResult.groupings != null)
            {
                var humanReadable = aggResult
                    .groupings
                    .OrderBy(Grouping => Grouping.Key)
                    .ThenBy(Grouping=> Grouping.Value)
                    .Select(Grouping => $"{Grouping.Key} = {Grouping.Value}");
                return string.Join(", ",humanReadable );
            }
            return string.Empty;
        });
        
        var serieses = new List<Series>();
        foreach (var grouping in groupedData)
        {
            serieses.Add(new LineSeries()
            {
                Name = grouping.Key,
                Data = grouping.Select(aggResult => new LineSeriesData()
                {
                    Y = aggResult.value,
                    X = new DateTimeOffset(aggResult.dt).ToUnixTimeMilliseconds()
                }).ToList()
            });
        }

        var chartOptions =
            new Highcharts
            {
                Chart = new Highsoft.Web.Mvc.Charts.Chart
                {
                    ZoomType = ChartZoomType.X,
                    Type = ChartType.Line,
                    Height = "600px"
                },
                Title = new Title
                {
                    Text = ctx.SelectedFilter!.name
                },
                XAxis = new List<XAxis>
                {
                    new XAxis
                    {
                        Type = "datetime",
                    }
                },
                YAxis = new List<YAxis>
                {
                    new YAxis
                    {
                        Title = new YAxisTitle
                        {
                            Text = "Value"
                        }
                    }
                },
                Legend = new Legend
                {
                    Enabled = true
                },
                PlotOptions = new PlotOptions
                {
                    Series = new PlotOptionsSeries
                    {
                        TurboThreshold = 10000
                    },
                    Line = new PlotOptionsLine
                    {
                        Marker = new PlotOptionsLineMarker
                        {
                            Radius = 2
                        },
                        LineWidth = 1,
                        States = new PlotOptionsLineStates
                        {
                            Hover = new PlotOptionsLineStatesHover
                            {
                                LineWidth = 2
                            }
                        },
                        Threshold = null
                    }
                },
                Series = serieses
            };

        chartOptions.ID = "chart";
        
        renderer = new HighchartsRenderer(chartOptions);
    }
}