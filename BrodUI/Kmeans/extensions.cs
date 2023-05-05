using System;

namespace KmeansAlgorithm
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

        public static int RandomInt(int max)
        {
            return new Random().Next(max);
        }
    }
}