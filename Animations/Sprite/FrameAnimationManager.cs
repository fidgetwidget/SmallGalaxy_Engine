using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using SmallGalaxy_Engine;
using SmallGalaxy_Engine.Animations;
using SmallGalaxy_Engine.Graphics;

namespace SmallGalaxy_Engine.Animations
{
    public class FrameAnimationManager : IDisposable
    {

        #region Fields

        private ClockManager _manager;
        private Frame _frame;
        private Dictionary<string, FrameAnimation> _frameAnimations;
        private FrameAnimation _currentAnimation = null;

        #endregion // Fields

        #region Properties

        public FrameAnimation CurrentAnimation { get { return _currentAnimation; } }

        #endregion // Properties


        #region Init

        public FrameAnimationManager(Frame frame)
        {
            _manager = new ClockManager();
            _frame = frame;
            _frameAnimations = new Dictionary<string, FrameAnimation>();
        }

        public void Dispose()
        {
            _manager = null;
            _frame = null;
            _frameAnimations.Clear();
            _frameAnimations = null;
        }

        #endregion // Init


        #region Update

        public void Update(float elapsedTime)
        {
            _manager.Update(elapsedTime);
        }

        #endregion // Update


        #region Methods

        // Frame Set format needs to be {AnimationName}_{FrameIndex} for this to work
        public void AddFrameAnimation(string name, int fps, FrameSet set)
        {
           AddFrameAnimation(name, fps, set, true);
        }
        public void AddFrameAnimation(string name, int fps, FrameSet set, bool loop)
        {
            if (_frameAnimations.ContainsKey(name))
            {
                throw new ArgumentException(string.Format("Frame Animation {0} Already Exists", name));
            }
            FrameAnimation anim = new FrameAnimation(name, _manager)
            {
                AnimationFrameSet = set,
                FramesPerSecond = fps,
                Duration = (float)(set.Count) / (float)(fps),
                Loop = loop,
                RepeatCount = loop ? int.MaxValue : 0,
                Apply = (v) =>
                {
                    if (v != null)
                    {
                        _frame.SourceRectangle = v.SourceRectangle;
                        _frame.Center = v.Center;
                    }
                }, // the most important part
            };
            _frameAnimations.Add(name, anim);
        }

        public void PlayFrameAnimation(string name)
        {
            if (!_frameAnimations.ContainsKey(name))
            {
                throw new ArgumentException(string.Format("Frame Animation {0} Doesn't Exists", name));
            }
            if (_currentAnimation != _frameAnimations[name])
            {
                if (_currentAnimation != null) { _currentAnimation.Stop(); }
                _currentAnimation = _frameAnimations[name];
                _currentAnimation.Start();
            }
        }

        public void PlayFrameAnimation(string name, int startIndex)
        {
            if (!_frameAnimations.ContainsKey(name))
            {
                throw new ArgumentException(string.Format("Frame Animation {0} Doesn't Exists", name));
            }
            if (_currentAnimation != _frameAnimations[name])
            {
                if (_currentAnimation != null) { _currentAnimation.Stop(); }
                _currentAnimation = _frameAnimations[name];
                _currentAnimation.Start(startIndex);
            }
        }

        public void StopFrameAnimation()
        {
            if (_currentAnimation != null) { _currentAnimation.Stop(); }
        }

        #endregion // Methods

    }
}
