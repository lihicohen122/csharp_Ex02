namespace Ex02_UI
{
    public class Program
    {
        public static void Main()
        {
            StartupMenu menu = new StartupMenu();
            if(menu.TryGetGameConfiguration(out int boardSize, out bool isVsComputer))
            {
                new ConsoleUserInterface(menu.GetQuitSymbol(), boardSize, isVsComputer).RunGame();
            }
        }
    }
}