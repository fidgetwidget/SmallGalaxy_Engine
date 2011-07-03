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

        private Verticies _verticies;
        private Verticies _transformedVerticies;

        #endregion // Fields


        #region Properties

        /// <summary>
        /// The raw set of points, in "model space".
        /// </summary>        
        public Verticies Vertices { get { return _verticies; } }

        /// <summary>
        /// The transformed points, typically in "world" space
        /// </summary>        
        public Verticies TransformedVertices { get { return _transformedVerticies; } }

        #endregion // Properties


        #region Init

        /// <summary>
        /// Constructs a new VectorPolygon object from the given points.
        /// </summary>
        /// <param name="verticies">The raw set of points.</param>
        public Polygon(Verticies verticies)
        {
            _verticies = verticies;
            _transformedVerticies = (Verticies)verticies.Clone();
        }
        public Polygon(Vector2[] verticies)
        {
            _verticies = new Verticies(verticies);
            _transformedVerticies = new Verticies(verticies);
        }

        #endregion // Init


        #region Methods

        public void Transform(Verticies value)
        {
            _transformedVerticies = value;
        }

        public void Transform(Matrix transform)
        {
            Vector2[] transformed = new Vector2[_verticies.Length];
            Vector2.Transform(_verticies.ToArray(), ref transform, transformed);
            _transformedVerticies.Clear();
            _transformedVerticies.verticies.InsertRange(0, transformed);
        }

        #endregion // Methods

    }

}
