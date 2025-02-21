using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class Report
{
    public int ReportId { set; get; }
    public ReportThemeList ReportTheme { get; set; }
    public required string ReportMessage { get; set; }
    public int ReporterId { get; set; }
    public required Person PersonReporter { get; set; }
    public int ReportedId { get; set; }
    public required Person PersonReported { get; set; }
}