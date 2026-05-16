using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
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
        private Game m_Game;

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
            int boardSize = m_Game.GetCurrentGameBoardSize();
            
            Ex02.ConsoleUtils.Screen.Clear();
            Console.Write("  ");
            for (int col = 0; col < boardSize; ++col)
            {
                Console.Write($" {col + 1}  "); // NEEDS FIXING!!!
            }
            
            Console.WriteLine();
            for (int row = 0; row < boardSize; ++row)
            {
                Console.Write($"{row + 1}|"); // NEEDS FIXING!!!
                for (int col = 0; col < boardSize; ++col)
                {
                    eCellSign currentSign = m_Game.GetCellSign(row, col);
                    char signToPrint = ' ';
                    
                    if (currentSign == eCellSign.Cross)
                    {
                        signToPrint = 'X';
                    }
                    else if (currentSign == eCellSign.Circle)
                    {
                        signToPrint = 'O';
                    }

                    Console.Write($" {signToPrint} |"); // NEEDS FIXING!!!
                }

                Console.WriteLine();
                Console.Write(" ="); // NEEDS FIXING!!!
                for (int col = 0; col < boardSize; ++col)
                {
                    Console.Write("===="); // NEEDS FIXING!!!
                }
                
                Console.WriteLine();
            }
        }

        private void printScore()
        {
            int []allPlayersScores = m_Game.GetAllPlayersScore();
            
            for(int i = 0; i < allPlayersScores.Length; ++i)
            {
                int playerScore = allPlayersScores[i];
                
                Console.Write($"Player {i + 1} score: {playerScore}");
                if(i < allPlayersScores.Length - 1)
                {
                    Console.Write(" | ");
                }
            }
            // Console.WriteLine($"Player 1 score: {player1Score} | Player 2 score: {player2Score}"); // NEEDS FIXING!!!
        }

        private bool readCellCoordinatesOrQuit(out int o_Row, out int o_Col)
        {
            bool isCoordinateValid = false;

            o_Row = 0;
            o_Col = 0;
            bool didUserQuit = false;
            while(!isCoordinateValid)
            {
                Console.WriteLine($"Enter row or '{k_Quit}' to quit: ");
                string userInput = Console.ReadLine();
                if(userInput == k_Quit)
                {
                    didUserQuit = true;
                    break;
                }

                isCoordinateValid = int.TryParse(userInput, out o_Row) && k_MinCoordinate <= o_Row && o_Row <= m_Game.GetCurrentGameBoardSize();
                if(!isCoordinateValid)
                {
                    Console.WriteLine("Invalid number! Please enter a valid row number.");
                }
            }

            isCoordinateValid = false;

            while(!isCoordinateValid && !didUserQuit)
            {
                Console.WriteLine($"Enter column or '{k_Quit}' to quit: ");
                string userInput = Console.ReadLine();
                if (userInput == k_Quit)
                {
                    didUserQuit = true;
                    break;
                }

                isCoordinateValid = int.TryParse(userInput, out o_Col) && k_MinCoordinate <= o_Col && o_Col <= m_Game.GetCurrentGameBoardSize();
                if(!isCoordinateValid)
                {
                    Console.WriteLine("Invalid number! Please enter a valid column number.");
                }
            }

            o_Row--;
            o_Col--;

            return didUserQuit;
        }

        private bool handleEndOfGame()
        {
            bool isValidInput = false;
            bool playAnotherRound = false;
            while(!isValidInput)
            {
                Console.WriteLine("Would you like to play another round? (yes/no): ");
                string userInput = Console.ReadLine();
                if(userInput == "yes")
                {
                    playAnotherRound = true;
                    isValidInput = true;
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
            bool quit = false;

            while (!isGameOver)
            {
                printBoard();

                if (m_Game.IsCurrentPlayerComputer())
                {
                    m_Game.PlayComputerTurn();
                }
                else
                {
                    quit = readCellCoordinatesOrQuit(out int row, out int col);

                    if (quit)
                    {
                        m_Game.QuitGame();
                    }
                    else
                    {
                        m_Game.PlayUserTurn(row, col);
                    }
                }
                
                isGameOver = m_Game.GetGameState() != eGameState.Playing;
            }
            
            printBoard();
            printScore();
        }

        public ConsoleUserInterface()
        {
            getInitialSettings(out int boardSize, out bool isVsComputer);
            m_Game = new Game(boardSize, isVsComputer);
        }

        public void RunGame()
        {
            bool playAnotherRound = true;

            m_Game.StartPlaying();
            while(playAnotherRound)
            {
                playSingleGame();
                handleEndOfGame();
                playAnotherRound = m_Game.GetGameState() != eGameState.Quit;
            }
        }
    }
}
