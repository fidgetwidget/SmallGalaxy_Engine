using System;
using System.Text;

namespace SmallGalaxy_Engine.Animations
{

    public class StringWriterAnimation : Animation<string>
    {

        #region Fields

        private string _toAppendTo;
        private string _toWrite;

        #endregion // Fields


        #region Properties

        public string StringToAppendTo { get { return _toAppendTo; } set { _toAppendTo = value; } }
        public string StringToWrite { get { return _toWrite; } set { _toWrite = value; } }

        #endregion // Properties


        #region Events

        public event EventHandler Begin;
        public event EventHandler Complete;

        #endregion // Events


        #region Init

        public StringWriterAnimation(ClockManager manager) :
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

        protected override string GetCurrentValue(float progress)
        {
            return _toAppendTo + _toWrite.Substring(0, (int)((float)_toWrite.Length * progress));
        }

        #endregion // Methods
    }

}
