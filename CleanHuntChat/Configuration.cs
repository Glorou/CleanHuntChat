using System;
using Dalamud.Configuration;

namespace CleanHuntChat;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 1;
    public int EnabledDurationInMinutes { get; set; } = 60;
    
    public bool permanentFilter { get; set; } = false;

    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
