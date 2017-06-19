using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectionOfHelpersTests.Sudoku;

namespace CollectionOfHelpers.Specialised.Sudoku
{
    public abstract class ValueLimitedPuzzleCell
    {
        private const int START_VALUE = -1;
        public bool IsAssigned => Value != START_VALUE;
        public int Value { get; protected set; }
        public List<int> PossibleValues;

        protected ValueLimitedPuzzleCell(IPossibilitiesGenerator generator)
        {
            Value = START_VALUE;
            PossibleValues = generator.EnumeratePossibilities().ToList();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public void AssignValue(int value)
        {
            if (value <= 0 || !PossibleValues.Contains(value))
            {
                throw new ArgumentOutOfRangeException();
            }

            PossibleValues.Remove(value);
            Value = value;
        }
    }
}