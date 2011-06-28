using System;

namespace SmallGalaxy_Engine.Animations
{
    public class FloatFromToAnimation : FromToAnimation<float>
    {
        public FloatFromToAnimation(ClockManager manager) :
            base(manager) { }

        protected override float Lerp(float from, float to, float progress)
        {
            return from + (to - from) * progress;
        }
    }
}
