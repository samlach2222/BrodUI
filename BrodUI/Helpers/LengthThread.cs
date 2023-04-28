using System;


// Length are calculated in cm
// A square is 0.5cm x 0.5cm

namespace BrodUI.Helpers
{
    internal class LengthThread
    {
        /// <summary>
        /// Length of yarn needed to make a knot at the beginning and at the end of the yarn use
        /// </summary>
        private const double Knot = 2;

        /// <summary>
        /// TODO : MISSING DOCUMENTATION
        /// </summary>
        public double Square = Math.Sqrt(0.5);

        /// <summary>
        /// Length of wire needed to make a cross
        /// </summary>
        private double Cross { get; set; }

        /// <summary>
        /// Total length of the thread
        /// </summary>
        public double TotalLength { get; set; }

        /// <summary>
        /// TODO : MISSING DOCUMENTATION
        /// </summary>
        /// <param name="color">TODO : MISSING DOCUMENTATION</param>
        /// <param name="image">TODO : MISSING DOCUMENTATION</param>
        public LengthThread(int color, int[,] image)
        {

            Cross = (2 * Square) + 1;

            TotalLength = 0;
            TotalLength = WireSize(color, image);

        }

        /// <summary>
        /// TODO : MISSING DOCUMENTATION
        /// </summary>
        /// <param name="color">TODO : MISSING DOCUMENTATION</param>
        /// <param name="image">TODO : MISSING DOCUMENTATION</param>
        public double WireSize(int color, int[,] image)
        {
            // Counter to know the number of consecutive pixels of the same color
            int length = 1;

            // We look on the image at the pixels where the color is present
            for (int i = 0; i < image.GetLongLength(1); i++)
            {
                for (int j = 0; j < image.GetLongLength(2); j++)
                {
                    // If the pixel is not of the color
                    if (image[i, j] != color) continue;
                    // If the neighboring pixel is of the color
                    if (image[i, j + 1] == color)
                        length += length;

                    // Once we have no more consecutive pixels of the color : total length calculation and length reset to 1
                    else
                    {
                        TotalLength = TotalLength + (Cross * length) + (2 * Knot);
                        length = 1;
                    }
                }
            }
            return TotalLength;
        }

    }

}



