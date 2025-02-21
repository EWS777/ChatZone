using ChatZone.Core.Models.Enums;

namespace ChatZone.Core.Models;

public class PersonSettings
{
    public LangList LangMenu { get; set; }
    public bool IsDark { get; set; }
    public bool IsFindByProfile { get; set; }
    public int PersonSettingsId { get; set; }
    public required Person Person { get; set; }
}