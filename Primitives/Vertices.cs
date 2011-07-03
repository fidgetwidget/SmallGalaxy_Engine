using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine.Primitives
{
    public struct Vertices : ICloneable
    {
        public List<Vector2> vertices;
        public int Length { get { return vertices.Count; } }
        public Vector2[] ToArray() { return vertices.ToArray(); }
        public Vector2 this[int i] { get { return vertices[i]; } set { vertices[i] = value; } }

        public Vertices(int capacity)  { vertices = new List<Vector2>(capacity); }
        public Vertices(Vector2[] v)   { vertices = new List<Vector2>(v); }
        public object Clone()           { return new Vertices(vertices.ToArray()); }

        public void Add(Vector2 point)
        {
            if (vertices == null) { vertices = new List<Vector2>(); }
            vertices.Add(point);
        }

        public void Insert(int index, Vector2 point)
        {
            if (vertices == null) { vertices = new List<Vector2>(); }
            vertices.Insert(index, point);
        }
        public void Insert(int index, LineSegment segment)
        {
            Insert(index, segment.end);
            Insert(index, segment.start);
        }
        public void Insert(int index, Vertices list)
        {
            // add them in reverse order to the list at the given index
            for (int i = list.Length; i >= 0; --i)
            {
                Insert(index, list[i]);
            }
        }

        public void Transform(Matrix transform)
        {
            Vector2[] transformed = new Vector2[Length];
            Vector2.Transform(vertices.ToArray(), ref transform, transformed);
            vertices.Clear();
            vertices.InsertRange(0, transformed);
        }

        public void Clear()
        {
            if (vertices == null) { return; }
            vertices.Clear();
        }
    }
}
