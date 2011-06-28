using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine
{
    /// <summary>
    /// Static methods to assist with random-number generation.
    /// </summary>
    public static class RandomMath
    {

        #region Random Singleton

        /// <summary>
        /// The Random object used for all of the random calls.
        /// </summary>
        private static Random random = new Random();
        public static Random Random
        {
            get { return random; }
        }

        #endregion // Random Singleton


        #region Single Variations

        /// <summary>
        /// Generate a random floating-point value between the minimum and 
        /// maximum values provided.
        /// </summary>
        /// <remarks>This is similar to the Random.Next method, substituting singles
        /// for integers.</remarks>
        /// <param name="minimum">The minimum value.</param>
        /// <param name="maximum">The maximum value.</param>
        /// <returns>A random floating-point value between the minimum and maximum v
        /// alues provided.</returns>
        public static float RandomBetween(float minimum, float maximum)
        {
            return minimum + (float)random.NextDouble() * (maximum - minimum);
        }


        #endregion // Single Variations


        #region Direction Generation

        /// <summary>
        /// Generate a random direction vector.
        /// </summary>
        /// <returns>A random direction vector in 2D space.</returns>
        public static Vector2 RandomDirection()
        {
            float angle = RandomBetween(0, MathHelper.TwoPi);
            return new Vector2((float)Math.Cos(angle),
                (float)Math.Sin(angle));
        }


        /// <summary>
        /// Generate a random direction vector within constraints.
        /// </summary>
        /// <param name="minimumAngle">The minimum angle.</param>
        /// <param name="maximumAngle">The maximum angle.</param>
        /// <returns>
        /// A random direction vector in 2D space, within the constraints.
        /// </returns>
        public static Vector2 RandomDirection(float minimumAngle, float maximumAngle)
        {
            float angle = RandomBetween(MathHelper.ToRadians(minimumAngle),
                MathHelper.ToRadians(maximumAngle)) - MathHelper.PiOver2;
            return new Vector2((float)Math.Cos(angle),
                (float)Math.Sin(angle));
        }


        #endregion // Direction Generation


        #region Shuffle List

        public static IList<T> Shuffle<T>(IList<T> list)
        {
            var rng = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }

        #endregion // Shuffle List

    }
}
