using System.Text.Json.Serialization;

namespace Chukei;

public class Settings
{
    public ushort Port { get; set; }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Settings))]
[JsonSerializable(typeof(ushort))]
public partial class SettingsContext : JsonSerializerContext { }
