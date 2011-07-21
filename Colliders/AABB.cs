using System;
using Microsoft.Xna.Framework;
using SmallGalaxy_Engine.Entities;
using SmallGalaxy_Engine.Primitives;

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

        public float Width { get { return upperBound.X - lowerBound.X; } }
        public float Height { get { return upperBound.Y - lowerBound.Y; } }

        public Vector2 Center { get { return 0.5f * (upperBound + lowerBound); } }        
        public Vector2 Extents { get { return 0.5f * (upperBound - lowerBound); } }
        
        #endregion // Properties

        #region Init
       
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
        public AABB(Vector2 lowerBounds, Vector2 upperBounds)
        {
            lowerBound = lowerBounds;
            upperBound = upperBounds;
        }
        
        #endregion // Init

        #region Methods

        #region Collision
        public bool Intersects(Vector2 point)
        {
            return AABB.IntersectsPoint(this, point);
        }
        public bool Intersects(AABB aabb)
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
        #endregion // Collision

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

        public override bool Equals(object obj)
        {
            if (obj is AABB)
            {
                AABB aabb = (AABB)obj;
                return (
                    this.lowerBound.X == aabb.lowerBound.X &&
                    this.lowerBound.Y == aabb.lowerBound.Y &&
                    this.upperBound.X == aabb.upperBound.X &&
                    this.upperBound.Y == aabb.upperBound.Y);
            }
            return base.Equals(obj);
        }
        public override string ToString()
        {
            return string.Format("lowerBound:{0} upperBound:{1}", lowerBound, upperBound);
        }
        
        #endregion // Methods


        #region Static Collision Methods

        public static bool Overlap(AABB a, AABB b)
        {
            return a.Intersects(b);
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
               
        // Does the Point Intersect with the AABB a
        public static bool IntersectsPoint(AABB a, Vector2 point)
        {
            return (
                point.X < a.Right  && point.X > a.Left && 
                point.Y < a.Bottom && point.Y > a.Top);
        }

        public static bool IntersectsLine_Fast(AABB a, Vector2 lineStart, Vector2 lineEnd)
        {
            if (a.Intersects(lineStart) || a.Intersects(lineEnd)) { return true; }

            LineSegment te, re, be, le;
            te = new LineSegment(new Vector2(a.Left, a.Top), new Vector2(a.Right, a.Top));
            re = new LineSegment(new Vector2(a.Right, a.Top), new Vector2(a.Right, a.Bottom));
            be = new LineSegment(new Vector2(a.Right, a.Bottom), new Vector2(a.Left, a.Bottom));
            le = new LineSegment(new Vector2(a.Left, a.Bottom), new Vector2(a.Left, a.Top));

            return (Collider.LinesIntersect_Fast(lineStart, lineEnd, te.start, te.end) ||
                Collider.LinesIntersect_Fast(lineStart, lineEnd, re.start, re.end) ||
                Collider.LinesIntersect_Fast(lineStart, lineEnd, be.start, be.end) ||
                Collider.LinesIntersect_Fast(lineStart, lineEnd, le.start, le.end));
        }

        // TODO: return ALL hit points (including ends if they are within the bounds of the AABB)
        public static bool IntersectsLine(AABB a, Vector2 lineStart, Vector2 lineEnd, out Vector2 hitPoint)
        {
            LineSegment te, re, be, le;
            te = new LineSegment(new Vector2(a.Left, a.Top), new Vector2(a.Right, a.Top));
            re = new LineSegment(new Vector2(a.Right, a.Top), new Vector2(a.Right, a.Bottom));
            be = new LineSegment(new Vector2(a.Right, a.Bottom), new Vector2(a.Left, a.Bottom));
            le = new LineSegment(new Vector2(a.Left, a.Bottom), new Vector2(a.Left, a.Top));

            return (Collider.LinesIntersect(lineStart, lineEnd, te.start, te.end, out hitPoint) ||
                Collider.LinesIntersect(lineStart, lineEnd, re.start, re.end, out hitPoint) ||
                Collider.LinesIntersect(lineStart, lineEnd, be.start, be.end, out hitPoint) ||
                Collider.LinesIntersect(lineStart, lineEnd, le.start, le.end, out hitPoint));

        }

        #endregion // Static Collision Methods

        #region Static Operators

        public static AABB operator +(Vector2 offset, AABB bounds)
        {
            AABB result = new AABB();
            result.lowerBound = bounds.lowerBound + offset;
            result.upperBound = bounds.upperBound + offset;

            return result;
        }
        public static AABB operator +(AABB bounds, Vector2 offset)
        {
            return offset + bounds;
        }

        #endregion // Static Operators

    }

}
