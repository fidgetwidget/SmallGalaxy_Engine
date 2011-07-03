using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SmallGalaxy_Engine.Entities;

namespace SmallGalaxy_Engine.Primitives
{

    #region Directions Enum

    /// <summary>
    /// the numbers go counter clockwise starting with the upper right corner
    ///  [1][0]
    /// [2][][5]
    ///  [3][4]
    /// </summary>
    public enum HexDirections : int
    {
        NorthEast = 0,
        NorthWest = 1,
        West = 2,
        SouthWest = 3,
        SouthEast = 4,
        East = 5,
        NumberOfDirections = 6,
    }

    #endregion // Directions Enum


    public class Hex : Entity
    {

        #region Fields

        private HexGrid _grid;
        private Point _coord;

        private float _radius;
        private float _width;
        private float _halfWidth;
        private float _height;
        private float _rowHeight;
        private float _rise;
        private float _slope;

        private Polygon _vertices;

        #endregion // Fields


        #region Properties

        public HexGrid Grid { get { return _grid; } protected internal set { _grid = value; } }

        public Point Coord { get { return _coord; } protected internal set { _coord = value; } }
        public int Col { get { return _coord.X; } protected internal set { _coord.X = value; } }
        public int Row { get { return _coord.Y; } protected internal set { _coord.Y = value; } }

        public float Radius { get { return _radius; } }
        public float Width { get { return _width; } }
        public float HalfWidth { get { return _halfWidth; } }
        public float Height { get { return _height; } }
        public float RowHeight { get { return _rowHeight; } }
        public float Rise { get { return _rise; } }
        public float Slope { get { return _slope; } }

        /// <summary>
        /// The Center Position of this tile
        /// </summary>
        public Vector2 Center
        {
            get { return new Vector2(_halfWidth, _radius); }
        }

        public Vertices Vertices { get { return _vertices.TransformedVertices; } }

        #endregion // Properties


        #region Init

        public Hex(float radius)
            : base()
        {
            _radius = radius;

            _height = 2 * radius;
            _rowHeight = 1.5f * radius;
            _halfWidth = (float)Math.Sqrt((radius * radius) - ((radius / 2) * (radius / 2)));
            _width = 2 * _halfWidth;
            _rise = _height - _rowHeight;
            _slope = _rise / _halfWidth;
        }

        protected override void InitCore()
        {
            InitVertices();
        }

        private void InitVertices()
        {
            Vector2[] vertices = new Vector2[6];

            vertices[0].X = _width + Position.X;
            vertices[0].Y = _rise + Position.Y;

            vertices[1].X = _halfWidth + Position.X;
            vertices[1].Y = Position.Y;

            vertices[2].X = Position.X;
            vertices[2].Y = _rise + Position.Y;

            vertices[3].X = Position.X;
            vertices[3].Y = _rowHeight + Position.Y;

            vertices[4].X = _halfWidth + Position.X;
            vertices[4].Y = _height + Position.Y;

            vertices[5].X = _width + Position.X;
            vertices[5].Y = _rowHeight + Position.Y;

            _vertices = new Polygon(vertices);
        }

        #endregion // Init


        #region Methods

        protected Matrix GetTransform()
        {
            Matrix rotM, posM, transform;

            Matrix.CreateRotationZ(Rotation, out rotM);
            Matrix.CreateTranslation(
                Position.X,
                Position.Y, 0, out posM);

            Matrix.Multiply(ref rotM, ref posM, out transform);
            return transform;
        }
        protected void TransformVertices(Matrix transform)
        {
            _vertices.Transform(transform);
        }

        public override void SetPosition(float x, float y)
        {
            base.SetPosition(x, y);
            TransformVertices(GetTransform());            
        }
        public override void SetRotation(float rotation)
        {
            base.SetRotation(rotation);
            TransformVertices(GetTransform());
        }

        #endregion // Methods


        #region Static Methods

        /// <summary>
        /// Returns the Hex Direction from a normal Vector
        /// </summary>
        public static HexDirections GetDirection(Vector2 normal)
        {
            if (normal.X >= 0 && normal.Y <= -0.5 && normal.Y > -1)
                return HexDirections.NorthEast;

            if (normal.X > 0 && normal.Y > -0.5f && normal.Y < 0.5f)
                return HexDirections.East;

            if (normal.X >= 0 && normal.Y > 0.5f && normal.Y <= 1)
                return HexDirections.SouthEast;

            if (normal.X <= 0 && normal.Y >= 0.5f && normal.Y < 1)
                return HexDirections.SouthWest;

            if (normal.X < 0 && normal.Y > -0.5f && normal.Y < 0.5f)
                return HexDirections.West;

            if (normal.X <= 0 && normal.Y < -0.5 && normal.Y >= -1)
                return HexDirections.NorthWest;

            return HexDirections.NumberOfDirections;
        }

        /// <summary>
        /// Returns the Direction after the rotating it the given amount
        /// </summary>
        /// <param name="direction">The Starting Direction</param>
        /// <param name="amount">The number of Directions to Rotate</param>
        public static HexDirections RotateDirection(HexDirections direction, int amount)
        {
            //Let's make sure our directions stay within the enumerated values.
            if (direction < HexDirections.NorthEast || direction > HexDirections.NorthWest || Math.Abs(amount) > (int)HexDirections.NorthWest)
                throw new InvalidOperationException("Directions out of range.");

            direction += amount;

            //Now we need to make sure direction stays within the proper range.
            //C# does not allow modulus operations on enums, so we have to convert to and from int.

            int n_dir = (int)direction % (int)HexDirections.NumberOfDirections;
            if (n_dir < 0) n_dir = (int)HexDirections.NumberOfDirections + n_dir;
            direction = (HexDirections)n_dir;

            return direction;
        }

        /// <summary>
        /// Returns the Direction opposite the given Direction
        /// </summary>
        public static HexDirections Opposite(HexDirections direction) { return RotateDirection(direction, 3); }

        /// <summary>
        /// Returns the tileCoordinate of the Neighboring tile in the given direction for the given tileCoordinate
        /// </summary>
        public static Point Neighbor(Point hexCoordinate, HexDirections direction)
        {
            Point offset = HexOffset(hexCoordinate, direction);
            hexCoordinate.X += offset.X;
            hexCoordinate.Y += offset.Y;
            return hexCoordinate;
        }

        /// <summary>
        /// Returns the coordinate origin Point for the given Hex in the given Direction
        /// </summary>
        public static Point HexOffset(Point hexCoordinate, HexDirections direction)
        {
            Point offset = new Point(0, 0);
            if (hexCoordinate.Y % 2 == 0) //Is this row even?
            {
                switch (direction)
                {
                    case HexDirections.NorthEast: offset = new Point(0, -1); break;
                    case HexDirections.NorthWest: offset = new Point(-1, -1); break;
                    case HexDirections.West: offset = new Point(-1, 0); break;
                    case HexDirections.SouthWest: offset = new Point(-1, 1); break;
                    case HexDirections.SouthEast: offset = new Point(0, 1); break;
                    case HexDirections.East: offset = new Point(1, 0); break;
                    default: break;
                }
            }
            else // Is this row odd
            {
                switch (direction)
                {
                    case HexDirections.NorthEast: offset = new Point(1, -1); break;
                    case HexDirections.NorthWest: offset = new Point(0, -1); break;
                    case HexDirections.West: offset = new Point(-1, 0); break;
                    case HexDirections.SouthWest: offset = new Point(0, 1); break;
                    case HexDirections.SouthEast: offset = new Point(1, 1); break;
                    case HexDirections.East: offset = new Point(1, 0); break;
                    default: break;
                }
            }
            return offset;
        }

        /// <summary>
        /// Retuns the Distance in Hexs between Hex Coordiantes A and B
        /// </summary>
        public static int HexDistance(Point A, Point B)
        {
            int dx = B.X - A.X;
            int dy = B.Y - A.Y;
            return (Math.Abs(dx) + Math.Abs(dy) + Math.Abs(dx - dy)) / 2;
        }

        /// <summary>
        /// Returns the origin Vector for the Corner at the given index
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetCornerOffset(float radius, int index)
        {
            if (index >= 6 || index < 0) { return Vector2.Zero; }
            Hex hex = new Hex(radius);
            hex.Position = -hex.Center;
            return hex.Vertices[index];
        }

        #endregion // Static Methods


        #region Operators

        public static bool operator !=(Hex a, Hex b)
        {
            if ((object)a == null)
            {
                return (object)b != null;
            }

            return !a.Equals(b);
        }
        public static bool operator ==(Hex a, Hex b)
        {
            if ((object)a == null)
            {
                return (object)b == null;
            }

            return a.Equals(b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            Hex item;
            if (obj is Hex)
            {
                item = (Hex)obj;
                if (item.Position == Position &&
                    item.Rotation == Rotation &&
                    item.Radius == Radius)
                    return true;
            }

            return false;
        }

        #endregion // Operator

    }
}
