using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SmallGalaxy_Engine.Entities;
using SmallGalaxy_Engine.Primitives;

namespace SmallGalaxy_Engine.Colliders
{
    public class PolygonCollider : Collider
    {

        protected Vertices _localvertices, _transformedvertices;
        public Vertices vertices { get { if (_entity != null) { Transform(_entity.GetTransform()); } return _transformedvertices; } }


        #region Init

        public PolygonCollider(Entity e, Vertices vertices) :
            base(ColliderTypes.Polygon)
        {
            _entity = e;
            _localvertices = vertices;
            _transformedvertices = (Vertices)_localvertices.Clone();
            SetBounds();
        }

        public PolygonCollider(Vector2 position, Vertices vertices) :
            base (ColliderTypes.Polygon)
        {
            _position = position;
            _localvertices = vertices;
            _transformedvertices = (Vertices)_localvertices.Clone();
            SetBounds();
        }

        #endregion // Init


        #region Methods

        protected void SetBounds()
        {
            // TODO: set bounds based on Transfromed Vertices
        }
        protected void Transform(Matrix transform)
        {
            // TODO: maybe it would be better to have vertices be array based?
            Vector2[] transformed = new Vector2[_localvertices.Length];
            Vector2.Transform(_localvertices.ToArray(), ref transform, transformed);
            _transformedvertices.Clear();
            _transformedvertices.vertices.InsertRange(0, transformed);
            SetBounds();
        }


        protected override bool CollideRectangle(RectangleCollider other, out CollisionData data)
        {
            // TODO: check collision
            data = new CollisionData(Vector2.Zero);
            return false;
        }

        protected override bool CollideCircle(CircleCollider other, out CollisionData data)
        {
            // TODO: check collision
            data = new CollisionData(Vector2.Zero);
            return false;
        }

        protected override bool CollidePolygon(PolygonCollider other, out CollisionData data)
        {
            // TODO: check collision
            data = new CollisionData(Vector2.Zero);
            return false;
        }

        #endregion // Methods

    }
}
