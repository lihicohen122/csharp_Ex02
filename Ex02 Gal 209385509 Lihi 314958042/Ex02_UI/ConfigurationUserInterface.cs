using System;

namespace Ex02_UI
{
    public class ConfigurationUserInterface
    {
        private const int k_LowerBound = 3;
        private const int k_UpperBound = 9;
        private const string k_Quit = "Q";

        public static string GetQuitSymbol()
        {
            return k_Quit;
        }

        public static bool TryGetGameConfiguration(out int o_BoardSize, out bool o_IsVsComputer)
        {
            bool isSetupSuccessful = tryGetBoardSize(out o_BoardSize);

            if(isSetupSuccessful)
            {
                isSetupSuccessful = tryGetIsVsComputer(out o_IsVsComputer);
            }
            else
            {
                o_IsVsComputer = false;
            }

            return isSetupSuccessful;
        }

        private static bool tryGetBoardSize(out int o_BoardSize)
        {
            bool isValidBoardSize = false;

            o_BoardSize = 0;
            while(!isValidBoardSize)
            {
                Console.WriteLine($"Enter board size between {k_LowerBound} and {k_UpperBound}: ");
                string userInput = Console.ReadLine();

                if(isQuitCommand(userInput))
                {
                    break;
                }

                isValidBoardSize = int.TryParse(userInput, out o_BoardSize) && k_LowerBound <= o_BoardSize && o_BoardSize <= k_UpperBound;
                if (!isValidBoardSize)
                {
                    Console.WriteLine("Invalid board size!");
                }
            }

            return isValidBoardSize;
        }

        private static bool tryGetIsVsComputer(out bool o_IsVsComputer)
        {
            bool isValidIsVsComputer = false;

            o_IsVsComputer = false;
            while(!isValidIsVsComputer)
            {
                Console.WriteLine("Would you like to play against the computer? (yes/no): ");
                string userInput = Console.ReadLine();

                if(isQuitCommand(userInput))
                {
                    break;
                }

                if(userInput == "yes")
                {
                    o_IsVsComputer = true;
                    isValidIsVsComputer = true;
                }
                else if(userInput == "no")
                {
                    o_IsVsComputer = false;
                    isValidIsVsComputer = true;
                }
                else
                {
                    Console.WriteLine("Invalid answer!");
                }
            }

            return isValidIsVsComputer;
        }

        private static bool isQuitCommand(string i_UserInput)
        {
            return i_UserInput == k_Quit;
        }
    }
}