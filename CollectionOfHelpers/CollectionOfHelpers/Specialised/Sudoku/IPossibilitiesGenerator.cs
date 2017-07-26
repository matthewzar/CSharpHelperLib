using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionOfHelpersTests.Sudoku
{
    public interface IPossibilitiesGenerator
    {
        IEnumerable<int> EnumeratePossibilities();
    }
}
