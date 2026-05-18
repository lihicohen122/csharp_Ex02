namespace Ex02_UI
{
    public class Program
    {
        public static void Main()
        {
            if(ConfigurationUserInterface.TryGetGameConfiguration(out int boardSize, out bool isVsComputer))
            {
                new ConsoleUserInterface(ConfigurationUserInterface.GetQuitSymbol(), boardSize, isVsComputer)
                    .RunGame();
            }
        }
    }
}