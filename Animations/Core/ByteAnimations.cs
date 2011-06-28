using System;

namespace SmallGalaxy_Engine.Animations
{
    public class ByteFromToAnimation : FromToAnimation<byte>
    {
        public ByteFromToAnimation(ClockManager manager) :
            base(manager) { }

        protected override byte Lerp(byte from, byte to, float progress)
        {
            return (byte)(from + (to - from) * progress);
        }
    }
}
