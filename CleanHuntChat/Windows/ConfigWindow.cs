using System;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace CleanHuntChat.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration configuration;

    // We give this window a constant ID using ###
    // This allows for labels being dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public ConfigWindow(Plugin plugin) : base("Clean Hunt Chat configuration###With a constant ID")
    {
        Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.AlwaysAutoResize;

        configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void PreDraw() { }

    public override void Draw()
    {
        var duration = configuration.EnabledDurationInMinutes;
        var permSetting = configuration.permanentFilter;
        if (ImGui.Checkbox("Have chat filtered permanently", ref permSetting))
        {
            configuration.permanentFilter = permSetting;
            configuration.Save();
        }
        if (ImGui.InputInt("Duration in minutes", ref duration))
        {
            configuration.EnabledDurationInMinutes = duration;
            configuration.Save();
        }

    }
}
