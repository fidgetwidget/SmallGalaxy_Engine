using System;
using System.Collections.Generic;

namespace SmallGalaxy_Engine.Animations
{
    // An Animation<T> is a clock that produces a value of type T
    // on each tick of the clock.
    public abstract class Animation<T> : Clock
    {

        #region Fields

        private Func<float, float> _progressTransform;
        private Action<T> _apply;
        private T _currentValue;

        #endregion // Fields


        #region Properties

        public Func<float, float> ProgressTransform { get { return _progressTransform; } set { _progressTransform = value; } }
        public Action<T> Apply { get { return _apply; } set { _apply = value; } }
        public T CurrentValue { get { return _currentValue; } }

        #endregion // Properties


        #region Init

        public Animation(ClockManager manager) :
            base(manager) { }

        #endregion // Init


        #region Methods

        protected override void OnTicked(float progress)
        {
            var progressTransform = _progressTransform;
            if (progressTransform != null)
            {
                progress = progressTransform(progress);
            }

            _currentValue = GetCurrentValue(progress);
            OnApply(_currentValue);
        }

        protected virtual void OnApply(T currentValue)
        {
            var apply = _apply;
            if (apply != null)
            {
                apply(currentValue);
            }
        }

        protected abstract T GetCurrentValue(float progress);

        #endregion // Methods

    }
    
}
