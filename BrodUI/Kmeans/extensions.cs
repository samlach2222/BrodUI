using System;

namespace BrodUI.KMeans
{
    public static class Extensions
    {
        public static void Times(this int i, Action action)
        {
            for (int j = 0; j < i; j++)
            {
                action();
            }
        }
    }
}