using System;
using Ex02_Logic;
using Ex02_Logic.Enums;

namespace Ex02_UI
{
    public class ConsoleUserInterface
    {
        private const int k_LowerBound = 3;
        private const int k_UpperBound = 9;
        private const int k_MinCoordinate = 1;
        private const string k_Quit = "Q";
        private readonly Game r_Game;

        private static void getInitialSettings(out int o_BoardSize, out bool o_IsVsComputer)
        {
            bool isValidBoardSize = false;
            bool isValidIsVsComputer = false;

            o_BoardSize = 0;
            o_IsVsComputer = false;
            while(!isValidBoardSize)
            {
                Console.WriteLine($"Enter board size between {k_LowerBound} and {k_UpperBound}: ");
                isValidBoardSize = int.TryParse(Console.ReadLine(), out o_BoardSize) && k_LowerBound <= o_BoardSize && o_BoardSize <= k_UpperBound;
                if(!isValidBoardSize)
                {
                    Console.WriteLine("Invalid board size!");
                }
            }
            
            while(!isValidIsVsComputer)
            {
                Console.WriteLine("Would you like to play against the computer? (yes/no): ");
                string input = Console.ReadLine();
                if(input == "yes")
                {
                    o_IsVsComputer = true;
                    isValidIsVsComputer = true;
                }
                else if(input == "no")
                {
                    o_IsVsComputer = false;
                    isValidIsVsComputer = true;
                }
                else
                {
                    Console.WriteLine("Invalid answer!");
                }
            }
        }

        private void printBoard()
        {
            int boardSize = r_Game.GetCurrentGameBoardSize();
            
            Ex02.ConsoleUtils.Screen.Clear();
            Console.Write("  ");
            for(int col = 0; col < boardSize; ++col)
            {
                Console.Write($" {col + 1}  "); // NEEDS FIXING!!!
            }
            
            Console.WriteLine();
            for(int row = 0; row < boardSize; ++row)
            {
                Console.Write($"{row + 1}|"); // NEEDS FIXING!!!
                for(int col = 0; col < boardSize; ++col)
                {
                    char signToPrint = ' ';

                    eCellSign currentSign = r_Game.GetCellSign(row, col);
                    if(currentSign == eCellSign.Cross)
                    {
                        signToPrint = 'X';
                    }
                    else if(currentSign == eCellSign.Circle)
                    {
                        signToPrint = 'O';
                    }

                    Console.Write($" {signToPrint} |"); // NEEDS FIXING!!!
                }

                Console.WriteLine();
                Console.Write(" ="); // NEEDS FIXING!!!
                for(int col = 0; col < boardSize; ++col)
                {
                    Console.Write("===="); // NEEDS FIXING!!!
                }
                
                Console.WriteLine();
            }
        }

        private void printScore()
        {
            int []allPlayersScores = r_Game.GetAllPlayersScore();
            
            for(int i = 0; i < allPlayersScores.Length; ++i)
            {
                int playerScore = allPlayersScores[i];
                
                Console.Write($"Player {i + 1} score: {playerScore}");
                if(i < allPlayersScores.Length - 1)
                {
                    Console.Write(" | ");
                }
            }
            Console.WriteLine();
            // Console.WriteLine($"Player 1 score: {player1Score} | Player 2 score: {player2Score}"); // NEEDS FIXING!!!
        }

        private bool readCellCoordinatesOrQuit(out int o_Row, out int o_Col)
        {
            bool didUserQuit = false;
            bool isCellEmpty = false;

            o_Row = 0;
            o_Col = 0;
            while(!isCellEmpty && !didUserQuit)
            {
                bool isCoordinateValid = false;

                while(!isCoordinateValid)
                {
                    Console.WriteLine($"Enter row or '{k_Quit}' to quit: ");
                    string userInput = Console.ReadLine();
                    if(userInput == k_Quit)
                    {
                        didUserQuit = true;
                        break;
                    }

                    isCoordinateValid = int.TryParse(userInput, out o_Row) && k_MinCoordinate <= o_Row && o_Row <= r_Game.GetCurrentGameBoardSize();
                    if(!isCoordinateValid)
                    {
                        Console.WriteLine("Invalid number! Please enter a valid row number.");
                    }
                }

                if(!didUserQuit)
                {
                    isCoordinateValid = false;
                    while(!isCoordinateValid)
                    {
                        Console.WriteLine($"Enter column or '{k_Quit}' to quit: ");
                        string userInput = Console.ReadLine();

                        if(userInput == k_Quit)
                        {
                            didUserQuit = true;
                            break;
                        }

                        isCoordinateValid = int.TryParse(userInput, out o_Col) && k_MinCoordinate <= o_Col && o_Col <= r_Game.GetCurrentGameBoardSize();
                        if(!isCoordinateValid)
                        {
                            Console.WriteLine("Invalid number! Please enter a valid column number.");
                        }
                    }

                    if(!didUserQuit)
                    {
                        o_Row--;
                        o_Col--;
                        if (r_Game.GetCellSign(o_Row, o_Col) == eCellSign.Empty)
                        {
                            isCellEmpty = true;
                        }
                        else
                        {
                            Console.WriteLine("That cell is already occupied! Please choose an empty cell.");
                        }
                    }
                }
            }

            return didUserQuit;
        }

        private bool handleEndOfGame()
        {
            bool isValidInput = false;
            bool playAnotherRound = false;
            
            printScore();
            while(!isValidInput)
            {
                Console.WriteLine("Would you like to play another round? (yes/no): ");
                string userInput = Console.ReadLine();

                if(userInput == "yes")
                {
                    playAnotherRound = true;
                    isValidInput = true;
                    r_Game.StartNewGame();
                }
                else if(userInput == "no")
                {
                    playAnotherRound = false;
                    isValidInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid answer! Please enter 'yes' or 'no'.");
                }
            }

            return playAnotherRound;
        }

        private void playSingleGame()
        {
            bool isGameOver = false;

            while(!isGameOver)
            {
                printBoard();
                if(r_Game.IsCurrentPlayerComputer())
                {
                    r_Game.PlayComputerTurn();
                }
                else
                {
                    if(readCellCoordinatesOrQuit(out int row, out int col))
                    {
                        r_Game.QuitGame();
                    }
                    else
                    {
                        r_Game.PlayUserTurn(row, col);
                    }
                }
                
                isGameOver = r_Game.GetGameState() != eGameState.Playing;
            }
            
            printBoard();
        }

        public ConsoleUserInterface()
        {
            getInitialSettings(out int boardSize, out bool isVsComputer);
            r_Game = new Game(boardSize, isVsComputer);
        }

        public void RunGame()
        {
            bool playAnotherRound = true;

            r_Game.StartNewGame();
            while(playAnotherRound)
            {
                playSingleGame();
                playAnotherRound = r_Game.GetGameState() != eGameState.Quit && handleEndOfGame();
            }
        }
    }
}
