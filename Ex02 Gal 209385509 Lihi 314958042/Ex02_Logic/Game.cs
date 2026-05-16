using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex02_Logic.Enums;

namespace Ex02_Logic
{
    internal class Game
    {
        private Board m_Board;
        private Player m_Player1;
        private Player m_Player2;
        private Player m_CurrentPlayer;
        private eGameState m_GameState { get; }

        public Game(int i_BoardSize, bool i_IsPlayerComputer)
        {
            m_Board = new Board(i_BoardSize);
            m_Player1 = new Player(eCellSign.Cross, false);
            m_Player2 = new Player(eCellSign.Circle, i_IsPlayerComputer);
            m_CurrentPlayer = m_Player1;
            m_GameState = eGameState.NotInitialized;
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

        private bool updateBoard(int i_Row, int i_Col)
        {
            return m_Board.UpdateCell(i_Row, i_Col, m_CurrentPlayer.GetPlayerSign());
        }

        public Player PlayUserTurn(int i_Row, int i_Col)
        {
            bool isTurnPlayed = updateBoard(i_Row, i_Col);

            if(isTurnPlayed)
            {
                m_CurrentPlayer = m_CurrentPlayer == m_Player1 ? m_Player1 : m_Player2;
            }

            return m_CurrentPlayer; 
        }

        public Player PlayComputerTurn()
        {
            bool isTurnPlayed = false;

            while(!isTurnPlayed)
            {
                int row = new Random().Next(0, (m_Board.GetBoardSize() - 1));
                int col = new Random().Next(0, (m_Board.GetBoardSize() - 1));
                isTurnPlayed = updateBoard(row, col);
            }

            m_CurrentPlayer = m_CurrentPlayer == m_Player1 ? m_Player1 : m_Player2;
            return m_CurrentPlayer;

        }

        public bool CheckIfWinner()
        {
            return checkRowSequence(0, m_CurrentPlayer.GetPlayerSign()) ||
                   checkColumnSequence(0, m_CurrentPlayer.GetPlayerSign()) ||
                   checkMainDiagonalSequence(m_CurrentPlayer.GetPlayerSign()) ||
                   checkAntiDiagonalSequence(m_CurrentPlayer.GetPlayerSign());
        }

        public bool CheckIfTie()
        {
            return m_Board.IsBoardFull() && !CheckIfWinner();
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

        private bool checkAntiDiagonalSequence(eCellSign i_Sign)
        {
            bool isAntiDiagonalSequence= true;

            for(int i = 0; i < m_Board.GetBoardSize(); i++)
            {
                if(m_Board.GetMatrix()[i][m_Board.GetBoardSize() - 1 - i] != i_Sign)
                {
                    isAntiDiagonalSequence = false;
                    break;
                }
            }

            return isAntiDiagonalSequence;
        }













    }
}
