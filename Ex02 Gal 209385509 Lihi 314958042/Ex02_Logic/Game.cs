using Ex02_Logic.Enums;

namespace Ex02_Logic
{
    public class Game
    {
        private readonly Board r_Board;
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private Player m_CurrentPlayer;
        private eGameState m_GameState;
        private readonly MonteCarloAI r_ComputerAI;

        public eGameState GameState
        {
            get
            {
                return m_GameState;
            }
        }

        public int BoardSize
        {
            get
            {
                return r_Board.BoardSize;
            }
        }

        public bool IsCurrentPlayerComputer
        {
            get
            {
                return m_CurrentPlayer.IsPlayerComputer;
            }
        }

        public Game(int i_BoardSize, bool i_IsOpponentComputer)
        {
            const bool v_IsComputerPlayer = true;

            r_ComputerAI = i_IsOpponentComputer ? new MonteCarloAI() : null;
            r_Board = new Board(i_BoardSize);
            r_Player1 = new Player(eCellSign.Cross, !v_IsComputerPlayer);
            r_Player2 = new Player(eCellSign.Circle, i_IsOpponentComputer);
            m_CurrentPlayer = r_Player1;
        }

        private eCellSign getOtherPlayerSign()
        {
            return m_CurrentPlayer == r_Player1 ? r_Player2.PlayerSign : r_Player1.PlayerSign;
        }

        public void StartNewGame()
        {
            clearGameBoard();
            m_GameState = eGameState.Playing;
            m_CurrentPlayer = r_Player1;
        }

        public void QuitGame()
        {
            m_GameState = eGameState.Quit;
        }

        public eCellSign GetCellSign(int i_Row, int i_Column)
        {
            return r_Board.GetCell(i_Row, i_Column);
        }

        private bool updateBoard(int i_Row, int i_Column)
        {
            return r_Board.UpdateCell(i_Row, i_Column, m_CurrentPlayer.PlayerSign);
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
            r_ComputerAI.StartMonteCarloTreeSearchAlgorithm(r_Board, m_CurrentPlayer.PlayerSign, getOtherPlayerSign(), out int bestRow, out int bestColumn);
            updateBoard(bestRow, bestColumn);
            checkAndHandleEndOfTurn(bestRow, bestColumn);
        }

        private void checkAndHandleEndOfTurn(int i_Row, int i_Column)
        {
            if(checkIfWinner(i_Row, i_Column))
            {
                if(m_CurrentPlayer == r_Player1)
                {
                    m_GameState = eGameState.Player2Won;
                    r_Player2.IncrementPlayerScore();
                }
                else
                {
                    m_GameState = eGameState.Player1Won;
                    r_Player1.IncrementPlayerScore();
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
            m_CurrentPlayer = m_CurrentPlayer == r_Player1 ? r_Player2 : r_Player1; 
        }

        private bool checkIfWinner(int i_Row, int i_Column)
        {
            eCellSign currentSign = m_CurrentPlayer.PlayerSign;

            return r_Board.CheckWinningSequence(i_Row, i_Column, currentSign);
        }

        private bool checkIfTie(int i_Row, int i_Column)
        {
            return r_Board.IsBoardFull() && !checkIfWinner(i_Row, i_Column);
        }

        public int[] GetAllPlayersScore()
        {
            int[] playersScore = new int[2];

            playersScore[0] = r_Player1.PlayerScore;
            playersScore[1] = r_Player2.PlayerScore;

            return playersScore;
        }

        private void clearGameBoard()
        {
            r_Board.ClearBoard();
        }
    }
}