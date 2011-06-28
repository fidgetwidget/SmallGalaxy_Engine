using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SmallGalaxy_Engine.Primitives
{
        
    public class HexGrid : Grid<Hex>
    {

        #region Fields

        private Vector2 _position;
        private Vector2 _padding;
        private Hex _hex;
        private bool _populate = false;

        #endregion // Fields


        #region Properties

        /// <summary>
        /// Sets the Grid Position (offsetting the position of all hex objects in the grid)
        /// </summary>
        /// <remarks> Changing the position of a large grid is expensive, so set the position before populating the grid </remarks>
        public Vector2 Position { get { return _position; } set { SetPosition(value.X, value.Y); } }
        public float X { get { return _position.X; } set { SetPosition(value, _position.Y); } }
        public float Y { get { return _position.Y; } set { SetPosition(_position.X, value); } }

        /// <summary>
        /// Sets the Grid Padding (the space between each hex object in the grid)
        /// </summary>
        /// <remarks> Changing the padding of a large grid is expensive, so set the padding before populating the grid </remarks>
        public Vector2 GridPadding { get { return _padding; } set { SetPadding(value); } }

        public Hex HexMeasures { get { return _hex; } }

        #endregion // Propetries


        #region Init

        public HexGrid(float radius, int colCount, int rowCount)
            : base(colCount, rowCount)
        {
            _hex = new Hex(radius);
        }

        public override void Initialize()
        {
            Initialize(_populate);
        }
        public void Initialize(bool populate)
        {
            for (int row = 0; row < RowCount; row++)
            {
                for (int col = 0; col < ColCount; col++)
                {
                    if (populate)
                        grid[col, row] = new Hex(_hex.Radius);
                    else
                        if (grid[col, row] == null) { continue; }
                    
                    grid[col, row].Grid = this;
                    grid[col, row].Col = col;
                    grid[col, row].Row = row;
                    grid[col, row].X =
                        Position.X + (col * _hex.Width + col * _padding.X + ((row % 2 == 1) ? _hex.HalfWidth : -_padding.X * 0.5f));
                    grid[col, row].Y =
                        Position.Y + (row * _hex.RowHeight + row * _padding.Y);
                }
            }
            IsInitialized = true;
        }

        #endregion // Init


        #region Methods

        public void AddHex(Point coord) { AddHex(coord.X, coord.Y); }
        public void AddHex(int col, int row) { SetAt(col, row, new Hex(_hex.Radius)); }

        protected override void SetAt(int col, int row, Hex value)
        {
            if (col < 0 || col >= ColCount) return;
            if (row < 0 || row >= RowCount) return;
            grid[col, row] = value;
            if (value == null) return;

            value.Grid = this;
            value.Col = col;
            value.Row = row;
            value.X = Position.X + (col * _hex.Width + col * _padding.X + ((row % 2 == 1) ? _hex.HalfWidth : -_padding.X * 0.5f));
            value.Y = Position.Y + (row * _hex.RowHeight + row * _padding.Y);
        }

        protected void SetPosition(float x, float y)
        {
            _position.X = x;
            _position.Y = y;

            if (!isInitialized) { return; }
            for (int row = 0; row <= RowCount; row++)
            {
                for (int col = 0; col <= ColCount; col++)
                {
                    if (grid[col, row] == null) { continue; }

                    grid[col, row].X =
                        Position.X + (col * _hex.Width + col * _padding.X + ((row % 2 == 1) ? _hex.HalfWidth : -_padding.X * 0.5f));
                    grid[col, row].Y =
                        Position.Y + (row * _hex.RowHeight + row * _padding.Y);
                }
            }
        }

        protected void SetPadding(Vector2 value)
        {
            _padding = value;

            if (!isInitialized) { return; }
            for (int row = 0; row <= RowCount; row++)
            {
                for (int col = 0; col <= ColCount; col++)
                {
                    if (grid[col, row] == null) { continue; }

                    grid[col, row].X =
                        Position.X + (col * _hex.Width + col * _padding.X + ((row % 2 == 1) ? _hex.HalfWidth : -_padding.X * 0.5f));
                    grid[col, row].Y =
                        Position.Y + (row * _hex.RowHeight + row * _padding.Y);

                }
            }
        }

        public Point GetNeighbor(Point coord, HexDirections direction)
        {
            Point neighbor = Hex.Neighbor(coord, direction);
            if (neighbor.X < 0 || neighbor.Y < 0 ||
                neighbor.X >= ColCount || neighbor.Y >= RowCount)
            {
                if (this[coord] != null)
                    return coord;
                else
                    return new Point(-1, -1);
            }

            if (this[neighbor] == null)
                return GetNeighbor(neighbor, direction);

            return neighbor;
        }

        /// <summary>
        /// Returns the Hex Coordinate of the given world position
        /// </summary>
        public Point CoordinateOf(Vector2 worldPosition)
        {
            return CoordinateOf(worldPosition.X, worldPosition.Y);
        }
        
        public Point CoordinateOf(float x, float y)
        {
            int X = (int)Math.Floor(Position.X + x / (_hex.Width + _padding.X));
            int Y = (int)Math.Floor(Position.Y + y / (_hex.RowHeight + _padding.Y));
            Vector2 offset = new Vector2(
                x - X * (_hex.Width + _padding.X),
                y - Y * (_hex.RowHeight + _padding.Y));

            if (Y % 2 == 0) //Is this an even row?
            {
                //Section type A
                if (offset.Y < (-_hex.Slope * offset.X + _hex.Rise)) //Point is below left line; inside SouthWest neighbor.
                {
                    X -= 1;
                    Y -= 1;
                }
                else if (offset.Y < (_hex.Slope * offset.X - _hex.Rise)) //Point is below right line; inside SouthEast neighbor.
                    Y -= 1;
            }
            else
            {
                //Section type B
                if (offset.X >= _hex.HalfWidth) //Is the point on the right side?
                {
                    if (offset.Y < (-_hex.Slope * offset.X + _hex.Rise * 2.0f)) //Point is below bottom line; inside SouthWest neighbor.
                        Y -= 1;
                }
                else //Point is on the left side
                {
                    if (offset.Y < (_hex.Slope * offset.X)) //Point is below the bottom line; inside SouthWest neighbor.
                        Y -= 1;
                    else //Point is above the bottom line; inside West neighbor.
                        X -= 1;
                }
            }

            return new Point(X, Y);
        }


        //*** Don't use these if you are only going to loop through it once and throw it away *** //

        public List<Point> CoordinateSubset(Rectangle area)
        {
            Point tl = CoordinateOf(area.Left, area.Top);
            tl.X = (int)MathHelper.Clamp(tl.X, 0, ColCount);
            tl.Y = (int)MathHelper.Clamp(tl.Y, 0, RowCount);

            Point br = CoordinateOf(area.Right, area.Bottom);
            br.X = (int)MathHelper.Clamp(br.X + 1, 0, ColCount);
            br.Y = (int)MathHelper.Clamp(br.Y + 1, 0, RowCount);

            List<Point> subset = new List<Point>((tl.X + br.X) * (tl.Y + br.Y));
            if (subset.Count > 0)
            {
                int i = 0;
                for (int row = tl.Y; row < br.Y; row++)
                {
                    for (int col = tl.X; col < br.X; col++)
                    {
                        subset[i++] = new Point(col, row);
                    }
                }
            }
            return subset;
        }

        public List<Point> CoordinateSubset(Point center, int radius)
        {
            List<Point> subset = new List<Point>(6);
            subset.Add(center);
            for (int r = 0; r <= radius + 1; r++)
            {
                for (int i = 0; i < r * 6; i++)
                {
                    var coord = HexGrid.PolarToHexCoordinate(center, r, i);
                    if (coord.X >= 0 && coord.Y >= 0 &&
                        coord.X < ColCount && coord.Y < RowCount)
                    {
                        subset.Add(coord);
                    }
                }
            }
            return subset;
        }

        public List<Point> CoordinateSubset(Point center, int innerRadius, int outerRadius)
        {
            List<Point> subset = new List<Point>();
            for (int r = innerRadius + 1; r <= outerRadius; r++)
            {
                for (int i = 0; i < r * 6; i++)
                {
                    var coord = HexGrid.PolarToHexCoordinate(center, r, i);
                    if (coord.X >= 0 && coord.Y >= 0 &&
                        coord.X < ColCount && coord.Y < RowCount)
                    {
                        subset.Add(coord);
                    }
                }
            }
            return subset;
        }

        /// <summary>
        /// Returns the Hex Coordinate for the given Polar Coordinate (r, i) for the given Center (center)
        /// </summary>
        public static Point PolarToHexCoordinate(Point center, int r, int i)
        {
            Point coord = center;
            int s, e;

            s = (i / r) % 6;
            e = (i % r);

            for (int a = 0; a < r; a++)
            {
                coord = Hex.Neighbor(coord, (HexDirections)((s + 5) % 6));
            }
            for (int a = 0; a <= e; a++)
            {
                coord = Hex.Neighbor(coord, (HexDirections)((s + 1) % 6));
            }
            return coord;
        }

        #endregion // Methods

    }
}
