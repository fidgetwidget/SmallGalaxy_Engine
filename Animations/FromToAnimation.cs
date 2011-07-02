using System;
using System.Collections.Generic;

namespace SmallGalaxy_Engine.Animations
{
    public abstract class FromToAnimation<T> : Animation<T>
    {

        #region Fields

        private Func<T, T, float, T> _interpolate;
        private T _from;
        private T _to;

        #endregion // Fields

        #region Properties

        public Func<T, T, float, T> Interpolate { get { return _interpolate; } set { _interpolate = value; } }
        public T From { get { return _from; } set { _from = value; } }
        public T To { get { return _to; } set { _to = value; } }

        #endregion // Properties


        #region Events

        public event EventHandler Begin;
        public event EventHandler Complete;
        public event EventHandler Looped;

        #endregion // Events


        #region Init

        public FromToAnimation(ClockManager manager) :
            base(manager) { }

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
            var interpolate = Interpolate;
            if (interpolate != null)
            {
                return interpolate(_from, _to, progress);
            }

            return Lerp(_from, _to, progress);
        }

        protected abstract T Lerp(T from, T to, float progress);

        #endregion // Methods

    }
}
