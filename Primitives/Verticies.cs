using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine.Primitives
{
    public struct Verticies : ICloneable
    {
        public List<Vector2> verticies;
        public int Length { get { return verticies.Count; } }
        public Vector2[] ToArray() { return verticies.ToArray(); }
        public Vector2 this[int i] { get { return verticies[i]; } set { verticies[i] = value; } }

        public Verticies(int capacity)  { verticies = new List<Vector2>(capacity); }
        public Verticies(Vector2[] v)   { verticies = new List<Vector2>(v); }
        public object Clone()           { return new Verticies(verticies.ToArray()); }

        public void Add(Vector2 point)
        {
            if (verticies == null) { verticies = new List<Vector2>(); }
            verticies.Add(point);
        }

        public void Insert(int index, Vector2 point)
        {
            if (verticies == null) { verticies = new List<Vector2>(); }
            verticies.Insert(index, point);
        }
        public void Insert(int index, LineSegment segment)
        {
            Insert(index, segment.end);
            Insert(index, segment.start);
        }
        public void Insert(int index, Verticies list)
        {
            // add them in reverse order to the list at the given index
            for (int i = list.Length; i >= 0; --i)
            {
                Insert(index, list[i]);
            }
        }

        public void Clear()
        {
            if (verticies == null) { return; }
            verticies.Clear();
        }
    }
}
