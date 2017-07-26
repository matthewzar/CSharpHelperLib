using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CollectionOfHelpers.ImageProcessing
{
    public class ImagesToGridGenerator : IDisposable
    {
        private Image[] ImageForUseInGrid;

        public int TotalRows;
        public int TotalColumns;
        public int TargetWidth;
        public int TargetHeight;
        
        public ImagesToGridGenerator(string[] bitmapImageFiles)
        {
            var listOfLoadedImages = new List<Image>(bitmapImageFiles.Length);
            foreach (var file in bitmapImageFiles)
            {
                listOfLoadedImages.Add(Image.FromFile(file));
            }
            ImageForUseInGrid = listOfLoadedImages.ToArray();
        }

        public Image GetLoadedImageAsGrid()
        {
            ThrowExceptionIfTooFewImages(TotalColumns, TotalRows);
            Image blankImage = new Bitmap(TargetWidth,TargetHeight);
            var singleCellDrawSize = new Rectangle(0,0,TargetWidth/TotalColumns, TargetHeight / TotalRows);
            using (var canvas = Graphics.FromImage(blankImage))
            {
                DrawAllImageToCanvas(singleCellDrawSize, canvas);
            }
            return blankImage;
        }

        private void DrawAllImageToCanvas(Rectangle singleCellDrawSize, Graphics canvas)
        {
            var imgNum = 0;
            for (var i = 0; i < TotalRows; i++)
            {
                for (var j = 0; j < TotalColumns; j++)
                {
                    var temp = singleCellDrawSize;
                    temp.X = j*temp.Width;
                    temp.Y = i*temp.Height;
                    canvas.DrawImage(ImageForUseInGrid[imgNum++], temp);
                }
            }
        }

        /// <summary>
        /// If left undisposed all images loaded will remain loaded up.
        /// </summary>
        public void Dispose()
        {
            foreach (var img in ImageForUseInGrid)
            {
                img.Dispose();
            }
            ImageForUseInGrid = null;
        }

        private void ThrowExceptionIfTooFewImages(int columns, int rows)
        {
            if (columns*rows < ImageForUseInGrid.Length)
            {
                throw new ArgumentOutOfRangeException(
                    $"Only {ImageForUseInGrid.Length} images loaded, not enough for a {columns}X{rows} grid (min required: {columns*rows})");
            }
        }
    }
}
