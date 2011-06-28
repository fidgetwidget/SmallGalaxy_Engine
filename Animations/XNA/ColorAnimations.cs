using System;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine.Animations
{
    public class ColorFromToAnimation : FromToAnimation<Color>
    {
        public ColorFromToAnimation(ClockManager manager) :
            base(manager) { }

        protected override Color Lerp(Color from, Color to, float progress)
        {
            return Color.Lerp(from, to, progress);
        }
    }
}
