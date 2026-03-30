using System;
using UnityEngine;

namespace Bill.Mutant.Core
{
    public static class MathUtility
    {
        public static float Clamp01(float value)
        {
            return Mathf.Clamp01(value);
        }

        public static bool Approximately(float a, float b, float epsilon = 0.0001f)
        {
            return Mathf.Abs(a - b) <= Mathf.Abs(epsilon);
        }

        public static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            if (Approximately(fromMin, fromMax))
            {
                throw new ArgumentException("fromMin and fromMax cannot be equal.");
            }

            var t = (value - fromMin) / (fromMax - fromMin);
            return Mathf.Lerp(toMin, toMax, t);
        }
    }
}
