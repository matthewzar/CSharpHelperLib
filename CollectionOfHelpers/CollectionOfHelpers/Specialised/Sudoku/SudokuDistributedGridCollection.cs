using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CollectionOfHelpers.Specialised.Sudoku
{
    public class SudokuDistributedGridCollection
    {
        public int GridSize => Columns.Length;
        private int boxSize;

        private SudokuSubCollection[] Columns;
        private SudokuSubCollection[] Rows;
        private SudokuSubCollection[] Boxes;

        public SudokuDistributedGridCollection(int gridSize)
        {
            throwExceptionForInvalidGridSize(gridSize);
            boxSize = (int) Math.Sqrt(gridSize);
            initialiseGrid(gridSize);
        }

        private void throwExceptionForInvalidGridSize(int gridSize)
        {
            if (gridSize < 1 || Math.Sqrt(gridSize)%1 != 0)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private void initialiseGrid(int gridSize)
        {
            initialiseEmptySubCollectionArrays(gridSize);
            populateSubCollectionArrays(gridSize);


            for (var y = 0; y < gridSize; y++)
            {
                for (var x = 0; x < gridSize; x++)
                {
                    mirrorColumnCellIntoRowCollection(x, y);
                    mirrorColumnCellIntoBoxCollection(x, y);
                }
            }
        }

        private void mirrorColumnCellIntoRowCollection(int x, int y)
        {
            Rows[x][y] = Columns[y][x];
        }

        private void mirrorColumnCellIntoBoxCollection(int x, int y)
        {
            var boxNum = convertRowColToBoxNum(x, y);
            var boxOffset = convertRowColToBoxOffset(y, x);
            Boxes[boxNum][boxOffset] = Columns[y][x];
        }

        private void populateSubCollectionArrays(int gridSize)
        {
            for (var x = 0; x < GridSize; x++)
            {
                Columns[x] = new SudokuSubCollection(gridSize);
                Rows[x] = new SudokuSubCollection(gridSize);
                Boxes[x] = new SudokuSubCollection(gridSize);
            }
        }

        private void initialiseEmptySubCollectionArrays(int gridSize)
        {
            Columns = new SudokuSubCollection[gridSize];
            Rows = new SudokuSubCollection[gridSize];
            Boxes = new SudokuSubCollection[gridSize];
        }

        private int convertRowColToBoxNum(int col, int row)
        {
            return row/boxSize + (col/boxSize)*boxSize;
        }

        private int convertRowColToBoxOffset(int col, int row)
        {
            return row%boxSize*boxSize + col%boxSize;
        }

        public SudokuSubCollection GetRow(int rowNumber)
        {
            return Rows[rowNumber];
        }

        public SudokuSubCollection GetColumn(int rowNumber)
        {
            return Columns[rowNumber];
        }

        public SudokuSubCollection GetBox(int rowNumber)
        {
            return Boxes[rowNumber];
        }

        public ValueLimitedPuzzleCell this[int x, int y] => Columns[x][y];

        public void AssignValue(int x, int y, int value)
        {
            Columns[x].AssignValue(y, value);
            Rows[y].RemovePossibility(value);
            var boxNum = convertRowColToBoxNum(x, y);
            Boxes[boxNum].RemovePossibility(value);
        }

        public override string ToString()
        {
            return string.Join<SudokuSubCollection>("\n", Rows);
        }
    }
}