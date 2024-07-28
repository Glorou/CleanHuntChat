using Dalamud.Plugin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePlugin.Classes
{
    internal class CommandHandler
    {
        private readonly Plugin plugin;

        public CommandHandler(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void OnCommand(string command, string args)
        {
            var shouldFilterChat = !plugin.Configuration.ShouldFilterChat;

            UpdateChatHandleConfigValue(shouldFilterChat);
            ToggleChatHandler(shouldFilterChat);
            SendToggleMessage(shouldFilterChat);
        }

        private void UpdateChatHandleConfigValue(bool shouldFilterChat)
        {
            plugin.Configuration.ShouldFilterChat = shouldFilterChat;
            plugin.Configuration.Save();
        }

        private void ToggleChatHandler(bool shouldFilterChat)
        {
            if (shouldFilterChat)
            {
                plugin.EnableChatHandler();
            }

            else plugin.DisableChatHandler();
        }

        private void SendToggleMessage(bool shouldFilterChat)
        {
            var valueText = shouldFilterChat ? "enabled" : "disabled";
            var outputText = $"Clean Hunt Chat {valueText}";

            Plugin.ChatGui.Print(outputText);
        }
    }
}
