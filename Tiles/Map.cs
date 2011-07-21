using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SmallGalaxy_Engine;
using SmallGalaxy_Engine.Entities;

namespace SmallGalaxy_Engine
{

    public class Map
    {

        #region Fields

        private Vector2 _position = Vector2.Zero; // the position of a cell at coord {0,0}
        private int _itemWidth, _itemHeight;
        private bool _hasLimits = false;
        private int _minRow = int.MinValue, _minCol = int.MinValue;
        private int _maxRow = int.MaxValue, _maxCol = int.MaxValue;
        private bool _wrapH = false;
        private bool _wrapV = false;

        #endregion // Fields


        #region Properties

        public Vector2 Position { get { return _position; } set { SetPosition(value.X, value.Y); } }
        public float X { get { return _position.X; } set { SetPosition(value, _position.Y); } }
        public float Y { get { return _position.Y; } set { SetPosition(_position.X, value); } }

        public int CellWidth { get { return _itemWidth; } }
        public int CellHeight { get { return _itemHeight; } }

        public bool WrapHorizontally { get { return _wrapH; } set { _wrapH = value; } }
        public bool WrapVertically { get { return _wrapV; } set { _wrapV = value; } }

        #endregion // Properties


        #region Init

        public Map(int cellWidth, int cellHeight)
        {
            _itemWidth = cellWidth;
            _itemHeight = cellHeight;
        }

        #endregion // Init


        #region Methods

        // Returns the Coord of the Cell at world position {x,y}
        public Point CoordOf(float x, float y)
        {
            int xCoord, yCoord;
            CoordOf(x, y, out xCoord, out yCoord);
            return new Point(xCoord, yCoord);
        }
        public Point CoordOf(Vector2 worldPosition)
        {
            return CoordOf(worldPosition.X, worldPosition.Y);
        }
        public void CoordOf(float x, float y, out int xCoord, out int yCoord)
        {
            xCoord = (int)Math.Floor((x - Position.X) / _itemWidth);
            yCoord = (int)Math.Floor((y - Position.Y) / _itemHeight);

            if (xCoord > 0 && xCoord > _maxCol) { xCoord = _wrapH ? xCoord % _maxCol : _maxCol; } 
            else if (xCoord < 0 && xCoord < _minCol) { xCoord = _wrapH ? xCoord % _minCol : _minCol; }

            if (yCoord > 0 && yCoord > _maxRow) { yCoord = _wrapV ? yCoord % _maxRow : _maxRow; }
            else if (yCoord < 0 && yCoord < _minRow) { yCoord = _wrapV ? yCoord % _minRow : _minRow; }
        }
        public void CoordOf(Vector2 worldPosition, out int xCoord, out int yCoord)
        {
            CoordOf(worldPosition.X, worldPosition.Y, out xCoord, out yCoord);
        }

        // Returns the world position of the Cell at the given coords {xCoord,yCoord}
        // Doesn't wrap the position based on Map limits and wrapping
        public Vector2 PositionOf(int xCoord, int yCoord)
        {
            float x, y;
            PositionOf(xCoord, yCoord, out x, out y);
            return new Vector2(x, y);
        }
        public Vector2 PositionOf(Point coord)
        {
            return PositionOf(coord.X, coord.Y);
        }
        public void PositionOf(int xCoord, int yCoord, out float x, out float y)
        {
            x = xCoord * CellWidth + X;
            y = yCoord * CellHeight + Y;
        }
        public void PositionOf(Point coord, out float x, out float y)
        {
            PositionOf(coord.X, coord.Y, out x, out y);
        }
        public void PositionOf(int xCoord, int yCoord, out Vector2 position)
        {
            PositionOf(xCoord, yCoord, out position.X, out position.Y);
        }
        public void PositionOf(Point coord, out Vector2 position)
        {
            PositionOf(coord.X, coord.Y, out position.X, out position.Y);
        }

        public void SetPosition(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
        }

        #endregion // Methods

    }

    // A list of Generic Cell's
    public class Map<T> : Grid<T> where T : class, ITileable<T>, new()
    {

        #region Fields

        private Vector2 _position;
        private int _cellWidth, _cellHeight;

        #endregion // Fields


        #region Properties

        /// <summary>
        /// Sets the Grid Position (offsetting the position of all Tile objects in the grid)
        /// </summary>
        /// <remarks> Changing the position of a large grid is expensive, so set the position before populating the grid </remarks>
        public Vector2 Position { get { return _position; } set { SetPosition(value.X, value.Y); } }
        public float X { get { return _position.X; } set { SetPosition(value, _position.Y); } }
        public float Y { get { return _position.Y; } set { SetPosition(_position.X, value); } }

        public int CellWidth { get { return _cellWidth; } }
        public int CellHeight { get { return _cellHeight; } }

        #endregion // Propetries


        #region Init

        public Map(int width, int height, int colCount, int rowCount)
            : base(colCount, rowCount)
        {
            _cellWidth = width;
            _cellHeight = height;
        }

        public override void Initialize()
        {
            Initialize(false);            
        }

        public void Initialize(bool populate)
        {
            for (int row = 0; row < RowCount; row++)
            {
                for (int col = 0; col < ColCount; col++)
                {
                    if (populate) { grid[col, row] = new T(); }
                    if (grid[col, row] == null) { continue; }

                    grid[col, row].SetMap(this);
                    grid[col, row].SetCoord(col, row);
                }
            }
            IsInitialized = true;
        }

        #endregion // Init


        #region Methods

        protected override void SetAt(int col, int row, T value)
        {
            if (col < 0 || col >= ColCount) return;
            if (row < 0 || row >= RowCount) return;
            grid[col, row] = value;
            if (value == null) return;

            value.SetMap(this);
            value.SetCoord(col, row);
        }

        protected void SetPosition(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
        }

        public T GetNeighbor(Point coord, TileDirections direction)
        {
            Point nCoord = GetNeighborCoord(coord, direction);
            if (InBounds(nCoord))
                return this[nCoord];
            else
                return null;
        }

        public Point GetNeighborCoord(Point coord, TileDirections direction)
        {
            Point neighbor = Tiles.Neighbor(coord, direction);
            if (neighbor.X < 0 || neighbor.Y < 0 ||
                neighbor.X >= ColCount || neighbor.Y >= RowCount)
            {
                if (InBounds(coord) && this[coord] != null)
                    return coord;
                else
                    return new Point(-1, -1);
            }

            if (this[neighbor] == null)
                return GetNeighborCoord(neighbor, direction);

            return neighbor;
        }

        public T AtPosition(Vector2 point)
        {
            return AtPosition(point.X, point.Y);
        }
        public T AtPosition(float x, float y)
        {
            int X = (int)Math.Floor((x - Position.X) / _cellWidth);
            int Y = (int)Math.Floor((y - Position.Y) / _cellHeight);
            return this[X, Y];
        }

        public Point CoordinateOf(Vector2 worldPosition)
        {
            return CoordinateOf(worldPosition.X, worldPosition.Y);
        }
        public Point CoordinateOf(float x, float y)
        {
            int X = (int)Math.Floor((x - Position.X) / _cellWidth);
            int Y = (int)Math.Floor((y - Position.Y) / _cellHeight);

            return new Point(X, Y);
        }

        public void GetCornerCoords(ref Rectangle area, out Point tl, out Point br)
        {
            tl = CoordinateOf(area.Left, area.Top);
            tl.X = (int)MathHelper.Clamp(tl.X, 0, ColCount);
            tl.Y = (int)MathHelper.Clamp(tl.Y, 0, RowCount);

            br = CoordinateOf(area.Right, area.Bottom);
            br.X = (int)MathHelper.Clamp(br.X + 1, 0, ColCount);
            br.Y = (int)MathHelper.Clamp(br.Y + 1, 0, RowCount);
        }

        public List<Point> CoordinateSubset(Rectangle area)
        {
            Point tl = CoordinateOf(area.Left, area.Top);
            tl.X = (int)MathHelper.Clamp(tl.X, 0, ColCount);
            tl.Y = (int)MathHelper.Clamp(tl.Y, 0, RowCount);

            Point br = CoordinateOf(area.Right, area.Bottom);
            br.X = (int)MathHelper.Clamp(br.X + 1, 0, ColCount);
            br.Y = (int)MathHelper.Clamp(br.Y + 1, 0, RowCount);

            List<Point> subset = new List<Point>((br.X - tl.X) * (br.Y - tl.Y));
            if (subset.Capacity > 0)
            {
                for (int row = tl.Y; row < br.Y; row++)
                {
                    for (int col = tl.X; col < br.X; col++)
                    {
                        subset.Add(new Point(col, row));
                    }
                }
            }
            return subset;
        }

        #endregion // Methods

    }
    
}
