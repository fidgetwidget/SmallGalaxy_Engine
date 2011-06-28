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
        private Verticies _transformedVertices;
        private Texture2D _texture;

        private bool _isLoaded = false;

        #endregion // Fields


        #region Properties

        /// <summary>
        /// The raw set of points, in "model space".
        /// </summary>        
        public Verticies Vertices { get { return _verticies; } }

        /// <summary>
        /// The transformed points, typically in "world" space
        /// </summary>        
        public Verticies TransformedVertices { get { return _transformedVertices; } }

        public bool IsLoaded { get { return _isLoaded; } }

        #endregion // Properties


        #region Init

        /// <summary>
        /// Constructs a new VectorPolygon object from the given points.
        /// </summary>
        /// <param name="verticies">The raw set of points.</param>
        public Polygon(Verticies verticies)
        {
            _verticies = verticies;
            _transformedVertices = (Verticies)verticies.Clone();
        }
        public Polygon(Vector2[] verticies)
        {
            _verticies = new Verticies(verticies);
            _transformedVertices = new Verticies(verticies);
        }

        public void Load(GraphicsDevice graphics)
        {
            if (_isLoaded) { return; }
            _texture = new Texture2D(graphics, 1, 1);
            Color[] whitePixels = new Color[] { Color.White };
            _texture.SetData<Color>(whitePixels);
            _isLoaded = true;
        }

        #endregion // Init


        #region Methods

        public void Insert(int index, Vector2 point)
        {
            _verticies.Insert(index, point);
        }
        public void Insert(int index, LineSegment segment)
        {
            _verticies.Insert(index, segment);
        }
        public void Insert(int index, Verticies list)
        {
            _verticies.Insert(index, list);
        }

        #endregion // Methods

    }

}
