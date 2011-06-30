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

    public class Map<T> : Grid<Cell<T>> where T : Entity, ITileable
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

        public int Width { get { return _cellWidth; } }
        public int Height { get { return _cellHeight; } }

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
                    if (populate)
                        grid[col, row] = new Cell<T>(this);
                    else
                        if (grid[col, row] == null) { continue; }

                    grid[col, row].Map = this;
                    grid[col, row].Col = col;
                    grid[col, row].Row = row;
                }
            }
            IsInitialized = true;
        }

        #endregion // Init


        #region Methods

        public void AddTile(Point coord) { AddTile(coord.X, coord.Y); }
        public virtual void AddTile(int col, int row) { SetAt(col, row, new Cell<T>(this)); }

        protected override void SetAt(int col, int row, Cell<T> value)
        {
            if (col < 0 || col >= ColCount) return;
            if (row < 0 || row >= RowCount) return;
            grid[col, row] = value;
            if (value == null) return;

            value.Map = this;
            value.Col = col;
            value.Row = row;
        }

        protected void SetPosition(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
        }

        public Cell<T> GetNeighborCell(Point coord, TileDirections direction)
        {
            Point nCoord = GetNeighbor(coord, direction);
            if (InBounds(nCoord))
                return this[nCoord];
            else
                return null;
        }

        public Point GetNeighbor(Point coord, TileDirections direction)
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
                return GetNeighbor(neighbor, direction);

            return neighbor;
        }

        public Cell<T> AtPoint(Vector2 point)
        {
            return AtPoint(point.X, point.Y);
        }
        public Cell<T> AtPoint(float x, float y)
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

        // because this is the one we will use almost every time, it can have the default name
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

        // Because Lists are faster to loop through, this is only to avoid using List.ToArray()
        public Point[] CoordinateSubsetArray(Rectangle area)
        {
            Point tl, br;
            GetCornerCoords(ref area, out tl, out br);

            Point[] subset = new Point[((br.X - tl.X) * (br.Y - tl.Y))];
            if (subset.Length > 0)
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

        #endregion // Methods

    }


    /* Non Generic Version of the Map
    public class Map : Grid<Cell>
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
        
        public int Width { get { return _cellWidth; } }
        public int Height { get { return _cellHeight; } }

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
                    if (populate)
                        grid[col, row] = new Cell(this);
                    else
                        if (grid[col, row] == null) { continue; }

                    grid[col, row].Grid = this;
                    grid[col, row].Col = col;
                    grid[col, row].Row = row;
                }
            }
            IsInitialized = true;
        }

        #endregion // Init


        #region Methods
        
        public void AddTile(Point coord) { AddTile(coord.X, coord.Y); }
        public virtual void AddTile(int col, int row) { SetAt(col, row, new Cell(this)); }

        protected override void SetAt(int col, int row, Cell value)
        {
            if (col < 0 || col >= ColCount) return;
            if (row < 0 || row >= RowCount) return;
            grid[col, row] = value;
            if (value == null) return;

            value.Grid = this;
            value.Col = col;
            value.Row = row;
        }

        protected void SetPosition(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
        }

        public Cell GetNeighborCell(Point coord, TileDirections direction)
        {
            return this[GetNeighbor(coord, direction)];
        }

        public Point GetNeighbor(Point coord, TileDirections direction)
        {
            Point neighbor = Cell.Neighbor(coord, direction);
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

        public Cell AtPoint(Vector2 point)
        {
            return AtPoint(point.X, point.Y);
        }
        public Cell AtPoint(float x, float y)
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

        // because this is the one we will use almost every time, it can have the default name
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

        // Because Lists are faster to loop through, this is only to avoid using List.ToArray()
        public Point[] CoordinateSubsetArray(Rectangle area)
        {
            Point tl, br;
            GetCornerCoords(ref area, out tl, out br);

            Point[] subset = new Point[((br.X - tl.X) * (br.Y - tl.Y))];
            if (subset.Length > 0)
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

        #endregion // Methods
        
    }
    //*/
    
}
