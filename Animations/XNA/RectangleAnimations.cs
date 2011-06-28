using System;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine.Animations
{
    public class RectangleFromToAnimation : FromToAnimation<Rectangle>
    {
        public RectangleFromToAnimation(ClockManager manager) :
            base(manager) { }

        protected override Rectangle Lerp(Rectangle from, Rectangle to, float progress)
        {
            int x = (int)(from.X + (to.X - from.X) * progress);
            int y = (int)(from.Y + (to.Y - from.Y) * progress);

            int width = (int)(from.Width + (to.Width - from.Width) * progress);
            int height = (int)(from.Height + (to.Height - from.Height) * progress);

            return new Rectangle(x, y, width, height);
        }
    }
}
