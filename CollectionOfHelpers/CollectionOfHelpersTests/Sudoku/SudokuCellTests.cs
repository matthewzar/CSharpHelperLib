using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectionOfHelpers.Specialised.Sudoku;
using NUnit.Framework;

namespace CollectionOfHelpersTests.Sudoku
{
    [TestFixture]
    public class SudokuCellTests
    {
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(6)]
        public void sudokuCell_SimpleSetterTest(int value)
        {
            //arrange
            var sut = new SudokuCell(9);

            //act
            sut.AssignValue(value);

            //assert
            Assert.AreEqual(sut.Value, value);
        }

        [TestCase(1)]
        [TestCase(6)]
        [TestCase(9)]
        public void sudokuCell_IsAssignedTest(int value)
        {
            var sut = new SudokuCell(9);

            sut.AssignValue(value);

            Assert.True(sut.IsAssigned);
        }

        [Test]
        public void sudokuCell_NotAssignedByDefaultTest()
        {
            var sut = new SudokuCell(9);

            Assert.False(sut.IsAssigned);
        }

        [Test]
        public void sudokuCell_InvalidValue_ExceptionAndNotAssigned_Test()
        {
            var sut = new SudokuCell(9);

            Assert.Throws(typeof(ArgumentOutOfRangeException), () => { sut.AssignValue(-9); });

            Assert.False(sut.IsAssigned);
        }

        [TestCase(4)]
        [TestCase(59)]
        [TestCase(161)]
        public void sudokuCell_InvalidValue_NewCellHasAllLegalValues_Test(int maxValue)
        {
            var sut = new SudokuCell(maxValue);

            Assert.AreEqual(maxValue, sut.PossibleValues.Count);
            for (int i = 1; i <= maxValue; i++)
            {
                Assert.Contains(i, sut.PossibleValues);
            }
        }

        [TestCase(9)]
        public void sudokuCell_InvalidValue_CantAssignIllegalValue_Test(int maxValue)
        {
            var sut = new SudokuCell(maxValue);

            //Act
            sut.AssignValue(1);

            //Assert
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => { sut.AssignValue(1); });
        }
    }
}