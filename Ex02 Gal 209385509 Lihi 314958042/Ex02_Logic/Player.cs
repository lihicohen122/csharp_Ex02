using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex02_Logic.Enums;

namespace Ex02_Logic
{
    internal class Player
    {
        private int m_Score;
        private eCellSign m_Sign;
        private bool m_IsComputer;

        public Player(eCellSign i_Sign, bool i_IsComputer)
        {
            m_Sign = i_Sign;
            m_IsComputer = i_IsComputer;
            m_Score = 0;
        }

        public eCellSign GetPlayerSign()
        {
            return m_Sign;
        }

        public bool IsPlayerComputer()
        {
            return m_IsComputer;
        }

        public int AddScore()
        {
            return ++m_Score;
        }


    }
}
