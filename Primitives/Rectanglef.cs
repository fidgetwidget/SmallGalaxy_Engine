using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SmallGalaxy_Engine.Primitives;

namespace SmallGalaxy_Engine
{
    public class Rectanglef
    {

        #region Fields

        private Vector2 _position = Vector2.Zero;
        private float _width = 1;
        private float _height = 1;

        #endregion // Fields


        #region Properties

        public Vector2 Position { get { return _position; } set { SetPosition(value); } }
        public float X { get { return _position.X; } set { SetPosition(value, _position.Y); } }
        public float Y { get { return _position.Y; } set { SetPosition(_position.X, value); } }
        public float Width { get { return _width; } set { SetWidth(value); } }
        public float Height { get { return _height; } set { SetHeight(value); } }
        
        public float Top { get { return _position.Y; } }
        public float Bottom { get { return _position.Y + _height; } }
        public float Left { get { return _position.X; } }
        public float Right { get { return _position.X + _width; } }

        public Vector2 Center { get { return Position + Origin; } }
        public Vector2 Origin { get { return new Vector2(_width / 2, _height / 2); } }
        
        #endregion // Properties


        #region Init

        public Rectanglef(float x, float y, float width, float height)
        {
            _position.X = x;
            _position.Y = y;
            _width = width;
            _height = height;
        }
        public Rectanglef(Vector2 position, float width, float height)
            : this(position.X, position.Y, width, height) { }        
        public Rectanglef(Rectanglef rect)
            : this(rect.X, rect.Y, rect.Width, rect.Height) { }
        public Rectanglef(Rectangle rect)
            : this(rect.X, rect.Y, rect.Width, rect.Height) { }
        
        #endregion // Init


        #region Methods

        public void GetCorners(out Vector2 tl, out Vector2 tr, out Vector2 br, out Vector2 bl)
        {
            tl = Position;
            tr = Position + new Vector2(_width, 0);
            br = Position + new Vector2(_width, _height);
            bl = Position + new Vector2(0, _height);
            return;
        }

        public void SetPosition(Vector2 value)
        {
            SetPosition(value.X, value.Y);
        }
        public void SetPosition(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
        }

        public void SetWidth(float value)
        {
            _width = value;
        }
        public void SetHeight(float value)
        {
            _height = value;
        }

        /// <summary>
        /// Whether or not this rectangle contains the point Position.
        /// </summary>
        public bool Contains(Vector2 position)
        {
            if ((position.X >= Left && position.X <= Right) &&
                (position.Y >= Top && position.Y <= Bottom))
                return true;

            return false;
        }

        /// <summary>
        /// Whether or not this rectangle contains the point at x,y
        /// </summary>
        public bool Contains(float x, float y)
        {
            if ((x >= Left && x <= Right) &&
                (y >= Top && y <= Bottom))
                return true;

            return false;
        }

        /// <summary>
        /// Whether or not this rectangle contains the given rectangle
        /// </summary>
        public bool Contains(Rectanglef rectangle)
        {
            if ((rectangle.Left >= Left && rectangle.Left <= Right) &&
                (rectangle.Right <= Right && rectangle.Right >= Left) &&
                (rectangle.Top >= Top && rectangle.Top <= Bottom) &&
                (rectangle.Bottom >= Top && rectangle.Bottom <= Bottom))
                return true;

            return false;
        }

        /// <summary>
        /// Whether or not this rectangle intersects the given rectangle
        /// </summary>
        public bool Intersects(Rectanglef rectangle)
        {
            if ((rectangle.X >= Left && rectangle.X <= Right) ||
                (rectangle.Y >= Top && rectangle.Y <= Bottom))
                return true;

            return false;
        }

        /// <summary>
        /// Return this Vector Rectangle As a Regular Rectangle
        /// </summary>
        /// <returns></returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }

        public override string ToString()
        {
            return "{X:" + X + " Y:" + Y + " Width:" + Width + " Height:" + Height + "}";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Rectanglef item;
            if (obj is Rectanglef)
            {
                item = (Rectanglef)obj;
            }
            else { return false; }

            return (this.Position == item.Position &&
                    this.Height == item.Height &&
                    this.Width == item.Width);
        }

        #endregion // Methods


        #region Static Methods

        /// <summary>
        /// Returns a Vector Rectangle that contains exactly the intersection between value1 and value2
        /// </summary>
        public static Rectanglef Intersect(Rectanglef value1, Rectanglef value2)
        {
            float left = Math.Max(value1.Left, value2.Left);
            float right = Math.Min(value1.Right, value2.Right);
            float top = Math.Max(value1.Top, value2.Top);
            float bottom = Math.Min(value1.Bottom, value2.Bottom);

            return new Rectanglef(left, top, (right - left), (bottom - top));
        }

        /// <summary>
        /// Return a VectorRectangle that Completely Contains Both value1 and value2
        /// </summary>
        public static Rectanglef Union(Rectanglef value1, Rectanglef value2)
        {
            float left = Math.Min(value1.Left, value2.Left);
            float right = Math.Max(value1.Right, value2.Right);
            float top = Math.Min(value1.Top, value2.Top);
            float bottom = Math.Min(value1.Bottom, value2.Bottom);

            return new Rectanglef(left, top, (right - left), (bottom - top));
        }

        #endregion // Static Methods


        #region Static Operator

        public static bool operator !=(Rectanglef a, Rectanglef b)
        {
            if ((object)a == null)
            {
                return (object)b != null;
            }

            return !a.Equals(b);
        }

        public static bool operator ==(Rectanglef a, Rectanglef b)
        {
            if ((object)a == null)
            {
                return (object)b == null;
            }

            return a.Equals(b);
        }

        public static Rectanglef Empty
        {
            get { return new Rectanglef(0, 0, 0, 0); }
        }

        #endregion //Static Operator

    }
}
