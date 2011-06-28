using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SmallGalaxy_Engine.Entities;
using SmallGalaxy_Engine.Sprites;

namespace SmallGalaxy_Engine.Primitives
{

    // Linked List of PathNodes
    public class Path : IEnumerator<PathNode>
    {

        #region Fields

        private PathNode _first = null;
        private PathNode _current = null;

        #endregion // Fileds

        #region Properties

        public PathNode First { get { return _first; } set { _first = value; } }
        public PathNode Current { get { return _current; } }

        #endregion // Properties


        #region Init

        public Path() { }

        public void Dispose()
        {
            PathNode node = null;
            do
            {
                if (node != null) { node.Dispose(); }
                node = _current;
            }
            while (MoveNext());
        }

        #endregion // Init


        #region IEnumerator

        public void Reset() { _current = _first; }
        public bool MoveNext() 
        {
            if (_current.IsLast) { return false; }
            _current = _current.Next;
            return true;
        }        

        object IEnumerator.Current { get { return _current; } }

        #endregion // IEnumerator
    
    }

    public class PathNode : Entity
    {

        #region Fields

        protected PathNode _prev = null;
        protected PathNode _next = null;

        #endregion // Fields

        #region Properties

        public PathNode Next { get { return _next; } set { SetNext(ref value); } }
        public PathNode Prev { get { return _prev; } }

        public bool IsFirst { get { return _prev == null; } }
        public bool IsLast { get { return _next == null; } }

        #endregion // Properties

        #region Init

        public PathNode() : base() { }

        #endregion // Init

        #region Methods

        public void SetNext(ref PathNode node) 
        { 
            this._next = node; 
            node._prev = this; 
        }

        // potentially put a new node between this and it's next neighbor.
        public void InsertNext(ref PathNode node)
        {
            PathNode _oldNext = this._next;

            if (_oldNext != null) { node.SetNext(ref _oldNext); }
            this.SetNext(ref node);
        }

        #endregion // Methods

    }
}
