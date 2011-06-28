using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine.Animations
{
    public static class ProgressTransforms
    {
        public static Func<float, float> Accelerate(float power)
        {
            return (p) => (float)Math.Pow(p, power);
        }

        public static Func<float, float> Decelerate(float power)
        {
            return (p) => 1.0f - (float)Math.Pow(1.0f - p, power);
        }

        public static float Snake(float progress)
        {
            return (1.0f - ((float)Math.Cos(progress * Math.PI) / 2.0f + 0.5f));
        }

        public static Func<float, float> Hold(float beginRatio, Func<float, float> transform, float endRatio)
        {
            return (p) =>
            {
                if (p < beginRatio)
                {
                    return transform(0.0f);
                }
                else if (p > 1.0f - endRatio)
                {
                    return transform(1.0f);
                }
                else
                {
                    float transformRatio = 1.0f - beginRatio - endRatio;
                    return transform((p - beginRatio) / transformRatio);
                }
            };
        }

        public static Func<float, float> Snake(int length)
        {
            return Replicate(length, Snake);
        }

        public static Func<float, float> Replicate(int count, Func<float, float> transform)
        {
            float scale = 1.0f / (float)count;
            return (p) =>
            {
                int seg;
                float prog;
                Chunk(p, scale, out seg, out prog);
                return seg * scale + scale * transform(prog);
            };
        }

        // Penner bounce
        public static float Bounce(float pos)
        {
            if (pos < (1f / 2.75f))
            {
                return (7.5625f * pos * pos);
            }
            else if (pos < (2f / 2.75f))
            {
                return (7.5625f * (pos -= (1.5f / 2.75f)) * pos + .75f);
            }
            else if (pos < (2.5f / 2.75f))
            {
                return (7.5625f * (pos -= (2.25f / 2.75f)) * pos + .9375f);
            }
            else
            {
                return (7.5625f * (pos -= (2.625f / 2.75f)) * pos + .984375f);
            }
        }

        public static Func<float, float> Elastic(int numCycles, float amplitude)
        {
            return (f) =>
            {
                return f + (float)Math.Pow(Math.E, -2.0 * (double)f) * amplitude * (float)Math.Sin(2 * Math.PI * (double)f * numCycles);
            };
        }

        private static void Chunk(float p, float len, out int segment, out float outp)
        {
            segment = (int)(p / len);
            outp = p % len / len;
        }

        public static Func<float, float> FromCurve(Curve c)
        {
            return (f) =>
            {
                return c.Evaluate(f);
            };
        }
    }
}
