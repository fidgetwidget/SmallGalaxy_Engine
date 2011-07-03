using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SmallGalaxy_Engine.Entities;

namespace SmallGalaxy_Engine.Colliders
{

    // NOTE: the Rectangle Collider doesn't rotate, so if you want a rotateable Rectangle, use the Polygon Collider
    public class RectangleCollider : Collider
    {
        
        #region Init

        public RectangleCollider(Entity e, int width, int height) :
            base(ColliderTypes.Rectangle)
        {
            _entity = e;
            _localBounds = new AABB(0, 0, width, height);
        }

        public RectangleCollider(Vector2 position, int width, int height) :
            base(ColliderTypes.Rectangle)
        {
            _position = position;
            _localBounds = new AABB(0, 0, width, height);
        }

        #endregion // Init


        #region Methods

        protected override bool CollideRectangle(RectangleCollider other, out CollisionData data)
        {
            Vector2 depth = AABB.GetIntersectionDepth(Bounds, other.Bounds);
            if (depth != Vector2.Zero && (Math.Abs(depth.X) > 1 || Math.Abs(depth.Y) > 1))
            {
                data = new CollisionData(depth);
                return true;
            }
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
