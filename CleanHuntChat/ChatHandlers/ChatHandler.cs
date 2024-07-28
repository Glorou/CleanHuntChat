using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using System;
using System.Linq;

namespace SamplePlugin.Classes
{
    internal class ChatHandler
    {
        private Plugin plugin;

        public ChatHandler(Plugin plugin)
        {
            this.plugin = plugin;
        }

        public void OnChat(XivChatType type, int timestamp, ref SeString sender, ref SeString message, ref bool isHandled)
        {
            if (IsOneOfTrackedChatTypes(type))
            {
                if (ContainsHuntInfo(message) == false)
                {
                    isHandled = true;
                }
            }
        }

        private bool ContainsHuntInfo(SeString message)
        {
            return message.TextValue.Contains("instance", StringComparison.CurrentCultureIgnoreCase)
                   || ContainsCoordsFlag(message);
        }

        private bool ContainsCoordsFlag(SeString message)
        {
            return message.Payloads.Any(payload => payload.Type == PayloadType.MapLink);
        }

        private bool IsOneOfTrackedChatTypes(XivChatType type)
        {
            return type == XivChatType.Shout || type == XivChatType.Yell;
        }
    }
}
