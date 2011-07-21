using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SmallGalaxy_Engine.Entities;

namespace SmallGalaxy_Engine.Colliders
{

    public enum ColliderTypes
    {
        Rectangle,
        Circle,
        Polygon,
        NumberOfTypes
    }

    public abstract class Collider
    {

        #region Fields

        private ColliderTypes _type;
        protected AABB _localBounds;
        protected Entity _entity = null;
        protected Vector2 _position = Vector2.Zero;

        #endregion // Fields
        
        #region Properties

        public ColliderTypes Type { get { return _type; } }
        public Vector2 Position { get { return _entity == null ? _position : _entity.Position; } }
        public AABB Bounds { get { return Position + _localBounds; } }

        #endregion // Properties

        #region Init

        public Collider(ColliderTypes type) { _type = type; }

        #endregion // Init


        #region Methods

        // Return shortest position offset to resolve the collision
        public virtual Vector2 ResolveCollision(Collider other)
        {
            CollisionData data;
            if (Collide(other, out data))
            {
                if (data.penetration != 0 && data.normal != Vector2.Zero)
                    return data.normal * data.penetration;

                if (data.depth != Vector2.Zero)
                    return new Vector2(
                        data.depth.X < data.depth.Y ? data.depth.X : 0,
                        data.depth.Y < data.depth.X ? data.depth.Y : 0);                
            }
            return Vector2.Zero;
        }

        public virtual bool Collide(Collider other, out CollisionData data)
        {
            switch (other.Type)
            {
                case ColliderTypes.Rectangle:
                    return CollideRectangle(other as RectangleCollider, out data);

                case ColliderTypes.Circle:
                    return CollideCircle(other as CircleCollider, out data);

                case ColliderTypes.Polygon:
                    return CollidePolygon(other as PolygonCollider, out data);

                default:
                    data = new CollisionData(Vector2.Zero);
                    return false;
            }
        }

        // ABSTRACT METHODS
        protected abstract bool CollideRectangle(RectangleCollider other, out CollisionData data);
        protected abstract bool CollideCircle(CircleCollider other, out CollisionData data);
        protected abstract bool CollidePolygon(PolygonCollider other, out CollisionData data);

        public abstract bool IntersectsPoint(Vector2 point);
        public abstract bool IntersectsLine(Vector2 start, Vector2 end, out Vector2 hitPoint);

        #endregion // Methods


        #region Static Collision Methods

        public static bool Collide(Collider a, Collider b, out CollisionData data)
        {
            return a.Collide(b, out data);
        }

        public static bool LinesIntersect_Fast(Vector2 aStart, Vector2 aEnd, Vector2 bStart, Vector2 bEnd)
        {
            Vector2 s1, s2;
            s1 = aEnd - aStart;
            s2 = bEnd - bStart;

            float s, t;
            s = (-s1.Y * (aStart.X - bStart.X) + s1.X * (aStart.Y - bStart.Y)) / (-s2.X * s1.Y + s1.X * s2.Y);
            t = (s2.X * (aStart.Y - bStart.Y) - s2.Y * (aStart.X - bStart.X)) / (-s2.X * s1.Y + s1.X * s2.Y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1) return true;

            return false;
        }
        public static bool LinesIntersect(Vector2 aStart, Vector2 aEnd, Vector2 bStart, Vector2 bEnd, out Vector2 hitPoint)
        {
            Vector2 s1, s2;
            s1 = aEnd - aStart;
            s2 = bEnd - bStart;

            float s, t;
            s = (-s1.Y * (aStart.X - bStart.X) + s1.X * (aStart.Y - bStart.Y)) / (-s2.X * s1.Y + s1.X * s2.Y);
            t = (s2.X * (aStart.Y - bStart.Y) - s2.Y * (aStart.X - bStart.X)) / (-s2.X * s1.Y + s1.X * s2.Y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                hitPoint = aStart + (t * s1);
                return true;
            }
            hitPoint = Vector2.Zero;
            return false;
        }

        #endregion // Static Collision Methods

        

    }
}
