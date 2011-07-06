using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine.Colliders
{

    public struct RayCastResult
    {
        public bool collision;
        public Vector2 position;

        public RayCastResult(Vector2? position)
        {
            collision = position.HasValue;
            this.position = position.HasValue ? position.Value : Vector2.Zero;
        }
    }

    public struct Ray2D
    {
        public Vector2 position;
        public Vector2 direction;

        public Ray2D(Vector2 position, Vector2 direction)
        {
            this.position = position;
            this.direction = Vector2.Normalize(direction);
        }
    }
    
    // TODO: add methods for ray casting and collision checking
    public static class RayCasting
    {

        private static List<Point> result; // for less garbage generation 

        // Returns the number of points between p0 and p1
        private static int PointsBetween(Point p0, Point p1)
        {
            return PointsBetween(p0.X, p0.Y, p1.X, p1.Y);
        }

        // Returns the number of points between (x0, y0) and (x1, y1)
        private static int PointsBetween(int x0, int y0, int x1, int y1)
        {
            return (int)Vector2.Distance(new Vector2(x0, y0), new Vector2(x1, y1));
        }



        // Returns the list of points from p0 to p1
        public static List<Point> BresenhamLine(Point p0, Point p1)
        {
            return BresenhamLine(p0.X, p0.Y, p1.X, p1.Y);
        }

        // Returns the list of points from (x0, y0) to (x1, y1)
        public static List<Point> BresenhamLine(int x0, int y0, int x1, int y1)
        {
            if (result == null) { result = new List<Point>(); }
            result.Clear();

            int sx, sy, dx, dy, e, e2;

            dx = Math.Abs(x1 - x0);
            dy = Math.Abs(y1 - x0);
            if (x0 < x1) { sx = 1; } else { sx = -1; }
            if (y0 < y1) { sy = 1; } else { sy = -1; }
            e = dx - dy;

            while (x0 != x1 && y0 != y1)
            {
                result.Add(new Point(x0, y0));
                e2 = e * 2;
                if (e2 > -dy) { e -= dy; x0 += sx; }
                if (e2 < dx) { e += dx; y0 += sy; }
            }

            return result;
        }

    }
}
