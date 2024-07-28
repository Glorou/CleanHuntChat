namespace CleanHuntChat.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string HelpMessage { get; }
        void OnCommand(string command, string args);
        void Dispose();
    }
}
