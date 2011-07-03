using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SmallGalaxy_Engine.Primitives
{

    public class Polygon
    {

        #region Fields

        private Vertices _vertices;
        private Vertices _transformedvertices;

        #endregion // Fields


        #region Properties

        /// <summary>
        /// The raw set of points, in "model space".
        /// </summary>        
        public Vertices Vertices { get { return _vertices; } }

        /// <summary>
        /// The transformed points, typically in "world" space
        /// </summary>        
        public Vertices TransformedVertices { get { return _transformedvertices; } }

        #endregion // Properties


        #region Init

        /// <summary>
        /// Constructs a new VectorPolygon object from the given points.
        /// </summary>
        /// <param name="vertices">The raw set of points.</param>
        public Polygon(Vertices vertices)
        {
            _vertices = vertices;
            _transformedvertices = (Vertices)vertices.Clone();
        }
        public Polygon(Vector2[] vertices)
        {
            _vertices = new Vertices(vertices);
            _transformedvertices = new Vertices(vertices);
        }

        #endregion // Init


        #region Methods

        public void Transform(Vertices value)
        {
            _transformedvertices = value;
        }

        public void Transform(Matrix transform)
        {
            Vector2[] transformed = new Vector2[_vertices.Length];
            Vector2.Transform(_vertices.ToArray(), ref transform, transformed);
            _transformedvertices.Clear();
            _transformedvertices.vertices.InsertRange(0, transformed);
        }

        #endregion // Methods

    }

}
