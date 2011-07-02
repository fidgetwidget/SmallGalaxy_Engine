using System;
using System.Collections.Generic;
using SmallGalaxy_Engine.Entities;

namespace SmallGalaxy_Engine.Animations
{
    public class TransformFromToAnimation : FromToAnimation<Transform>
    {

        public TransformFromToAnimation(ClockManager manager) :
            base(manager) { }

        protected override Transform Lerp(Transform from, Transform to, float progress)
        {
            return Transform.Lerp(from, to, progress);
        }

    }

    public class TransformKeyframeAnimation : KeyframeAnimation<Transform>
    {

        public TransformKeyframeAnimation(ClockManager manager) :
            base(manager) { }

        protected override Transform Lerp(Transform from, Transform to, float progress)
        {
            return Transform.Lerp(from, to, progress);
        }

    }

}
