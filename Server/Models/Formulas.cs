using System;

namespace Server.Models
{
    internal static class Formulas
    {
        private static readonly double[] StrengthCompute = { 0.646, 0.647, 0.657 };
        private static readonly double[] DextetyCompute = { 0.409, 0.419, 0.429 };
        private static readonly double[] EnduranceCompute = { 0.430, 0.440, 0.450 };

        private static readonly Random random = new Random();

        public static double GetStrengthBonus(int strength)
            => Math.Floor(strength * StrengthCompute[random.Next(0, StrengthCompute.Length - 1)]);
        public static double GetDextityBonus(int dextity)
            => Math.Floor(dextity * DextetyCompute[random.Next(0, DextetyCompute.Length - 1)]);
        public static double GetEnduranceBonus(int endurance)
            => Math.Floor(endurance * EnduranceCompute[random.Next(0, EnduranceCompute.Length - 1)]);
    }
}