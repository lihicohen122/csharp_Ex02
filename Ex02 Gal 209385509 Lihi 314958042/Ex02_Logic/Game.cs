using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex02_Logic.Enums;

namespace Ex02_Logic
{
    public class Game
    {
        private Board m_Board;
        private Player m_Player1;
        private Player m_Player2;
        private Player m_CurrentPlayer;
        private eGameState m_GameState { get; set; }

        public Game(int i_BoardSize, bool i_IsPlayerComputer)
        {
            m_Board = new Board(i_BoardSize);
            m_Player1 = new Player(eCellSign.Cross, false);
            m_Player2 = new Player(eCellSign.Circle, i_IsPlayerComputer);
            m_CurrentPlayer = m_Player1;
            m_GameState = eGameState.NotInitialized;
        }

        public void StartPlaying()
        {
            m_GameState = eGameState.Playing;
        }

        public eGameState GetGameState()
        {
            return m_GameState;
        }

        public void QuitGame()
        {
            m_GameState = eGameState.Quit;
        }

        public eCellSign GetCurrentPlayerSign()
        {
            return m_CurrentPlayer.GetPlayerSign();
        }

        public bool IsCurrentPlayerComputer()
        {
            return m_CurrentPlayer.IsPlayerComputer();
        }

        public eCellSign GetCellSign(int i_Row, int i_Col)
        {
            return m_Board.GetCell(i_Row, i_Col);
        }

        public int GetCurrentGameBoardSize()
        {
            return m_Board.GetBoardSize();
        }

        private bool updateBoard(int i_Row, int i_Col)
        {
            return m_Board.UpdateCell(i_Row, i_Col, m_CurrentPlayer.GetPlayerSign());
        }

        public void PlayUserTurn(int i_Row, int i_Col)
        {
            bool isTurnPlayed = updateBoard(i_Row, i_Col);

            if(isTurnPlayed)
            {
                checkAndHandleEndOfTurn(i_Row, i_Col);
            }
        }

        public void PlayComputerTurn()
        {
            bool isTurnPlayed = false;
            int row = 0;
            int col = 0;

            while (!isTurnPlayed)
            {
                row = new Random().Next(0, (m_Board.GetBoardSize() - 1));
                col = new Random().Next(0, (m_Board.GetBoardSize() - 1));
                isTurnPlayed = updateBoard(row, col);
            }

            checkAndHandleEndOfTurn(row, col);
        }

        private void checkAndHandleEndOfTurn(int i_Row, int i_Col)
        {
            if (CheckIfWinner(i_Row, i_Col))
            {
                if (m_CurrentPlayer == m_Player1)
                {
                    m_GameState = eGameState.Player2Won;
                    m_Player2.AddScore();
                }
                else
                {
                    m_GameState = eGameState.Player1Won;
                    m_Player1.AddScore();
                }
            }
            else if (CheckIfTie(i_Row, i_Col))
            {
                m_GameState = eGameState.Tie;
            }
            else
            {
                switchPlayer();
            }
        }

        private void switchPlayer()
        {
            m_CurrentPlayer = m_CurrentPlayer == m_Player1 ? m_Player2 : m_Player1; 
        }

        public bool CheckIfWinner(int i_Row, int i_Col)
        {
            eCellSign currentSign = m_CurrentPlayer.GetPlayerSign();

            return checkRowSequence(i_Row, currentSign) ||
                   checkColumnSequence(i_Col, currentSign) ||
                   checkMainDiagonalSequence(currentSign) ||
                   checkSecondaryDiagonalSequence(currentSign);
        }

        public bool CheckIfTie(int i_Row, int i_Col)
        {
            return m_Board.IsBoardFull() && !CheckIfWinner(i_Row, i_Col);
        }

        private bool checkRowSequence(int i_Row, eCellSign i_Sign)
        {
            bool isRowSequence = true;

            for(int col = 0; col < m_Board.GetBoardSize(); col++)
            {
                if(m_Board.GetMatrix()[i_Row][col] != i_Sign)
                {
                    isRowSequence = false;
                    break;
                }
            }

            return isRowSequence;
        }

        private bool checkColumnSequence(int i_Col, eCellSign i_Sign)
        {
            bool isColumnSequence = true;

            for(int row = 0; row < m_Board.GetBoardSize(); row++)
            {
                if(m_Board.GetMatrix()[row][i_Col] != i_Sign)
                {
                    isColumnSequence = false;
                    break;
                }
            }

            return isColumnSequence;
        }

        private bool checkMainDiagonalSequence(eCellSign i_Sign)
        { 
            bool isMainDiagonalSequence = true; 

            for(int i = 0; i < m_Board.GetBoardSize(); i++)
            {
                if(m_Board.GetMatrix()[i][i] != i_Sign)
                {
                    isMainDiagonalSequence = false;
                    break;
                }
            }

            return isMainDiagonalSequence;
        }

        private bool checkSecondaryDiagonalSequence(eCellSign i_Sign)
        {
            bool isSecondaryDiagonalSequence = true;

            for(int i = 0; i < m_Board.GetBoardSize(); i++)
            {
                if(m_Board.GetMatrix()[i][m_Board.GetBoardSize() - 1 - i] != i_Sign)
                {
                    isSecondaryDiagonalSequence = false;
                    break;
                }
            }

            return isSecondaryDiagonalSequence;
        }

        public int[] GetAllPlayersScore()
        {
            int[] playersScore = new int[2];

            playersScore[0] = m_Player1.GetPlayerScore();
            playersScore[1] = m_Player2.GetPlayerScore();

            return playersScore;
        }


    }
}
