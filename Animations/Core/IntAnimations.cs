using System;

namespace SmallGalaxy_Engine.Animations
{
    public class IntFromToAnimation : FromToAnimation<int>
    {
        public IntFromToAnimation(ClockManager manager) :
            base(manager) { }

        protected override int Lerp(int from, int to, float progress)
        {
            return (int)(from + (to - from) * progress);
        }
    }

    public class IntKeyframeAnimation : KeyframeAnimation<int>
    {
        public IntKeyframeAnimation(ClockManager manager) :
            base(manager) { }

        protected override int Lerp(int from, int to, float progress)
        {
            return (int)(from + (to - from) * progress);
        }
    }

}
