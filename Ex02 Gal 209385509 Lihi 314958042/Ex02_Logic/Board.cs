using Ex02_Logic.Enums;

namespace Ex02_Logic
{
    internal class Board
    {
        private readonly eCellSign[][] r_Matrix;
        private readonly int r_BoardSize;
        private int m_EmptyCellCount;

        public Board(int i_Size)
        {
            r_BoardSize = i_Size;
            r_Matrix = new eCellSign[r_BoardSize][];
            for(int i = 0; i < r_BoardSize; i++)
            {
                r_Matrix[i] = new eCellSign[r_BoardSize];
            }

            m_EmptyCellCount = r_BoardSize * r_BoardSize;
            ClearBoard();
        }

        public eCellSign[][] GetMatrix()
        {
            return r_Matrix;
        }

        public int GetBoardSize()
        {
            return r_BoardSize;
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
            bool isOutOfBounds = i_Row < 0 || i_Row >= r_BoardSize || i_Col < 0 || i_Col >= r_BoardSize;

            return isOutOfBounds;
        }

        private bool isCellValid(int i_Row, int i_Col)
        {
            return !isCellOutOfBounds(i_Row, i_Col) && isCellEmpty(i_Row, i_Col);
        }

        public void ClearBoard()
        {
            for(int i = 0; i < r_BoardSize; ++i)
            {
                for(int j = 0; j < r_BoardSize; ++j)
                {
                    r_Matrix[i][j] = eCellSign.Empty;
                }
            }

            m_EmptyCellCount = r_BoardSize * r_BoardSize;
        }

        public bool IsBoardFull()
        {
            return m_EmptyCellCount == 0;
        }
    }
}
