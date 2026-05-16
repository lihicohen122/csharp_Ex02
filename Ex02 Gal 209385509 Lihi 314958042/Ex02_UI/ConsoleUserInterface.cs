using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex02_Logic;

namespace Ex02_UI
{
    public class ConsoleUserInterface
    {

        private Game m_Game;

        private static void getInitialSettings(out int o_BoardSize, out bool o_IsVsComputer)
        {
            bool isValidBoardSize = false;
            bool isValidIsVsComputer = false;
            o_BoardSize = 0;
            o_IsVsComputer = false;

            while(!isValidBoardSize)
            {
                Console.WriteLine("Enter board size between 3 and 9: ");
                isValidBoardSize = int.TryParse(Console.ReadLine(), out o_BoardSize) && 3 < o_BoardSize && o_BoardSize < 9;
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
            int boardSize = m_Game.getBoardSize();
            
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
            int []allPlayersScores = m_Game.getAllPlayersScore();
            
            for(int i = 0; i < allPlayersScores.Length; ++i)
            {
                int playerScore = allPlayersScores[i];
                
                Console.Write($"Player {i + 1} score: {playerScore}");
                if (i < allPlayersScores.Length - 1)
                {
                    Console.Write(" | ");
                }
            }
            Console.WriteLine($"Player 1 score: {player1Score} | Player 2 score: {player2Score}"); // NEEDS FIXING!!!
        }

        private void getPlayerMode(out int o_Row, out int o_Col, out bool o_Quit)
        {
            
        }

        private bool handleEndOfGame()
        {
            
        }

        private void playSingleGame()
        {
            bool isGameOver = false;
            bool quit = false;

            while (!isGameOver)
            {
                printBoard();

                if (m_Game.IsComputerTurn())
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
                
                isGameOver = m_Game.GameState != eGameState.Playing;
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

            while(playAnotherRound)
            {
                playSingleGame();
                handleEndOfGame();
                playAnotherRound = m_Game.GameState != eGameState.Quit;
            }

        }
    }
}
