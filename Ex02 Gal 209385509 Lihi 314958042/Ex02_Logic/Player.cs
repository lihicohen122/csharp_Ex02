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
        private int Score { get; }
        private eCellSign Sign { get;}
        private bool IsComputer { get;}
        private string Team { get;}

        public Player(eCellSign i_Sign, bool i_IsComputer, string i_Team)
        {
            Sign = i_Sign;
            IsComputer = i_IsComputer;
            Team = i_Team;
            Score = 0;
        }
    }
}
