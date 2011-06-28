using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SmallGalaxy_Engine;
using SmallGalaxy_Engine.Animations;
using SmallGalaxy_Engine.Graphics;

namespace SmallGalaxy_Engine.Animations
{
    /// <summary>
    /// This animation Class assumes that the only thing you will want to change as 
    /// the frame animates is it's source rectangle. 
    /// If you also want to alter its center, flip mode, or texture along a timeline, 
    /// a new frame animation class would need to be made.
    /// </summary>
    public class FrameAnimation : Animation<Frame>
    {

        #region Fields

        private string _name;
        private FrameSet _set;
        private int _fps = 10; // frames per second
        private int _index = 0;

        #endregion // Fields


        #region Properties

        // Frame Set format needs to be {AnimationName}_{FrameIndex} for this to work
        public string AnimationName { get { return _name; } set { _name = value; } }
        public FrameSet AnimationFrameSet { get { return _set; } set { SetFrameSet(value); } }
        public Frame CurrentFrame { get { return _set.GetFrame(string.Format("{0}_{1}", _name, _index)); } }
        public int CurrentIndex { get { return _index; } }

        public int FrameCount { get { return _set.Count; } }
        public int FramesPerSecond { get { return _fps; } set { SetFramesPerSecond(value); } }
        public bool Loop { get { return RepeatCount == int.MaxValue; } set { if (value) { RepeatCount = int.MaxValue; } else { RepeatCount = 0; } } }

        #endregion // Properties


        #region Init

        public FrameAnimation(string animName, ClockManager manager) : this(animName, null, manager) { }

        public FrameAnimation(string animName, FrameSet animationFrameSet, ClockManager manager)
            : base(manager)
        {
            _name = animName;
            _set = animationFrameSet;
            _index = 0;
        }

        public FrameAnimation(FrameAnimation clone)
            : base(clone._manager)
        {
            _name = clone._name;
            _set = clone._set;
            _fps = clone._fps;
            _index = 0;
        }

        #endregion // Init


        #region Methods

        public void Start(int index)
        {
            base.Start();
            totalElapsedTime = (float)index / (float)_set.Count;
        }

        // Frame Set format needs to be {AnimationName}_{FrameIndex} for this to work
        protected void SetFrameSet(FrameSet value)
        {
            _set = value;
            _index = 0;
        }

        protected override void OnBegin()
        {
            _index = 0;
        }

        protected override Frame GetCurrentValue(float progress)
        {
            _index = (int)Math.Floor(progress * _set.Count); // this is a messy way of doing it, but it works.
            return CurrentFrame;
        }

        protected void SetFramesPerSecond(int value)
        {
            _fps = value;
            Duration = _set.Count / _fps; // duration is a result of total frames and frames per second
        }

        #endregion // Methods


    }
}
