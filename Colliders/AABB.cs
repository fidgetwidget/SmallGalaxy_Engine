using System;
using Microsoft.Xna.Framework;
using SmallGalaxy_Engine.Entities;

namespace SmallGalaxy_Engine.Colliders
{

    // Axis Aligned Bounding Box
    public struct AABB
    {

        public Vector2 lowerBound;
        public Vector2 upperBound;        

        #region Properties

        public float Left { get { return lowerBound.X; } }
        public float Right { get { return upperBound.X; } }
        public float Top { get { return lowerBound.Y; } }
        public float Bottom { get { return upperBound.Y; } }

        public Vector2 Center { get { return 0.5f * (upperBound + lowerBound); } }
        
        public Vector2 Extents { get { return 0.5f * (upperBound - lowerBound); } }
        
        #endregion // Properties

        #region Init

        public AABB(Vector2 lowerBounds, Vector2 upperBounds)
        {
            lowerBound = lowerBounds;
            upperBound = upperBounds;
        }

        public AABB(Rectangle rect)
        {
            lowerBound.X = rect.X;
            lowerBound.Y = rect.Y;
            upperBound.X = rect.X + rect.Width;
            upperBound.Y = rect.Y + rect.Height;
        }

        public AABB(float x, float y, float width, float height)
        {
            lowerBound.X = x;
            lowerBound.Y = y;
            upperBound.X = lowerBound.X + width;
            upperBound.Y = lowerBound.Y + height;
        }
        
        public AABB(Vector2 position, float width, float height)
        {
            lowerBound.X = position.X;
            lowerBound.Y = position.Y;
            upperBound.X = lowerBound.X + width;
            upperBound.Y = lowerBound.Y + height;
        }

        
        #endregion // Init

        #region Methods
        
        // Does it overlap with the other
        public bool Overlap(AABB aabb)
        {
            Vector2 d1, d2;
            d1 = aabb.upperBound - lowerBound;
            d2 = upperBound - aabb.lowerBound;

            if (d1.X > 0.0f || d1.Y > 0.0f)
                return false;

            if (d2.X > 0.0f || d2.Y > 0.0f)
                return false;

            return true;
        }
        // Does this contain the other within it
        public bool Contains(AABB aabb)
        {
            if (upperBound.X <= aabb.upperBound.X) return false;
            if (upperBound.Y <= aabb.upperBound.Y) return false;
            if (aabb.lowerBound.X <= lowerBound.X) return false;
            if (aabb.lowerBound.Y <= lowerBound.Y) return false;

            return true;
        }

        // Add the other to this bounds
        public void Combine(ref AABB aabb)
        {
            upperBound = Vector2.Min(upperBound, aabb.upperBound);
            lowerBound = Vector2.Max(lowerBound, aabb.lowerBound);
        }

        public Rectangle ToRectangle()
        {
            int x, y, width, height;
            x = (int)lowerBound.X;
            y = (int)lowerBound.Y;
            width = (int)(upperBound.X - lowerBound.X);
            height = (int)(upperBound.Y - lowerBound.Y);

            return new Rectangle(x, y, width, height);
        }
        public override string ToString()
        {
            return string.Format("lowerBound:{0} upperBound:{1}", lowerBound, upperBound);
        }
        
        #endregion // Methods

        // STATIC 
        public static bool Overlap(AABB a, AABB b)
        {
            return a.Overlap(b);
        }
        public static Vector2 GetIntersectionDepth(AABB a, AABB b)
        {
            // Calculate half sizes.
            Vector2 extendsA = a.Extents;
            Vector2 extendsB = b.Extents;

            // Calculate centers.
            Vector2 centerA = a.Center;
            Vector2 centerB = b.Center;

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
        public static AABB operator + (AABB bounds, Vector2 offset)
        {
            return offset + bounds;
        }

    }

}
