using Chat.Core.Models.Enums;

namespace Chat.Core.Models;

public class PersonSettings
{
    public LangList LangMenu { get; set; }
    public bool IsDark { get; set; }
    public bool IsFindByProfile { get; set; }
    
    public int PersonSettingsId { get; set; }
    public Person Person { get; set; }
}