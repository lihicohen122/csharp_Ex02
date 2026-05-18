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

        public int BoardSize
        {
            get
            {
                return r_BoardSize;
            }
        }

        public int NumberOfEmptyCells
        {
            get
            {
                return m_EmptyCellCount;
            }
        }

        public eCellSign GetCell(int i_Row, int i_Column)
        {
            return r_Matrix[i_Row, i_Column];
        }

        public bool UpdateCell(int i_Row, int i_Column, eCellSign i_Sign)
        {
            bool isCellUpdateable = isCellValid(i_Row, i_Column);

            if(isCellUpdateable)
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
            for(int i = 0; i < r_BoardSize; ++i)
            {
                for(int j = 0; j < r_BoardSize; ++j)
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

        public bool CheckWinningSequence(int i_Row, int i_Column, eCellSign i_Sign)
        {
            return checkRowSequence(i_Row, i_Sign) || checkColumnSequence(i_Column, i_Sign) ||
                   checkMainDiagonalSequence(i_Sign) || checkSecondaryDiagonalSequence(i_Sign);
        }

        private bool checkRowSequence(int i_Row, eCellSign i_Sign)
        {
            bool isRowSequence = true;

            for(int column = 0; column < r_BoardSize; ++column)
            {
                if(r_Matrix[i_Row, column] != i_Sign)
                {
                    isRowSequence = false;
                    break;
                }
            }

            return isRowSequence;
        }

        private bool checkColumnSequence(int i_Column, eCellSign i_Sign)
        {
            bool isColumnSequence = true;

            for(int row = 0; row < r_BoardSize; ++row)
            {
                if(r_Matrix[row, i_Column] != i_Sign)
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

            for(int i = 0; i < r_BoardSize; ++i)
            {
                if(r_Matrix[i, i] != i_Sign)
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

            for(int i = 0; i < r_BoardSize; ++i)
            {
                if(r_Matrix[i, r_BoardSize - 1 - i] != i_Sign)
                {
                    isSecondaryDiagonalSequence = false;
                    break;
                }
            }

            return isSecondaryDiagonalSequence;
        }

        public Board CloneBoard()
        { ;
            Board clonedBoard = new Board(r_BoardSize);

            for(int row = 0; row < r_BoardSize; ++row)
            {
                for(int column = 0; column < r_BoardSize; ++column)
                {
                    eCellSign currentSign = GetCell(row, column);
                    if(currentSign != eCellSign.Empty)
                    {
                        clonedBoard.UpdateCell(row, column, currentSign);
                    }
                }
            }

            return clonedBoard;
        }

        public void RestoreState(Board i_OriginalBoard)
        {
            for(int row = 0; row < r_BoardSize; ++row)
            {
                for(int column = 0; column < r_BoardSize; ++column)
                {
                    r_Matrix[row, column] = i_OriginalBoard.GetCell(row, column);
                }
            }

            m_EmptyCellCount = i_OriginalBoard.NumberOfEmptyCells;
        }
    }
}