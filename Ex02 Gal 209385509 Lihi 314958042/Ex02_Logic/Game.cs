using System;
using Ex02_Logic.Enums;

namespace Ex02_Logic
{
    public class Game
    {
        private Board m_Board;
        private Player m_Player1;
        private Player m_Player2;
        private Player m_CurrentPlayer;
        private eGameState m_GameState;

        public Game(int i_BoardSize, bool i_IsPlayerComputer)
        {
            m_Board = new Board(i_BoardSize);
            m_Player1 = new Player(eCellSign.Cross, false);
            m_Player2 = new Player(eCellSign.Circle, i_IsPlayerComputer);
            m_CurrentPlayer = m_Player1;
            m_GameState = eGameState.NotInitialized;
        }

        public void StartNewGame()
        {
            clearGameBoard();
            m_GameState = eGameState.Playing;
            m_CurrentPlayer = m_Player1;
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

        public bool IsPlayerComputer()
        {
            return m_CurrentPlayer.IsPlayerComputer();
        }

        public bool IsCurrentPlayerComputer()
        {
            return m_CurrentPlayer.IsPlayerComputer();
        }

        public eCellSign GetCellSign(int i_Row, int i_Column)
        {
            return m_Board.GetCell(i_Row, i_Column);
        }

        public int GetCurrentGameBoardSize()
        {
            return m_Board.GetBoardSize();
        }

        private bool updateBoard(int i_Row, int i_Column)
        {
            return m_Board.UpdateCell(i_Row, i_Column, m_CurrentPlayer.GetPlayerSign());
        }

        public void PlayUserTurn(int i_Row, int i_Column)
        {
            bool isTurnPlayed = updateBoard(i_Row, i_Column);

            if(isTurnPlayed)
            {
                checkAndHandleEndOfTurn(i_Row, i_Column);
            }
        }

        public void PlayComputerTurn()
        {
            bool isTurnPlayed = false;
            int row = 0;
            int column = 0;

            while(!isTurnPlayed)
            {
                row = new Random().Next(0, m_Board.GetBoardSize());
                column = new Random().Next(0, m_Board.GetBoardSize());
                isTurnPlayed = updateBoard(row, column);
            }

            checkAndHandleEndOfTurn(row, column);
        }

        private void checkAndHandleEndOfTurn(int i_Row, int i_Column)
        {
            if(checkIfWinner(i_Row, i_Column))
            {
                if(m_CurrentPlayer == m_Player1)
                {
                    m_GameState = eGameState.Player2Won;
                    m_Player2.IncrementPlayerScore();
                }
                else
                {
                    m_GameState = eGameState.Player1Won;
                    m_Player1.IncrementPlayerScore();
                }
            }
            else if(checkIfTie(i_Row, i_Column))
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

        private bool checkIfWinner(int i_Row, int i_Column)
        {
            eCellSign currentSign = m_CurrentPlayer.GetPlayerSign();

            return m_Board.CheckWinningSequence(i_Row, i_Column, currentSign);
        }

        private bool checkIfTie(int i_Row, int i_Column)
        {
            return m_Board.IsBoardFull() && !checkIfWinner(i_Row, i_Column);
        }

        public int[] GetAllPlayersScore()
        {
            int[] playersScore = new int[2];

            playersScore[0] = m_Player1.GetPlayerScore();
            playersScore[1] = m_Player2.GetPlayerScore();

            return playersScore;
        }

        private void clearGameBoard()
        {
            m_Board.ClearBoard();
        }
    }
}