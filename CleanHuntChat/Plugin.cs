using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using SamplePlugin.Windows;
using SamplePlugin.Classes;
using CleanHuntChat.Commands;
using System.Collections.Generic;

namespace SamplePlugin;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IChatGui ChatGui { get; private set; } = null!;

    private const string CommandName = "/cleanhunt";

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("SamplePlugin");

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

        // This adds a button to the plugin installer entry of this plugin which allows
        // to toggle the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        // Adds another button that is doing the same but for the main ui of the plugin
        //PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;

        chatHandler = new ChatHandler(this);
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();

        foreach(ICommand command in commands)
        {
            CommandManager.RemoveHandler(command.Name);
            command.Dispose();
            commands.Remove(command);
        }

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
    //public void ToggleMainUI() => MainWindow.Toggle();
}
