using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine.Colliders
{
    public struct CollisionData
    {
        public Vector2 depth;
        public Vector2 normal;
        public float penetration;

        public CollisionData(Vector2 depth)
        {
            this.depth = depth;
            normal = Vector2.Zero;
            penetration = 0f;
        }

        public CollisionData(Vector2 normal, float penetration)
        {
            this.normal = normal;
            this.penetration = penetration;
            depth = normal * penetration;
        }
    }
}
