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
        private bool m_DidUserQuit;
        private readonly Game r_Game;

        private void getInitialSettings(out int o_BoardSize, out bool o_IsVsComputer)
        {
            bool isValidBoardSize = false;
            bool isValidIsVsComputer = false;

            m_DidUserQuit = false;
            o_BoardSize = 0;
            o_IsVsComputer = false;
            while(!isValidBoardSize)
            {
                Console.WriteLine($"Enter board size between {k_LowerBound} and {k_UpperBound}: ");
                string userInput = Console.ReadLine();

                if(userInput == k_Quit)
                {
                    m_DidUserQuit = true;
                    break;
                }

                isValidBoardSize = int.TryParse(userInput, out o_BoardSize) && k_LowerBound <= o_BoardSize && o_BoardSize <= k_UpperBound;
                if(!isValidBoardSize)
                {
                    Console.WriteLine("Invalid board size!");
                }
            }

            if(!m_DidUserQuit)
            {
                while(!isValidIsVsComputer)
                {
                    Console.WriteLine("Would you like to play against the computer? (yes/no): ");
                    string userInput = Console.ReadLine();

                    if(userInput == k_Quit)
                    {
                        m_DidUserQuit = true;
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
            }
        }

        private void printBoard()
        {
            int boardSize = r_Game.GetCurrentGameBoardSize();
            
            Ex02.ConsoleUtils.Screen.Clear();
            Console.Write("  ");
            for(int column = 0; column < boardSize; ++column)
            {
                Console.Write($" {column + 1}  ");
            }
            
            Console.WriteLine();
            for(int row = 0; row < boardSize; ++row)
            {
                Console.Write($"{row + 1}|");
                for(int column = 0; column < boardSize; ++column)
                {
                    char signToPrint = ' ';

                    eCellSign currentSign = r_Game.GetCellSign(row, column);
                    if(currentSign == eCellSign.Cross)
                    {
                        signToPrint = 'X';
                    }
                    else if(currentSign == eCellSign.Circle)
                    {
                        signToPrint = 'O';
                    }

                    Console.Write($" {signToPrint} |");
                }

                Console.WriteLine();
                Console.Write(" =");
                for(int column = 0; column < boardSize; ++column)
                {
                    Console.Write("====");
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
        }

        private bool didUserQuitCoordinateInput(string i_CoordinateName, out int o_Coordinate)
        {
            bool isCoordinateValid = false;
            o_Coordinate = 0;

            while(!isCoordinateValid && !m_DidUserQuit)
            {
                Console.WriteLine($"Enter {i_CoordinateName} or '{k_Quit}' to quit: ");
                string userInput = Console.ReadLine();

                if(userInput == k_Quit)
                {
                    m_DidUserQuit = true;
                }
                else
                {
                    isCoordinateValid = int.TryParse(userInput, out o_Coordinate) && k_MinCoordinate <= o_Coordinate && o_Coordinate <= r_Game.GetCurrentGameBoardSize();
                    if (!isCoordinateValid)
                    {
                        Console.WriteLine($"Invalid number! Please enter a valid {i_CoordinateName} number.");
                    }
                }
            }

            return m_DidUserQuit;
        }

        private bool didUserQuitWhileEnteringCoordinates(out int o_Row, out int o_Column)
        {
            bool isCellEmpty = false;

            o_Row = 0;
            o_Column = 0;
            while(!isCellEmpty && !m_DidUserQuit)
            {
                m_DidUserQuit = didUserQuitCoordinateInput("row", out o_Row);

                if(!m_DidUserQuit)
                {
                    m_DidUserQuit = didUserQuitCoordinateInput("column", out o_Column);
                }

                if(!m_DidUserQuit)
                {
                    o_Row--;
                    o_Column--;
                    if (r_Game.GetCellSign(o_Row, o_Column) == eCellSign.Empty)
                    {
                        isCellEmpty = true;
                    }
                    else
                    {
                        Console.WriteLine("That cell is already occupied! Please choose an empty cell.");
                    }
                }
            }

            return m_DidUserQuit;
        }

        private bool checkIfUserWantsToContinueAndShowResults()
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
                    isValidInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid answer! Please enter 'yes' or 'no'.");
                }
            }

            return playAnotherRound;
        }

        private void playSingleRound()
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
                    if(didUserQuitWhileEnteringCoordinates(out int row, out int column))
                    {
                        r_Game.QuitGame();
                    }
                    else
                    {
                        r_Game.PlayUserTurn(row, column);
                    }
                }
                
                isGameOver = r_Game.GetGameState() != eGameState.Playing;
            }
            
            printBoard();
        }

        public ConsoleUserInterface()
        {
            getInitialSettings(out int boardSize, out bool isVsComputer);
            if (!m_DidUserQuit)
            {
                r_Game = new Game(boardSize, isVsComputer);
            }
            else
            {
                r_Game = null;
            }
        }

        public void RunGame()
        {
            if(m_DidUserQuit)
            {
                return;
            }

            bool playAnotherRound = true;

            r_Game.StartNewGame();
            while(playAnotherRound)
            {
                playSingleRound();
                playAnotherRound = r_Game.GetGameState() != eGameState.Quit && checkIfUserWantsToContinueAndShowResults();
            }
        }
    }
}