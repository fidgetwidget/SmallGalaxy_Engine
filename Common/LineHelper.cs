using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine
{
    public static class LineHelper
    {

        public static Vector2 RotateAboutOrigin(Vector2 point, Vector2 origin, float rotation)
        {
            Vector2 u = point - origin; //point relative to origin

            if (u == Vector2.Zero)
                return point;

            float a = (float)Math.Atan2(u.Y, u.X); //angle relative to origin
            a += rotation; //rotate  

            //u is now the new point relative to origin  
            u = u.Length() * new Vector2((float)Math.Cos(a), (float)Math.Sin(a));
            return u + origin;
        }

        // returns Angle in Radians between 0 and 2*Pi
        public static float AngleBetween(Vector2 pointA, Vector2 pointB)
        {
            if (pointA == pointB) { return 0; }

            float angle = (float)Math.Atan2((double)(pointA.Y - pointB.Y), (double)(pointA.X - pointB.X)) - MathHelper.ToRadians(90);
            if (angle < 0) angle += MathHelper.ToRadians(360);
            return angle;
        }

        public static float VectorToAngle(Vector2 direction)
        {
            return (float)Math.Atan2(direction.Y, direction.X);
        }

    }
}
