using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SmallGalaxy_Engine.Entities;

namespace SmallGalaxy_Engine.Colliders
{
    public class CircleCollider : Collider
    {

        private float _radius = 1f;
        public float Radius { get { return _radius; } set { _radius = value; } }

        #region Init

        public CircleCollider(Entity e, float radius) :
            base(ColliderTypes.Circle)
        {
            _entity = e;
            _radius = radius;
            _localBounds = new AABB(new Vector2(-radius, -radius), new Vector2(radius, radius));
        }
        public CircleCollider(Vector2 position, float radius) :
            base(ColliderTypes.Circle)
        {
            _position = position;
            _radius = radius;
            _localBounds = new AABB(new Vector2(-radius, -radius), new Vector2(radius, radius));
        }

        #endregion // Init

        #region Methods

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

        public override bool IntersectsLine(Vector2 start, Vector2 end, out Vector2 hitPoint)
        {
            throw new NotImplementedException();
        }

        public override bool IntersectsPoint(Vector2 point)
        {
            throw new NotImplementedException();
        }

        #endregion // Methods

    }
}
