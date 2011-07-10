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
        private static Random _random = new Random(_seed);
        private static int _seed = GenerateRandomSeed();

        public static Random Random
        {
            get { return _random; }
        }
        public static int CurrentRandomSeed
        {
            get { return _seed; }
        }
        
        public static void SetRandomSeed(int seed)
        {
            _seed = seed;
            _random = new Random(seed);
        }

        // Generates an integer value based on the current DateTime
        public static int GenerateRandomSeed()
        {
            DateTime now = DateTime.Today;
            return (now.Year + now.Month + now.Day + now.Hour + now.Minute + now.Second + now.Millisecond) / 16;
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
            return minimum + (float)_random.NextDouble() * (maximum - minimum);
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

        public static IList<T> Shuffle<T>(IList<T> list, int? seed = null)
        {
            Random rng;
            if (seed == null)
                rng = new Random();
            else
                rng = new Random(seed.Value);

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

    // based upon http://freespace.virgin.net/hugo.elias/models/m_perlin.htm
    public static class PerlinNoise
    {
        // Deligate Methods to allow anyone to create and use different noise and noise Interpolation Methods
        public delegate float NoiseFunction1D(int x, int? seed);
        public delegate float NoiseFunction2D(int x, int y, int? seed);
        public delegate float NoiseInterpolation(float a, float b, float x);
    
        // Generates a 1D Noise Map using persistance, octaves, frequency and amplitude
        public static float[] GenerateNoise(int width,
            float persistance, int octaves, float frequency, float amplitude,
            int? seed = null) // a null seed uses a randomly generated seed
        {
            float[] noise = new float[width];
            if (!seed.HasValue) { seed = RandomMath.GenerateRandomSeed(); }

            for (int x = 0; x < width; ++x)
            {
                noise[x] = PerlinNoise1D(x, persistance, octaves, frequency, amplitude, seed);
            }
            return noise;
        }

        // Generates a 2D Noise Map using persistance, octaves, frequency and amplitude
        public static float[,] GenerateNoise(int width, int height, 
            float persistance, int octaves, float frequency, float amplitude,
            int? seed = null) // a null seed uses a randomly generated seed
        {
            float[,] noise = new float[width, height];
            if (!seed.HasValue) { seed = RandomMath.GenerateRandomSeed(); }
            
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    noise[x, y] = PerlinNoise2D(x, y, persistance, octaves, frequency, amplitude, seed);
                }
            }
            return noise;
        }

        // Returns the Interpolated 1D Noise Value using given x, persistance, octaves, frequency, and amplitude
        private static float PerlinNoise1D(int x, 
            float persistance, int octaves, float frequency, float amplitude,
            int? seed = null)
        {
            float total = 0;
            float p = persistance;
            int n = octaves - 1;
            float freq = frequency;
            float amp = amplitude;

            for (int i = 0; i < n; i++)
            {
                total += InterpolatedNoise1D(x * freq, seed) * amp;
                freq *= 2;  // Math.Pow(2,i);
                amp *= p;   // Math.Pow(p,i);
            }
            return total;
        }

        // Returns the Interpolated 2D Noise Value using given x|y, persistance, octaves, frequency, and amplitude 
        private static float PerlinNoise2D(int x, int y, 
            float persistance, int octaves, float frequency, float amplitude,
            int? seed = null)
        {
            float total = 0;
            float p = persistance;
            int n = octaves - 1;
            float freq = frequency;
            float amp = amplitude;

            for (int i = 0; i < n; i++)
            {
                total += InterpolatedNoise2D((float)x * freq, ((float)y * freq) * amp, seed);
                freq *= 2;  // Math.Pow(2,i);
                amp *= p;   // Math.Pow(p,i);
            }
            return total;
        }

        // Default uses SmoothNoise and CosineInterpolate
        public static float InterpolatedNoise1D(float x, int? seed = null)
        {
            return InterpolatedNoise1D(x, SmoothedNoise1D, CosineInterpolate, seed);
        }

        // Default uses SmoothNoise and CosineInterpolate
        public static float InterpolatedNoise2D(float x, float y, int? seed = null)
        {
            return InterpolatedNoise2D(x, y, SmoothedNoise2D, CosineInterpolate, seed);
        }
        
        public static float InterpolatedNoise1D(float x, 
            NoiseFunction1D noiseFunc, NoiseInterpolation noiseInterp, 
            int? seed = null)
        {
            int ix = (int)x;
            float fx = x - ix;

            float v1 = noiseFunc(ix, seed);
            float v2 = noiseFunc(ix + 1, seed);

            return noiseInterp(v1, v2, fx);
        }

        public static float InterpolatedNoise2D(float x, float y,
            NoiseFunction2D noiseFunc, NoiseInterpolation noiseInterp,
            int? seed = null)
        {
            int ix = (int)x;
            float fx = x - ix;

            int iy = (int)y;
            float fy = y - iy;

            float v1 = noiseFunc(ix, iy, seed);
            float v2 = noiseFunc(ix + 1, iy, seed);
            float v3 = noiseFunc(ix, iy + 1, seed);
            float v4 = noiseFunc(ix + 1, iy + 1, seed);

            float i1 = noiseInterp(v1, v2, fx);
            float i2 = noiseInterp(v3, v4, fx);

            return noiseInterp(i1, i2, fy);
        }


        #region Noise Functions

        // In order for Perlin Noise Random to work, the number returned needs to seem random
        // but always return the same 'random' number when given the same index (and seed) value

        public static float SmoothedNoise1D(int x, int? seed = null)
        {
            return Noise1D(x, seed) / 2 + Noise1D(x - 1, seed) / 4 + Noise1D(x + 1, seed) / 4;
        }

        public static float SmoothedNoise2D(int x, int y, int? seed = null)
        {
            float corners = (
                Noise2D(x - 1, y - 1, seed) + 
                Noise2D(x + 1, y - 1, seed) + 
                Noise2D(x - 1, y + 1, seed) + 
                Noise2D(x + 1, y + 1, seed)
                ) / 16f;
            float sides = (
                Noise2D(x - 1, y, seed) + 
                Noise2D(x + 1, y, seed) + 
                Noise2D(x, y - 1, seed) + 
                Noise2D(x, y + 1, seed)
                ) / 8f;
            float center = Noise2D(x, y, seed) / 4f;

            return corners + sides + center;
        }

        // Base Noise Methods
        public static float Noise1D(int x, int? seed = null)
        {
            if (seed.HasValue) { x += seed.Value; }
            x = (x << 13) ^ x;
            return (float)(1.0 - ((x * (x * x * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);
        }
        public static float Noise2D(int x, int y, int? seed = null)
        {
            return Noise1D(x + y * 8997587, seed);
        }

        #endregion // Noise Functions

        #region Noise Interpolation 

        public static float LinearInterpolate(float a, float b, float x)
        {
            return a * (1 - x) + b * x;
        }

        public static float CosineInterpolate(float a, float b, float x)
        {
            float ft = x * (float)Math.PI;
            float f = (float)(1 - Math.Cos(ft)) * 0.5f;
            return a * (1 - f) + b * f;
        }

        #endregion // Noise Interpolation

    }
}
