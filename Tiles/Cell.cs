using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SmallGalaxy_Engine.Colliders;
using SmallGalaxy_Engine.Entities;
using SmallGalaxy_Engine.Utils;


namespace SmallGalaxy_Engine
{

    #region TileDirections enum

    /// <summary>
    /// 8 way directions enum
    /// [NW][N][NE] 0, 1, 2
    /// [W] [x] [E] 7,    3
    /// [SW][S][SE] 6, 5, 4
    /// </summary>
    public enum TileDirections
    {
        NorthWest = 0,
        North = 1,
        NorthEast = 2,
        East = 3,
        SouthEast = 4,
        South = 5,
        SouthWest = 6,
        West = 7,
        NumberOfDirections = 8,
    }

    #endregion // TileDirections enum


    #region Tiles Helper Class

    public static class Tiles
    {
        #region Static Methods

        public static Point Neighbor(Point tileCoordinate, TileDirections direction)
        {
            Point offset = Offset(tileCoordinate, direction);
            tileCoordinate.X += offset.X;
            tileCoordinate.Y += offset.Y;
            return tileCoordinate;
        }
        public static Point Offset(Point tileCoordinate, TileDirections direction)
        {
            Point offset = new Point(0, 0);
            switch (direction)
            {
                case TileDirections.NorthWest: offset = new Point(-1, -1); break;
                case TileDirections.North: offset = new Point(0, -1); break;
                case TileDirections.NorthEast: offset = new Point(1, -1); break;
                case TileDirections.West: offset = new Point(-1, 0); break;
                case TileDirections.East: offset = new Point(1, 0); break;
                case TileDirections.SouthWest: offset = new Point(-1, 1); break;
                case TileDirections.South: offset = new Point(0, 1); break;
                case TileDirections.SouthEast: offset = new Point(1, 1); break;
                default: break;
            }
            return offset;
        }

        #endregion // Static Methods
    }

    #endregion // Tiles Helper Class


    public class Cell : ITileable<Cell>
    {

        #region Fields

        private Map<Cell> _map;
        private Point _coord;

        #endregion // Fields


        #region Properties

        public Point Coord { get { return _coord; } protected internal set { _coord = value; } }
        public int Col { get { return _coord.X; } protected internal set { _coord.X = value; } }
        public int Row { get { return _coord.Y; } protected internal set { _coord.Y = value; } }

        public int Width { get { return _map.CellWidth; } }
        public int Height { get { return _map.CellHeight; } }

        #endregion // Properties


        #region Methods

        public void SetMap(Map<Cell> map) { _map = map; }
        public void SetCoord(int x, int y) { _coord.X = x; _coord.Y = y; }
        public Point GetCoord() { return _coord; }

        #endregion // Methods



    }

    // Generic Cell (of Entity)
    public class Cell<T> : ITileable<Cell<T>> where T : Entity
    {

        #region Fields

        private Map<Cell<T>> _map;
        private Point _coord; // grid coordinates
        private T _entity; // The Tile data

        #endregion // Fields


        #region Properties

        public Map<Cell<T>> Map { get { return _map; } protected internal set { _map = value; } }

        public Point Coord { get { return _coord; } protected internal set { _coord = value; } }
        public int Col { get { return _coord.X; } protected internal set { _coord.X = value; } }
        public int Row { get { return _coord.Y; } protected internal set { _coord.Y = value; } }

        public int Width { get { return _map.CellWidth; } }
        public int Height { get { return _map.CellHeight; } }

        public Vector2 Position { get { return GetPosition(); } }
        public float X { get { return GetPosition().X; } }
        public float Y { get { return GetPosition().Y; } }
        public Vector2 Center { get { return GetCenter(); } }
        public AABB Bounds { get { return GetBounds(); } }

        public T Entity { get { return _entity; } }
        public bool IsEmpty { get { return _entity == null; } }

        #endregion // Properties


        #region Events

        public event EventHandler EntityAddedEvent;
        public event EventHandler EntityRemovedEvent;

        #endregion // Events


        #region Init

        public Cell() { }

        #endregion // Init


        #region Methods

        public void SetMap(Map<Cell<T>> map)
        {
            _map = map;
        }
        public void SetCoord(int x, int y)
        {
            _coord.X = x;
            _coord.Y = y;
        }
        public Point GetCoord()
        {
            return _coord;
        }

        public Vector2 GetPosition()
        {
            float x, y;
            x = (Width * Col) + _map.Position.X;
            y = (Height * Row) + _map.Position.Y;
            return new Vector2(x, y);
        }
        public Vector2 GetCenter()
        {
            float x, y;
            x = Width / 2;
            y = Height / 2;
            return new Vector2(x, y);
        }
        public AABB GetBounds()
        {
            return new AABB(Position, Width, Height);
        }

        public void SetEntity(T entity)
        {
            if (_entity != null) { RemoveEntity(); }
            if (entity != null)
            {
                _entity = entity;
                _entity.SetPosition(X, Y);
            }
            if (EntityAddedEvent != null) { EntityAddedEvent(this, EventArgs.Empty); }
        }
        public void RemoveEntity()
        {
            if (_entity == null) { return; }
            _entity = null;

            if (EntityRemovedEvent != null) { EntityRemovedEvent(this, EventArgs.Empty); }
        }

        public Cell<T> GetNeighbor(TileDirections direction)
        {
            return _map[Tiles.Neighbor(_coord, direction)];
        }

        #endregion // Methods


        #region Operators

        public static bool operator !=(Cell<T> a, Cell<T> b)
        {
            if ((object)a == null)
            {
                return (object)b != null;
            }

            return !a.Equals(b);
        }
        public static bool operator ==(Cell<T> a, Cell<T> b)
        {
            if ((object)a == null)
            {
                return (object)b == null;
            }

            return a.Equals(b);
        }

        public override int GetHashCode()
        {
            // TODO : generate a hash
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            Cell<T> item;
            if (obj is Cell<T>)
            {
                item = (Cell<T>)obj;
                if (item._map == _map &&
                    item._coord == _coord)
                    return true;
            }

            return false;
        }

        #endregion // Operator

    }
}