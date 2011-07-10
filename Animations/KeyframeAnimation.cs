using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine.Animations
{
    public abstract class KeyframeAnimation<T> : Animation<T>
    {

        #region Fields

        private int _fps;
        private Dictionary<int, T> _keyframes;

        #endregion // Fields

        #region Properties

        public int FramesPerSecond { get { return _fps; } set { _fps = value; } }

        #endregion // Properties

        #region Events

        public event EventHandler Begin;
        public event EventHandler Complete;
        public event EventHandler Looped;

        #endregion // Events


        #region Init

        public KeyframeAnimation(ClockManager manager)
            : base(manager) { }

        #endregion // Init

        #region Methods

        protected override void OnBegin()
        {
            if (Begin != null)
                Begin(this, EventArgs.Empty);

            base.OnBegin();
        }
        protected override void OnCompleted()
        {
            if (Complete != null)
                Complete(this, EventArgs.Empty);

            base.OnCompleted();
        }
        protected override void OnLooped()
        {
            if (Looped != null)
                Looped(this, EventArgs.Empty);

            base.OnLooped();
        }

        protected override T GetCurrentValue(float progress)
        {
            float currentTime = progress * Duration;

            int prev = 0, next = -1;
            foreach (int index in _keyframes.Keys)
            {
                if (currentTime * _fps > index) { prev = index; }
                else { next = index; break; }
            }

            // there is no next keyframe 
            if (next == -1) { return _keyframes[prev]; }

            float diff, amount;
            diff = (next - prev) * _fps;
            amount = (currentTime - (prev * _fps)) / diff;

            return Lerp(_keyframes[prev], _keyframes[next], amount);
        }

        protected abstract T Lerp(T from, T to, float progress);

        public void AddKeyframe(int index, T value)
        {
            if (_keyframes.ContainsKey(index)) { throw new ArgumentException(string.Format("Keyframe at Index {0} already exists", index)); }
            _keyframes.Add(index, value);
        }
        public void SetKeyframe(int index, T value)
        {
            if (!_keyframes.ContainsKey(index)) { AddKeyframe(index, value); return; }
            _keyframes[index] = value;
        }
        public void RemoveKeyframe(int index)
        {
            if (!_keyframes.ContainsKey(index)) { throw new ArgumentException(string.Format("There is no Keyframe at Index {0}", index)); }
            _keyframes.Remove(index);
        }        

        #endregion // Methods

    }
}
