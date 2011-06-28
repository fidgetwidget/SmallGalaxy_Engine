using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine.Animations
{
    public class KeyframeFromToAnimation : FromToAnimation<Keyframe>
    {
        public KeyframeFromToAnimation(ClockManager manager) :
            base(manager) { }

        protected override Keyframe Lerp(Keyframe from, Keyframe to, float progress)
        {
            return new Keyframe()
                {
                    Transform = Matrix.Lerp(from.Transform, to.Transform, progress),
                    Origin = Vector2.Lerp(from.Origin, to.Origin, progress),
                    Tint = Color.Lerp(from.Tint, to.Tint, progress),
                };
        }
    }

    public class KeyframesAnimation : Animation<Keyframe>
    {

        #region Fields

        private string _name;
        private Dictionary<int, Keyframe> _keyframes;
        private int _fps; // frames per second

        #endregion // Fields


        #region Properties

        public string Name { get { return _name; } set { _name = value; } }
        public int FPS { get { return _fps; } set { _fps = value; } }
        public bool Loop { get { return RepeatCount == int.MaxValue; } set { if (value) { RepeatCount = int.MaxValue; } else { RepeatCount = 0; } } }
       
        #endregion // Properties


        #region Init

        public KeyframesAnimation(ClockManager manager) 
            : base(manager) 
        {
            _keyframes = new Dictionary<int, Keyframe>();
        }

        #endregion // Init


        #region Methods

        protected override Keyframe GetCurrentValue(float progress)
        {
            float currentTime = progress * Duration;

            Keyframe prev, next;
            GetKeyframesForTime(currentTime, out prev, out next);
            if (next == null) { return Keyframe.Lerp(prev, prev, 1f); }

            float diff, amount;            
            diff = (next.Index - prev.Index) * _fps;
            amount = (currentTime - (prev.Index * _fps)) / diff;

            return Keyframe.Lerp(prev, next, amount);
        }

        protected void AddKeyframe(Keyframe item)
        {
            if (_keyframes.ContainsKey(item.Index)) { throw new ArgumentException(string.Format("Keyframe at Time {0} already exists", item.Index)); }
            _keyframes.Add(item.Index, item);
        }

        public Keyframe GetKeyframeAt(int index)
        {
            if (!_keyframes.ContainsKey(index)) { return null; }
            return _keyframes[index];
        }
        public void GetKeyframeAt(int index, out Keyframe item)
        {
            if (!_keyframes.ContainsKey(index)) { item = null; return; }
            item = _keyframes[index];
        }

        public void SaveKeyframe(Keyframe item)
        {
            if (!_keyframes.ContainsKey(item.Index)) { AddKeyframe(item); }
            _keyframes[item.Index] = item;
        }

        public void RemoveKeyframe(int index)
        {
            if (_keyframes.ContainsKey(index)) { _keyframes.Remove(index); }
        }

        // This is somewhat slow... and needs to be optimized if its going to be useable on any real scale.
        protected void GetKeyframesForTime(float time, out Keyframe prev, out Keyframe next)
        {
            prev = _keyframes[0];
            next = null;
            foreach (int index in _keyframes.Keys)
            {
                if (time * _fps > index) { prev = _keyframes[index]; }
                else { next = _keyframes[index]; return; }
            }
        }

        #endregion // Methods

    }
}
