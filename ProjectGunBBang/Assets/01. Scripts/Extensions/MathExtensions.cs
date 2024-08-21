using System;
using UnityEngine;

namespace GB.Extensions
{
    public static class MathExtensions
    {
        /// <summary>
        /// callback?.Invoke(digit(1, 10, 100), digit_number(523 => 3, 2, 5), index_total);
        /// </summary>
        public static void ForEachDigit(this int source, Action<int, int, int> callback)
        {
            if(source == 0)
                return;

            source = Mathf.Abs(source);

            int i = 0;
            int origin = source;
            int length = Mathf.FloorToInt(Mathf.Log10(origin));
            int cursor = (int)MathF.Pow(10, length);
            while (cursor > 0)
            {
                int number = origin / cursor;
                for (int j = 0; j < number; j++, i++)
                    callback?.Invoke(cursor, number, i);

                origin %= cursor;
                cursor /= 10;
            }
        }

        /// <summary>
        /// callback?.Invoke(digit(1, 10, 100), digit_number(523 => 3, 2, 5), index_total);
        /// </summary>
        public static void ForEachDigit(this ushort source, Action<ushort, ushort, int> callback)
        {
            if (source == 0)
                return;

            source = (ushort)Mathf.Abs(source);

            int i = 0;
            ushort origin = source;
            int length = Mathf.FloorToInt(Mathf.Log10(origin));
            ushort cursor = (ushort)MathF.Pow(10, length);
            while (cursor > 0)
            {
                ushort number = (ushort)(origin / cursor);
                for (int j = 0; j < number; j++, i++)
                    callback?.Invoke(cursor, number, i);

                origin %= cursor;
                cursor /= 10;
            }
        }

        public static float ArithmeticSequence(float n, float start, float diff) => start + (n - 1) * diff;
    }
}