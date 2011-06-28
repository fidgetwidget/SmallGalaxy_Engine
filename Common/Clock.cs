using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine
{
    public enum ClockState
    {
        Playing,
        Paused,
        Stopped,
    }

    public class Clock
    {

        #region Fields

        private bool _autoReverse = false;
        private int _repeatCount = 0;
        private float _beginOffset = 0;
        private float _duration = 1.0f;

        private float _progress = 0;
        private ClockState _state = ClockState.Stopped;
        internal ClockManager _manager;

        protected float totalElapsedTime = 0;

        #endregion // Fields


        #region Properties

        public bool AutoReverse { get { return _autoReverse; } set { _autoReverse = value; } }
        public int RepeatCount { get { return _repeatCount; } set { _repeatCount = value; } }
        public float BeginOffset { get { return _beginOffset; } set { _beginOffset = value; } }
        public float Duration { get { return _duration; } set { _duration = value; } }

        public float Progress { get { return _progress; } }
        public ClockState State { get { return _state; } }
        public ClockManager ClockManager { get { return _manager; } }

        #endregion // Properties


        #region Init

        protected Clock(ClockManager manager)
        {
            _manager = manager;
        }

        #endregion // Init


        #region Update

        internal void Update(float elapsedTime)
        {
            System.Diagnostics.Debug.Assert(State == ClockState.Playing);

            var beginOffset = _beginOffset;
            var oldTotalElapsedTime = totalElapsedTime;
            totalElapsedTime += elapsedTime;

            if (totalElapsedTime >= beginOffset)
            {
                if (oldTotalElapsedTime <= beginOffset)
                {
                    OnBegin();
                }

                var effectiveElapsedTime = totalElapsedTime - beginOffset;
                var oldEffectiveElapsedTime = oldTotalElapsedTime - beginOffset;
                var duration = _duration;
                var segment = (int)(effectiveElapsedTime / duration);
                var oldSegment = (int)(oldEffectiveElapsedTime / duration);
                var autoReverse = _autoReverse;
                var totalSegments = (long)RepeatCount * (autoReverse ? 2L : 1L) + (autoReverse ? 1L : 0L);

                if (segment > totalSegments)
                {
                    // completed
                    _progress = (autoReverse ? 0.0f : 1.0f);
                    Stop();
                    OnTicked(Progress);
                    OnCompleted();
                }
                else
                {
                    _progress = (float)((effectiveElapsedTime % duration) / duration);

                    // odd loops are fliped if we're auto-reversing
                    if (autoReverse && segment % 2 == 1)
                    {
                        _progress = 1.0f - _progress;
                    }

                    for (int i = oldSegment; i < segment; i++)
                    {
                        // in autoReverse mode, 'loop' only once we've gone there & back
                        // i.e. 2 'loops'
                        if (!autoReverse || i % 2 == 1)
                        {
                            OnLooped();
                        }
                    }

                    OnTicked(Progress);
                }
            }
        }

        #endregion // Update


        #region Methods

        public virtual void Start()
        {
            switch (_state)
            {
                case ClockState.Paused:
                    Reset();
                    ClockManager.Clocks.Add(this);
                    break;
                case ClockState.Playing:
                    // we dont need to add because these states should already be added.
                    Reset();
                    break;
                case ClockState.Stopped:
                    // we don't need to reset, because stopped clocks already are // no they aren't!
                    Reset();
                    ClockManager.Clocks.Add(this);
                    break;
            }

            _state = ClockState.Playing;

            if (_beginOffset == 0.0)
            {
                // clocks without BeginOffset update syncronously
                Update(0.0f);
            }
        }

        public virtual void Stop()
        {
            switch (_state)
            {
                case ClockState.Paused:
                case ClockState.Playing:
                    ClockManager.Clocks.Remove(this);
                    break;
                case ClockState.Stopped:
                    // nothing to do, we're already stopped
                    break;
            }
            // was reset suposed to go here? Reset();
            _state = ClockState.Stopped;
        }

        public virtual void Pause()
        {
            switch (_state)
            {
                case ClockState.Paused:
                    // success!
                    break;
                case ClockState.Playing:
                    ClockManager.Clocks.Remove(this);
                    break;
                case ClockState.Stopped:
                    // huh?  ignore it.
                    break;
            }

            _state = ClockState.Paused;
        }

        public virtual void Resume()
        {
            switch (_state)
            {
                case ClockState.Paused:
                    ClockManager.Clocks.Add(this);
                    break;
                case ClockState.Playing:
                    // huh? ignore it.
                    break;
                case ClockState.Stopped:
                    ClockManager.Clocks.Add(this);
                    break;
            }

            _state = ClockState.Playing;
        }

        public virtual void SkipToBegin()
        {
            if (totalElapsedTime < _beginOffset)
            {
                totalElapsedTime = _beginOffset;
            }
        }

        public virtual void Reset()
        {
            totalElapsedTime = 0.0f;
            _progress = 0.0f;
        }

        protected virtual void OnBegin() { }
        protected virtual void OnTicked(float progress) { }
        protected virtual void OnLooped() { }
        protected virtual void OnCompleted() { }

        #endregion // Methods

    }
}
