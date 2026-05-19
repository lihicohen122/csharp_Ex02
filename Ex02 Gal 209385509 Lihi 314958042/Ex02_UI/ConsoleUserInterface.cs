using System;
using Ex02_Logic;
using Ex02_Logic.Enums;

namespace Ex02_UI
{
    public class ConsoleUserInterface
    {
        private const int k_MinCoordinate = 1;
        private readonly string r_Quit;
        private readonly Game r_Game;

        private void printBoard()
        {
            int boardSize = r_Game.BoardSize;
            
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
            bool didUserQuit = false;

            o_Coordinate = 0;
            while(!isCoordinateValid && !didUserQuit)
            {
                Console.WriteLine($"Enter {i_CoordinateName} or '{r_Quit}' to quit: ");
                string userInput = Console.ReadLine();

                if(userInput == r_Quit)
                {
                    didUserQuit = true;
                }
                else
                {
                    isCoordinateValid = int.TryParse(userInput, out o_Coordinate) &&
                                        k_MinCoordinate <= o_Coordinate && o_Coordinate <= r_Game.BoardSize;
                    if(!isCoordinateValid)
                    {
                        Console.WriteLine($"Invalid number! Please enter a valid {i_CoordinateName} number.");
                    }
                }
            }

            return didUserQuit;
        }

        private bool didUserQuitWhileEnteringCoordinates(out int o_Row, out int o_Column)
        {
            bool isCellEmpty = false;
            bool didUserQuit = false;

            o_Row = 0;
            o_Column = 0;
            while(!isCellEmpty && !didUserQuit)
            {
                didUserQuit = didUserQuitCoordinateInput("row", out o_Row);
                if(!didUserQuit)
                {
                    didUserQuit = didUserQuitCoordinateInput("column", out o_Column);
                }

                if(!didUserQuit)
                {
                    o_Row--;
                    o_Column--;
                    if(r_Game.GetCellSign(o_Row, o_Column) == eCellSign.Empty)
                    {
                        isCellEmpty = true;
                    }
                    else
                    {
                        Console.WriteLine("That cell is already occupied! Please choose an empty cell.");
                    }
                }
            }

            return didUserQuit;
        }

        private bool checkIfUserWantsToContinueAndShowResults()
        {
            bool isValidInput = false;
            bool wantsToContinue = false;
            
            printScore();
            while(!isValidInput)
            {
                Console.WriteLine("Would you like to play another round? (yes/no): ");
                string userInput = Console.ReadLine();

                if(userInput == "yes")
                {
                    wantsToContinue = true;
                    isValidInput = true;
                    r_Game.StartNewGame();
                }
                else if(userInput == "no" || userInput == r_Quit)
                {
                    wantsToContinue = false;
                    isValidInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid answer! Please enter 'yes' or 'no'.");
                }
            }
            
            return wantsToContinue;
        }

        private void playSingleRound()
        {
            bool isGameOver = false;

            while(!isGameOver)
            {
                printBoard();
                if(r_Game.IsCurrentPlayerComputer)
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
                
                isGameOver = r_Game.GameState != eGameState.Playing;
            }
            
            printBoard();
        }

        public ConsoleUserInterface(string i_QuitSymbol, int i_BoardSize, bool i_IsVsComputer)
        {
            r_Quit = i_QuitSymbol;
            r_Game = new Game(i_BoardSize, i_IsVsComputer);
            r_Game.StartNewGame();
        }

        public void RunGame()
        {
            bool playAnotherRound = true;
            
            while(playAnotherRound)
            {
                playSingleRound();
                if(r_Game.GameState == eGameState.Quit)
                {
                    break;
                }
                
                playAnotherRound = checkIfUserWantsToContinueAndShowResults();
            }
        }
    }
}