using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionOfHelpers.Specialised.Sudoku
{
    public class SudokuSubCollection
    {
        private ValueLimitedPuzzleCell[] cells;

        public SudokuSubCollection(int length)
        {
            cells = new ValueLimitedPuzzleCell[length];
            for (int i = 0; i < length; i++)
            {
                cells[i] = new SudokuCell(length);
            }
        }

        public ValueLimitedPuzzleCell this[int index]
        {
            get { return cells[index]; }
            set { cells[index] = value; }
        }

        public override string ToString()
        {
            return string.Join<ValueLimitedPuzzleCell>(",", cells);
        }
    }
}