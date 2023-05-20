using System;


// Length are calculated in cm
// A square is 0.5cm x 0.5cm

namespace BrodUI.Helpers
{
    /// <summary>
    /// Class to calculate the length of on thread for an image based on its color
    /// </summary>
    public class LengthThread
    {
        /// <summary>
        /// Length of yarn needed to make a knot at the beginning and at the end of the yarn use
        /// </summary>
        public const double Knot = 2;

        /// <summary>
        /// Length of thread needed to make a cross 
        /// </summary>
        public static readonly double Cross = (2 * Math.Sqrt(0.5)) + 1;

        /// <summary>
        /// Total length of the thread
        /// </summary>
        public double TotalLength { get; set; }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="color">Color of the thread of which we are measuring the length</param>
        /// <param name="image">Image we are working on</param>
        public LengthThread(int color, int[,] image)
        {
            TotalLength = 0;
            TotalLength = WireSize(color, image);
        }

        /// <summary>
        /// Function to calculate the length of the thread which color is defined in the parameters as well as the image we are working on
        /// </summary>
        /// <param name="color">Color of the thread of which we are measuring the length</param>
        /// <param name="image">Image we are working on</param>
        /// <returns>Size of the wire</returns>
        public double WireSize(int color, int[,] image)
        {
            // Counter to know the number of consecutive pixels of the same color
            int length = 1;

            // We look on the image at the pixels where the color is present
            for (int i = 0; i < image.GetLongLength(0); i++)
            {
                for (int j = 0; j < image.GetLongLength(1); j++)
                {
                    // If the pixel is not of the color
                    if (image[i, j] != color) continue;

                    if (j < image.GetLongLength(1) - 1)
                    {
                        // If the neighboring pixel is of the color
                        if (image[i, j + 1] == color)
                            length += 1;

                        // Once we have no more consecutive pixels of the color : total length calculation and length reset to 1
                        else
                        {
                            TotalLength = TotalLength + (Cross * length) + (2 * Knot);
                            length = 1;
                        }
                    }

                    if (j != image.GetLongLength(1) - 1) continue;
                    TotalLength = TotalLength + (Cross * length) + (2 * Knot);
                    length = 1;
                }
            }
            return TotalLength;
        }
    }
}