using SamplePlugin;
using System;
using System.Timers;


namespace CleanHuntChat.Commands
{
    public class CommandCleanHunt : ICommand
    {
        public string Name => "/cleanhunt";
        public string HelpMessage => $"Turns {Strings.PLUGIN_NAME} on for {plugin.Configuration.EnabledDurationInMinutes} minutes. Type '{Name} off' to disable early.";

        private Plugin plugin;
        private Timer timer;

        public CommandCleanHunt(Plugin plugin)
        {
            this.plugin = plugin;

            this.timer = new Timer();
            this.timer.Elapsed += (s, e) => OnCleanHuntTimerElapsed();
        }

        public void OnCommand(string command, string args)
        {
            if (args.Length == 0)
            {
                HandleMainCommand();
            }

            else if (args.Contains("off", StringComparison.CurrentCultureIgnoreCase)) {
                HandleOffSubCommand();
            }
        }

        private void HandleMainCommand()
        {
            plugin.EnableChatHandler();
            SetupNewTimer(timer);
            Plugin.ChatGui.Print($"Enabled {Strings.PLUGIN_NAME} for {plugin.Configuration.EnabledDurationInMinutes} minutes.");
        }
        private void HandleOffSubCommand()
        {
            plugin.DisableChatHandler();
            timer.Stop();
            Plugin.ChatGui.Print($"Disabled {Strings.PLUGIN_NAME} early.");
        }

        private void OnCleanHuntTimerElapsed()
        {
            plugin.DisableChatHandler();
            timer.Stop();
            Plugin.ChatGui.Print($"{Strings.PLUGIN_NAME} has been disabled.");
        }

        private void SetupNewTimer(Timer timer)
        {
            timer.Stop();
            timer.Interval = MinutesToMilliSeconds(plugin.Configuration.EnabledDurationInMinutes);
            timer.Start();
        }

        private double MinutesToMilliSeconds(int minutes)
        {
            return minutes * 60000;
        }

        public void Dispose()
        {
            timer.Stop();
            timer.Dispose();
        }
    }
}
