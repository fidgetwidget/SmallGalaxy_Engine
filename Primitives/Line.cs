using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SmallGalaxy_Engine.Primitives
{
    public struct LineSegment 
    {
        public Vector2 start;
        public Vector2 end;
    }

    public class Line
    {

        #region Fields

        private LineSegment _segment;
        private Texture2D _texture;

        #endregion // Fields


        #region Properties

        public LineSegment Segment { get { return _segment; } }
        public Vector2 Start { get { return _segment.start; } set { SetStart(value); } }
        public Vector2 End { get { return _segment.end; } set { SetEnd(value); } }

        public float Length { get { return Vector2.Distance(_segment.start, _segment.end); } }
        public float Rotation { get { return AngleBetween(_segment.start, _segment.end); } }
        
        #endregion // Properties


        #region Init

        public Line() { }
        public Line(Vector2 start, Vector2 end)
        {
            _segment.start = start;
            _segment.end = end;
        }
        public Line(Vector2 start, float length, float rotation)
        {
            _segment.start = start;
            _segment.end = RotateAboutOrigin(start + new Vector2(0, -length), start, rotation);
        }
        public Line(Vector2 start, float length, Vector2 projectedEnd)
        {
            _segment.start = start;
            _segment.end = RotateAboutOrigin(start + new Vector2(0, -length), start, AngleBetween(start, projectedEnd));
        }

        #endregion // Init


        #region Methods

        protected void SetStart(Vector2 value)
        {
            SetStart(value.X, value.Y);
        }
        protected void SetStart(float x, float y)
        {
            _segment.start.X = x;
            _segment.start.Y = y;
        }

        protected void SetEnd(Vector2 value)
        {
            SetEnd(value.X, value.Y);
        }
        protected void SetEnd(float x, float y)
        {
            _segment.end.X = x;
            _segment.end.Y = y;
        }

        #endregion // Methods


        #region Static Methods

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

        public static float AngleBetween(Vector2 pointA, Vector2 pointB)
        {
            if (pointA == pointB) { return 0; }
            float angle = (float)Math.Atan2((double)(pointA.Y - pointB.Y), (double)(pointA.X - pointB.X)) - MathHelper.ToRadians(90);
            if (angle < 0) angle += MathHelper.ToRadians(360);
            return angle;
        }

        public static float VectorToAngle(Vector2 directionVector)
        {
            float angle = (float)Math.Atan2(directionVector.Y, directionVector.X);
            return angle;
        }

        #endregion // Static Methods

    }
}
