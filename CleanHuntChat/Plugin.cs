using System.Collections.Generic;
using CleanHuntChat.ChatHandlers;
using CleanHuntChat.Commands;
using CleanHuntChat.Windows;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace CleanHuntChat;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IChatGui ChatGui { get; private set; } = null!;

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("CleanHuntChat");

    private ConfigWindow ConfigWindow { get; init; }

    private readonly List<ICommand> commands;

    private readonly ChatHandler chatHandler;

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        ConfigWindow = new ConfigWindow(this);
        WindowSystem.AddWindow(ConfigWindow);
        
        
        commands = new List<ICommand>();
        InitializeCommands();

        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        chatHandler = new ChatHandler();
        if (Configuration.permanentFilter)
        {
            EnableChatHandler();
        }
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();
        ConfigWindow.Dispose();

        foreach(ICommand command in commands)
        {
            CommandManager.RemoveHandler(command.Name);
            command.Dispose();
        }
        commands.Clear();
        DisableChatHandler();
    }

    private void InitializeCommands()
    {
        ICommand cleanHuntCommand = new CommandCleanHunt(this);
        CommandManager.AddHandler(cleanHuntCommand.Name, new CommandInfo(cleanHuntCommand.OnCommand)
        {
            HelpMessage = cleanHuntCommand.HelpMessage
        });

        commands.Add(cleanHuntCommand);
    }

    public void EnableChatHandler()
    {
        DisableChatHandler();
        ChatGui.CheckMessageHandled += chatHandler.OnChat;
    }

    public void DisableChatHandler()
    {
        ChatGui.CheckMessageHandled -= chatHandler.OnChat;
    }

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();
}
