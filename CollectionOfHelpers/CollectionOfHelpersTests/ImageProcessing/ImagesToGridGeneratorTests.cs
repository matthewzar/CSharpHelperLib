using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Drawing;
using System.IO;
using CollectionOfHelpers.ImageProcessing;

namespace CollectionOfHelpersTests.ImageProcessing
{
    [TestFixture]
    public class ImagesToGridGeneratorTests
    {
        private static void CreateSinglePixelBitmapFile(Color colour, string saveFileName)
        {
            Color testColour = colour;
            Bitmap img = new Bitmap(1, 1);
            img.SetPixel(0, 0, testColour);
            img.Save(saveFileName);
        }

        [TestCase]
        public void TestSingleImageTo1X1Grid()
        {
            ////arrange
            //create a testing image
            var testFileName = "testSingleImageTo1X1Grid.png";
            var testColour = Color.Red;
            try
            {
                CreateSinglePixelBitmapFile(testColour, testFileName);

                ////act
                var sut = new ImagesToGridGenerator(new[] {testFileName})
                {
                    TargetWidth = 1,
                    TargetHeight = 1,
                    TotalColumns = 1,
                    TotalRows = 1
                };

                using (var actualBmp = new Bitmap(sut.GetLoadedImageAsGrid()))
                {
                    sut.Dispose();

                    ////assert
                    Assert.AreEqual(1, actualBmp.Width);
                    Assert.AreEqual(1, actualBmp.Height);
                    AssertRGBValuesMatch(testColour, actualBmp.GetPixel(0, 0));
                }
            }
            finally
            {
                //cleanup
                if (File.Exists(testFileName))
                {
                    File.Delete(testFileName);
                }
            }
        }

        [TestCase]
        public void TestDoubleImageTo2X1Grid()
        {
            ////arrange
            //create a testing image
            var testFile1Name = "testSingleImageTo2X1Grid1.png";
            var testFile2Name = "testSingleImageTo2X1Grid2.png";
            var testColour1 = Color.Red;
            var testColour2 = Color.Blue;
            try
            {
                CreateSinglePixelBitmapFile(testColour1, testFile1Name);
                CreateSinglePixelBitmapFile(testColour2, testFile2Name);

                ////act
                var sut = new ImagesToGridGenerator(new[] { testFile1Name, testFile2Name })
                {
                    TargetWidth = 2,
                    TargetHeight = 1,
                    TotalColumns = 2,
                    TotalRows = 1
                };

                using (var actualBmp = new Bitmap(sut.GetLoadedImageAsGrid()))
                {
                    sut.Dispose();
                    
                    ////assert
                    Assert.AreEqual(2, actualBmp.Width);
                    Assert.AreEqual(1, actualBmp.Height);

                    AssertRGBValuesMatch(testColour1, actualBmp.GetPixel(0, 0));
                    AssertRGBValuesMatch(testColour2, actualBmp.GetPixel(1, 0));
                }
            }
            finally
            {
                //cleanup
                if (File.Exists(testFile1Name))
                {
                    File.Delete(testFile1Name);
                }
                if (File.Exists(testFile2Name))
                {
                    File.Delete(testFile2Name);
                }
            }
        }

        private static void AssertRGBValuesMatch(Color colorExpected, Color colorActual)
        {
            Assert.AreEqual(colorExpected.R, colorActual.R);
            Assert.AreEqual(colorExpected.G, colorActual.G);
            Assert.AreEqual(colorExpected.B, colorActual.B);
        }
    }
}