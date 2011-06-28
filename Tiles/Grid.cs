using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine
{
    public class Grid<T>
    {

        #region Field

        public int ColCount { get; protected internal set; }
        public int RowCount { get; protected internal set; }

        public bool IsInitialized { get; protected internal set; }
        protected bool isInitialized = false;

        protected T[,] grid;

        #endregion // Field


        #region Init

        public Grid(int colCount, int rowCount)
        {
            ColCount = colCount; // count wide
            RowCount = rowCount; // count hight

            grid = new T[ColCount, RowCount];
        }

        public virtual void Initialize() { isInitialized = true; }

        #endregion // Init


        #region Methods

        public T this[int col, int row]
        {
            get { return GetAt(col, row); }
            set { SetAt(col, row, value); }
        }
        public T this[Point coord]
        {
            get { return GetAt(coord); }
            set { SetAt(coord, value); }
        }

        protected T GetAt(Point coord)
        {
            return GetAt(coord.X, coord.Y);
        }
        protected virtual T GetAt(int col, int row)
        {
            if (col < 0 || col >= ColCount) { throw new ArgumentOutOfRangeException(); }
            if (row < 0 || row >= RowCount) { throw new ArgumentOutOfRangeException(); }
            return grid[col, row];
        }

        protected void SetAt(Point coord, T value)
        {
            SetAt(coord.X, coord.Y, value);
        }
        protected virtual void SetAt(int col, int row, T value)
        {
            if (col < 0 || col >= ColCount) { throw new ArgumentOutOfRangeException(); }
            if (row < 0 || row >= RowCount) { throw new ArgumentOutOfRangeException(); }
            grid[col, row] = value;
        }

        public virtual bool InBounds(Point coord)
        {
            return InBounds(coord.X, coord.Y);
        }
        public virtual bool InBounds(int col, int row)
        {
            if (col < 0 || col >= ColCount) return false;
            if (row < 0 || row >= RowCount) return false;
            return true;
        }        

        #endregion // Methods

    }
}
