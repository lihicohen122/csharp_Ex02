using Ex02_Logic.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02_Logic
{
    internal class Board
    {
        private readonly eCellSign[][] r_Matrix;
        private int m_BoardSize;
        private int m_EmptyCellCount;

        public Board(int i_Size)
        {
            m_BoardSize = i_Size;
            r_Matrix = new eCellSign[m_BoardSize][];

            for(int i = 0; i < m_BoardSize; i++)
            {
                r_Matrix[i] = new eCellSign[m_BoardSize];
            }

            m_EmptyCellCount = m_BoardSize * m_BoardSize;
        }

        public eCellSign[][] GetMatrix()
        {
            return r_Matrix;
        }

        public int GetBoardSize()
        {
            return m_BoardSize;
        }

        public eCellSign GetCell(int i_Row, int i_Col)
        {
            return r_Matrix[i_Row][i_Col];
        }

        public int GetNumberOfEmptyCells()
        {
            return m_EmptyCellCount;
        }

        public bool UpdateCell(int i_Row, int i_Col, eCellSign i_Sign)
        {
            bool isCellUpdateable = isCellValid(i_Row, i_Col);

            if(isCellUpdateable)
            {
                r_Matrix[i_Row][i_Col] = i_Sign;
                m_EmptyCellCount--;
            }

            return isCellUpdateable;
        }

        private bool isCellEmpty(int i_Row, int i_Col)
        {
            return r_Matrix[i_Row][i_Col] == eCellSign.Empty;
        }

        private bool isCellOutOfBounds(int i_Row, int i_Col)
        {
            bool isOutOfBounds = i_Row < 0 || i_Row >= m_BoardSize || i_Col < 0 || i_Col >= m_BoardSize;
            return isOutOfBounds;
        }

        private bool isCellValid(int i_Row, int i_Col)
        {
            return !isCellOutOfBounds(i_Row, i_Col) && isCellEmpty(i_Row, i_Col);
        }

        public void ClearBoard()
        {
            for(int i = 0; i < m_BoardSize; i++)
            {
                for(int j = 0; j < m_BoardSize; j++)
                {
                    r_Matrix[i][j] = eCellSign.Empty;
                }
            }

            m_EmptyCellCount = m_BoardSize * m_BoardSize;
        }

        public bool IsBoardFull()
        {
            return m_EmptyCellCount == 0;
        }
    }
}
