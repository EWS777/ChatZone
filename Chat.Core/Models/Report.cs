using Chat.Core.Models.Enums;

namespace Chat.Core.Models;

public class Report
{
    public int ReportId { set; get; }
    public ReportThemeList ReportTheme { get; set; }
    public string ReportMessage { get; set; }
    
    public int ReporterId { get; set; }
    public Person PersonReporter { get; set; }
    public int ReportedId { get; set; }
    public Person PersonReported { get; set; }
    public int ChatId { get; set; }
    public Chat Chat { get; set; }
}