using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SmallGalaxy_Engine.Entities;
using SmallGalaxy_Engine.Primitives;

namespace SmallGalaxy_Engine.Colliders
{
    public class PolygonCollider : Collider
    {

        protected Verticies _localVerticies, _transformedVerticies;
        public Verticies Verticies { get { if (_entity != null) { Transform(_entity.GetTransform()); } return _transformedVerticies; } }


        #region Init

        public PolygonCollider(Entity e, Verticies verticies) :
            base(ColliderTypes.Polygon)
        {
            _entity = e;
            _localVerticies = verticies;
            _transformedVerticies = (Verticies)_localVerticies.Clone();
            SetBounds();
        }

        public PolygonCollider(Vector2 position, Verticies verticies) :
            base (ColliderTypes.Polygon)
        {
            _position = position;
            _localVerticies = verticies;
            _transformedVerticies = (Verticies)_localVerticies.Clone();
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
            // TODO: maybe it would be better to have Verticies be array based?
            Vector2[] transformed = new Vector2[_localVerticies.Length];
            Vector2.Transform(_localVerticies.ToArray(), ref transform, transformed);
            _transformedVerticies.Clear();
            _transformedVerticies.verticies.InsertRange(0, transformed);
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
