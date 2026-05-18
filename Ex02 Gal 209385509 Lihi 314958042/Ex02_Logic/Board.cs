using Ex02_Logic.Enums;

namespace Ex02_Logic
{
    internal class Board
    {
        private readonly eCellSign[,] r_Matrix;
        private readonly int r_BoardSize;
        private readonly int r_TotalNumOfCells;
        private int m_EmptyCellCount;

        public Board(int i_Size)
        {
            r_BoardSize = i_Size;
            r_Matrix = new eCellSign[r_BoardSize, r_BoardSize];

            r_TotalNumOfCells = r_BoardSize * r_BoardSize;
            ClearBoard();
        }

        public eCellSign[,] GetMatrix()
        {
            return r_Matrix;
        }

        public int GetBoardSize()
        {
            return r_BoardSize;
        }

        public eCellSign GetCell(int i_Row, int i_Column)
        {
            return r_Matrix[i_Row, i_Column];
        }

        public int GetNumberOfEmptyCells()
        {
            return m_EmptyCellCount;
        }

        public bool UpdateCell(int i_Row, int i_Column, eCellSign i_Sign)
        {
            bool isCellUpdateable = isCellValid(i_Row, i_Column);

            if (isCellUpdateable)
            {
                r_Matrix[i_Row, i_Column] = i_Sign;
                m_EmptyCellCount--;
            }

            return isCellUpdateable;
        }

        private bool isCellEmpty(int i_Row, int i_Column)
        {
            return r_Matrix[i_Row, i_Column] == eCellSign.Empty;
        }

        private bool isCellOutOfBounds(int i_Row, int i_Column)
        {
            return i_Row < 0 || i_Row >= r_BoardSize || i_Column < 0 || i_Column >= r_BoardSize;
        }

        private bool isCellValid(int i_Row, int i_Column)
        {
            return !isCellOutOfBounds(i_Row, i_Column) && isCellEmpty(i_Row, i_Column);
        }

        public void ClearBoard()
        {
            for (int i = 0; i < r_BoardSize; ++i)
            {
                for (int j = 0; j < r_BoardSize; ++j)
                {
                    r_Matrix[i, j] = eCellSign.Empty;
                }
            }

            m_EmptyCellCount = r_TotalNumOfCells;
        }

        public bool IsBoardFull()
        {
            return m_EmptyCellCount == 0;
        }
    }
}