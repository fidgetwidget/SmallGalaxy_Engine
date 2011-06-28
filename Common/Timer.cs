using System;
using System.Collections.Generic;

namespace SmallGalaxy_Engine
{
    public class Timer : Clock
    {

        #region Events

        public event EventHandler Begin;
        public event EventHandler Complete;
        public event EventHandler Looped;

        #endregion // Events


        #region Init

        public Timer(ClockManager manager) :
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

        #endregion // Methods

    }
}
