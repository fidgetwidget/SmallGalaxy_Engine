using System;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine.Animations
{
    public class Vector2FromToAnimation : FromToAnimation<Vector2>
    {
        public Vector2FromToAnimation(ClockManager manager) :
            base(manager) { }

        protected override Vector2 Lerp(Vector2 from, Vector2 to, float progress)
        {
            return Vector2.Lerp(from, to, progress);
        }
    }
}
