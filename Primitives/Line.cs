using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SmallGalaxy_Engine;

namespace SmallGalaxy_Engine.Primitives
{
    public struct LineSegment 
    {
        public Vector2 start;
        public Vector2 end;

        public LineSegment(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
        }

        public LineSegment(Vector2 start, Vector2 direction, float length)
        {
            this.start = start;
            this.end = LineHelper.RotateAboutOrigin(start + new Vector2(0, -length), start, LineHelper.VectorToAngle(direction));
        }

        public LineSegment(Vector2 start, float rotation, float length)
        {
            this.start = start;
            this.end = LineHelper.RotateAboutOrigin(start + new Vector2(0, -length), start, rotation);
        }        

        public static LineSegment operator +(Vector2 offset, LineSegment line)
        {
            return line + offset;
        }
        public static LineSegment operator +(LineSegment line, Vector2 offset)
        {
            line.start += offset;
            line.end += offset;
            return line;
        }


    }    
}
