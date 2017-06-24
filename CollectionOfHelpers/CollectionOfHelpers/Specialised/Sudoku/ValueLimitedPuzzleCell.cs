using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectionOfHelpersTests.Sudoku;

namespace CollectionOfHelpers.Specialised.Sudoku
{
    /// <summary>
    /// A single cell in a numeric puzzles, tracks the chosen value
    /// the possible values, and eventually the correct value.
    /// </summary>
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
            if (IsAssigned || !PossibleValues.Contains(value))
            {
                throw new ArgumentOutOfRangeException("You tried to assign an impossible value");
            }

            PossibleValues = new List<int> { value };
            Value = value;
        }
        
        public void RemovePossibility(int value)
        {
            if (IsAssigned) return;
            PossibleValues.Remove(value);
        }
    }
}