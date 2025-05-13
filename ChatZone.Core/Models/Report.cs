using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class Report
{
    public int IdReport { get; set; }
    public ReportThemeList ReportTheme { get; set; }
    public required string ReportMessage { get; set; }
    public int IdReporter { get; set; }
    public int IdReported { get; set; }
    public int? IdSingleMessageReport { get; set; }
    public int? IdGroupMessageReport { get; set; }
    public required Person PersonReporter { get; set; }
    public required Person PersonReported { get; set; }
    public SingleMessage? SingleMessage { get; set; }
    public GroupMessage? GroupMessage { get; set; }
}