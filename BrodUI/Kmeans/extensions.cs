using System;

namespace BrodUI.Kmeans
{
    public static class Extensions
    {
        public static void Times(this int i, Action action)
        {
            for (var j = 0; j < i; j++)
            {
                action();
            }
        }
    }
}