using Ex02_Logic.Enums;

namespace Ex02_Logic
{
    internal class Player
    {
        private int m_Score;
        private readonly eCellSign r_Sign;
        private readonly bool r_IsComputer;

        public Player(eCellSign i_Sign, bool i_IsComputer)
        {
            r_Sign = i_Sign;
            r_IsComputer = i_IsComputer;
            m_Score = 0;
        }

        public void IncrementPlayerScore()
        {
            ++m_Score;
        }

        public eCellSign PlayerSign
        {
            get
            {
                return r_Sign;
            }
        }

        public int PlayerScore
        {
            get
            {
                return m_Score;
            }
        }

        public bool IsPlayerComputer
        {
            get
            {
                return r_IsComputer;
            }
        }
    }
}
