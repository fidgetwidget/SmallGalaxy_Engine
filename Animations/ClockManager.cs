using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine
{
    public class ClockManager
    {

        #region Fields

        private List<Clock> _clocks = new List<Clock>();
        private object _tag;

        #endregion // Fields


        #region Properties
        
        protected internal List<Clock> Clocks { get { return _clocks; } }

        // to assign any helpful variables to the Clock Manager (for use in events mostly)
        public object Tag { get { return _tag; } set { _tag = value; } }

        #endregion // Properties


        #region Init

        public ClockManager() { }

        #endregion // Init


        #region Update

        public void Update(float elapsedTime)
        {
            if (_clocks.Count > 0)
            {
                for (int i = 0; i < _clocks.Count; i++)
                {
                    _clocks[i].Update(elapsedTime);
                }
            }
        }

        #endregion // Update

    }
}
