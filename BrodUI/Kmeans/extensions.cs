using System;

namespace BrodUI.KMeans
{
    /// <summary>
    /// Extension class
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Times loop
        /// </summary>
        /// <param name="i">counter of the loop</param>
        /// <param name="action">action to do during the loop</param>
        public static void Times(this int i, Action action)
        {
            for (int j = 0; j < i; j++)
            {
                action();
            }
        }
    }
}