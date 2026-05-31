namespace ChatZone.Core.Notifications;

public class SmtpSettings
{
    public string SMTPServer { get; set; } = string.Empty;
    public int SMTPPort { get; set; }
    public string SMTPEmail { get; set; } = string.Empty;
    public string SMTPPassword { get; set; } = string.Empty;
    public string LinkToClick { get; set; } = string.Empty;
}