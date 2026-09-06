namespace TaskManagement.Common.Settings;

public class IdGeneratorSetting
{
    /// Worker slot encoded in the id. Valid range is 0-7 (3 bits)
    public byte WorkerId { get; set; }
}
