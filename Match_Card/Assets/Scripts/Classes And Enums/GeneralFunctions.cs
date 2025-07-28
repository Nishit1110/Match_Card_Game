using System;
using System.Text;
using UnityEngine;

namespace Nishit.Class
{
    public static class MathFunctions
    {
        public static Vector2Int GetBestGridLayout(int pairCount)
        {
            int totalCards = pairCount * 2;

            int bestRows = 1;
            int bestCols = totalCards;

            int minDifference = totalCards;

            for (int rows = 1; rows <= Mathf.Sqrt(totalCards); rows++)
            {
                if (totalCards % rows == 0)
                {
                    int cols = totalCards / rows;
                    int diff = Mathf.Abs(cols - rows);

                    if (diff < minDifference)
                    {
                        bestRows = rows;
                        bestCols = cols;
                        minDifference = diff;
                    }
                }
            }

            return new Vector2Int(bestRows, bestCols); // or (cols, rows) if you want horizontal emphasis
        }
    }

    public static class StringBuilderPool
    {
        [ThreadStatic]
        private static StringBuilder cachedBuilder;

        public static StringBuilder Get()
        {
            if (cachedBuilder == null)
                cachedBuilder = new StringBuilder(128); // initial capacity

            cachedBuilder.Clear();
            return cachedBuilder;
        }

        public static string Release()
        {
            return cachedBuilder.ToString();
        }
    }
}
