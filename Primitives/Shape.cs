using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using SmallGalaxy_Engine.Colliders;

namespace SmallGalaxy_Engine.Primitives
{
    public struct Shape
    {
        private Verticies _verticies;
        public Verticies Verticies { get { return _verticies; } }
        public Shape(Verticies verticies)
        {
            _verticies = verticies;
        }

        public static Shape CreateLine(Vector2 start, Vector2 end)
        {
            Shape s = new Shape();

            s._verticies.Add(start);
            s._verticies.Add(end);

            return s;
        }
        public static Shape CreateLine(LineSegment segment)
        {
            Shape s = new Shape();

            s._verticies.Add(segment.start);
            s._verticies.Add(segment.end);

            return s;
        }

        public static Shape CreateTriangle(Vector2 a, Vector2 b, Vector2 c)
        {
            Shape s = new Shape();

            s._verticies.Add(a);
            s._verticies.Add(b);
            s._verticies.Add(c);
            s._verticies.Add(a);

            return s;
        }

        public static Shape CreateRectangle(Rectangle r)
        {
            Shape s = new Shape();

            s._verticies.Add(new Vector2(r.Left, r.Top));
            s._verticies.Add(new Vector2(r.Right, r.Top));

            s._verticies.Add(new Vector2(r.Right, r.Bottom));
            s._verticies.Add(new Vector2(r.Left, r.Bottom));

            s._verticies.Add(new Vector2(r.Left, r.Top));

            return s;
        }
        public static Shape CreateRectangle(AABB aabb)
        {
            Shape s = new Shape();

            s._verticies.Add(aabb.lowerBound);
            s._verticies.Add(new Vector2(aabb.upperBound.X, aabb.lowerBound.Y));

            s._verticies.Add(aabb.upperBound);
            s._verticies.Add(new Vector2(aabb.lowerBound.X, aabb.upperBound.Y));

            s._verticies.Add(aabb.lowerBound);

            return s;
        }

        // Taken from http://forums.create.msdn.com/forums/t/7414.aspx?PageIndex=1
        public static Shape CreateCircle(Vector2 position, float radius, int sides)
        {
            Shape s = new Shape();

            float max = 2 * (float)Math.PI;
            float step = max / (float)sides;

            for (float theta = 0; theta < max; theta += step)
            {
                s._verticies.Add(position + new Vector2(radius * (float)Math.Cos((double)theta),
                    radius * (float)Math.Sin((double)theta)));
            }

            // then add the first vector again so it's a complete loop
            s._verticies.Add(position + new Vector2(radius * (float)Math.Cos(0),
                    radius * (float)Math.Sin(0)));

            return s;
        }

        // Taken from http://forums.create.msdn.com/forums/t/7414.aspx?PageIndex=1
        public static Shape CreateEllipse(Vector2 position, float semiMajorAxis, float semiMinorAxis, float angleOffset, int sides)
        {
            Shape s = new Shape();

            float max = 2.0f * (float)Math.PI;
            float step = max / (float)sides;
            float h = 0.0f;
            float k = 0.0f;

            for (float t = 0.0f; t < max; t += step)
            {
                // center point: (h,k); add as argument if you want (to circumvent modifying this.Position)
                // x = h + a*cos(t)  -- a is semimajor axis, b is semiminor axis
                // y = k + b*sin(t)
                s._verticies.Add(position + new Vector2((float)(h + semiMajorAxis * Math.Cos(t)),
                                        (float)(k + semiMinorAxis * Math.Sin(t))));
            }

            // then add the first vector again so it's a complete loop
            s._verticies.Add(position + new Vector2((float)(h + semiMajorAxis * Math.Cos(step)),
                                    (float)(k + semiMinorAxis * Math.Sin(step))));

            // now rotate it as necessary
            Matrix m = Matrix.CreateRotationZ(angleOffset);
            for (int i = 0; i < s._verticies.Length; i++)
            {
                s._verticies[i] = Vector2.Transform((Vector2)s._verticies[i], m);
            }

            return s;
        }

    }
}
