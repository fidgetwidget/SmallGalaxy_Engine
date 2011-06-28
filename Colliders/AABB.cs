using System;
using Microsoft.Xna.Framework;
using SmallGalaxy_Engine.Entities;

namespace SmallGalaxy_Engine.Colliders
{

    // Axis Aligned Bounding Box
    public struct AABB
    {
        public Vector2 upperBound;
        public Vector2 lowerBound;

        public AABB(float x, float y, int width, int height)
        {
            lowerBound.X = x;
            lowerBound.Y = y;
            upperBound.X = lowerBound.X + width;
            upperBound.Y = lowerBound.Y + height;
        }
        public AABB(Vector2 position, int width, int height)
        {
            lowerBound.X = position.X;
            lowerBound.Y = position.Y;
            upperBound.X = lowerBound.X + width;
            upperBound.Y = lowerBound.Y + height;
        }

        public Vector2 GetCenter()
        {
            return 0.5f * (upperBound + lowerBound);
        }

        public Vector2 GetExtents()
        {
            return 0.5f * (upperBound - lowerBound);
        } 

        public bool Contains(ref AABB aabb)
        {
            if (upperBound.X <= aabb.upperBound.X) return false;
            if (upperBound.Y <= aabb.upperBound.Y) return false;
            if (aabb.lowerBound.X <= lowerBound.X) return false;
            if (aabb.lowerBound.Y <= lowerBound.Y) return false;

            return true;
        }

        public void Combine(ref AABB aabb)
        {
            upperBound = Vector2.Min(upperBound, aabb.upperBound);
            lowerBound = Vector2.Max(lowerBound, aabb.lowerBound);
        }

        public override string ToString()
        {
            return string.Format("lowerBound:{0} upperBound:{1}", lowerBound, upperBound);
        }

        public static bool Overlap(ref AABB a, ref AABB b)
        {
            Vector2 d1, d2;
            d1 = b.upperBound - a.lowerBound;
            d2 = a.upperBound - b.lowerBound;

            if (d1.X > 0.0f || d1.Y > 0.0f)
                return false;

            if (d2.X > 0.0f || d2.Y > 0.0f)
                return false;

            return true;
        }

        public static Vector2 GetIntersectionDepth(AABB a, AABB b)
        {
            // Calculate half sizes.
            Vector2 extendsA = a.GetExtents();
            Vector2 extendsB = b.GetExtents();

            // Calculate centers.
            Vector2 centerA = a.GetCenter();
            Vector2 centerB = b.GetCenter();

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = extendsA.X + extendsB.X;
            float minDistanceY = extendsA.Y + extendsB.Y;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        public static AABB operator + (Vector2 offset, AABB bounds)
        {
            AABB result = new AABB();
            result.lowerBound = bounds.lowerBound + offset;
            result.upperBound = bounds.upperBound + offset;

            return result;
        }

    }

}
