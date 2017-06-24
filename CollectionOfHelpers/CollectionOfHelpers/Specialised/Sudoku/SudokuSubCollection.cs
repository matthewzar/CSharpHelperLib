using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionOfHelpers.Specialised.Sudoku
{
    public class SudokuSubCollection
    {
        private SudokuCell[] cells;

        public SudokuSubCollection(int length)
        {
            cells = new SudokuCell[length];
            for (int i = 0; i < length; i++)
            {
                cells[i] = new SudokuCell(length);
            }
        }

        public void AssignValue(int position, int value)
        {
            cells[position].AssignValue(value);
            RemovePossibility(value);
        }

        public void RemovePossibility(int value)
        {
            foreach (var cell in cells)
            {
                cell.RemovePossibility(value);
            }
        }

        //Set as obsolete becuase it exposes too much of the puzzles under-workings
        public SudokuCell this[int index]
        {
            get => cells[index];
            set => cells[index] = value;
        }
        
        public override string ToString()
        {
            return string.Join<ValueLimitedPuzzleCell>(",", cells);
        }
    }
}