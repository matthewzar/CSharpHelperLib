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
    public class SudokuInitialisationTests
    {
        [TestCase(4)]
        [TestCase(9)]
        [TestCase(16)]
        [TestCase(25)]
        public void sudokuDistributedGridCollection_ConstructorValidSizeTest(int gridSize)
        {
            //arrange

            //act
            SudokuDistributedGridCollection sut = new SudokuDistributedGridCollection(gridSize);

            //assert
            Assert.NotNull(sut);
            Assert.AreEqual(gridSize, sut.GridSize);
        }

        [TestCase(-9)]
        [TestCase(8)]
        [TestCase(11)]
        public void sudokuDistributedGridCollection_ConstructorInvalidSizeExceptionTest(int gridSize)
        {
            //arrange

            //act

            //assert
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => { new SudokuDistributedGridCollection(gridSize); });
        }

        [TestCase(4)]
        [TestCase(9)]
        [TestCase(16)]
        public void sudokuDistributedGridCollection_ConstructorNonNullGridTest(int gridSize)
        {
            //arrange

            //act
            SudokuDistributedGridCollection sut = new SudokuDistributedGridCollection(gridSize);

            //assert
            Assert.NotNull(sut[0, 0]);
        }

        [TestCase(4)]
        [TestCase(9)]
        [TestCase(16)]
        public void sudokuDistributedGridCollection_SimpleRowVsColumnVsBox_SameReferenceTest(int gridSize)
        {
            //arrange

            //act
            SudokuDistributedGridCollection sut = new SudokuDistributedGridCollection(gridSize);


            //assert
            Assert.AreSame(sut[0, 0], sut.GetRow(0)[0]);
            Assert.AreSame(sut[0, 0], sut.GetColumn(0)[0]);
            Assert.AreSame(sut[0, 0], sut.GetBox(0)[0]);
        }

        [Test]
        public void sudokuDistributedGridCollection_MultipleRowVsColumnVsBox_SameReferenceTest()
        {
            //arrange

            //act
            SudokuDistributedGridCollection sut = new SudokuDistributedGridCollection(9);


            //assert
            for (int i = 0; i < 9; i++)
            {
                Assert.AreSame(sut[i, 0], sut.GetRow(0)[i]);
                Assert.AreSame(sut[0, i], sut.GetColumn(0)[i]);
            }

            //testing box 3 (the lowercase 'x')
            //XXX
            //xXX
            //XXX
            Assert.AreSame(sut[0, 3], sut.GetBox(3)[0]);
            Assert.AreSame(sut[0, 4], sut.GetBox(3)[3]);
            Assert.AreSame(sut[0, 5], sut.GetBox(3)[6]);
            Assert.AreSame(sut[1, 3], sut.GetBox(3)[1]);
        }

        [TestCase(4)]
        [TestCase(9)]
        [TestCase(16)]
        public void sudokuDistributedGridCollection_UnassignedCellsTest(int size)
        {
            //arrange
            int sumOfInteger1ToSize = size*(size + 1)/2;

            //act
            SudokuDistributedGridCollection sut = new SudokuDistributedGridCollection(size);


            //assert
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Assert.False(sut[i,j].IsAssigned);
                    Assert.AreEqual(size, sut[i, j].PossibleValues.Count);
                    Assert.AreEqual(sumOfInteger1ToSize, sut[i, j].PossibleValues.Sum());
                }
            }
        }

        [Test]
        public void sudokuDistributedGridCollection_ToStringOfEmptyPuzzle()
        {
            //arrange
            SudokuDistributedGridCollection sut = new SudokuDistributedGridCollection(4);
            string expected = "-1,-1,-1,-1\n-1,-1,-1,-1\n-1,-1,-1,-1\n-1,-1,-1,-1";
            
            //assert
            Assert.AreEqual(expected, sut.ToString());
        }

        [Test]
        public void sudokuDistributedGridCollection_ToStringOfPartiallyAssignedPuzzle()
        {
            //arrange
            SudokuDistributedGridCollection sut = new SudokuDistributedGridCollection(4);
            string expected = "4,-1,-1,-1\n-1,-1,-1,-1\n-1,-1,-1,-1\n-1,-1,-1,-1";

            //act
            sut[0, 0].AssignValue(4);

            //assert
            Assert.AreEqual(expected, sut.ToString());
        }
    }
}