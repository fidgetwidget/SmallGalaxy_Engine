using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SmallGalaxy_Engine.Sprites;

namespace SmallGalaxy_Engine.Animations
{
    public class SpriteAnimationManager
    {

        #region Fields

        protected Sprite _sprite;
        protected ClockManager _clockManager;

        #endregion // Fields


        #region Animations

        public Vector2FromToAnimation PositionAnimation;
        public FloatFromToAnimation RotationAnimation;
        public FloatFromToAnimation ParalaxAnimation;
        public Vector2FromToAnimation ScaleAnimation;
        public ColorFromToAnimation TintAnimation;
        public ByteFromToAnimation AlphaAnimation;

        #endregion // Animations


        #region Init

        public SpriteAnimationManager(Sprite sprite)
        {
            _sprite = sprite;
            _clockManager = new ClockManager();
        }

        #endregion // Init


        #region Update

        public void Update(float elapsedTime)
        {
            _clockManager.Update(elapsedTime);
        }

        #endregion // Update


        #region Methods

        #region Set Position Animation

        public void SetPositionAnimation(Vector2 to, float duration)
        {
            SetPositionAnimation(_sprite.Position, to, duration, 0f, false, 0);
        }
        public void SetPositionAnimation(Vector2 from, Vector2 to, float duration)
        {
            SetPositionAnimation(from, to, duration, 0f, false, 0);
        }
        public void SetPositionAnimation(Vector2 from, Vector2 to, float duration,
            float beginOffset, bool autoReverse, int repeatCount)
        {
            if (PositionAnimation == null)
            { PositionAnimation = new Vector2FromToAnimation(_clockManager); }
            else
            { PositionAnimation.Stop(); }

            PositionAnimation.From = from;
            PositionAnimation.To = to;
            PositionAnimation.Duration = duration;
            PositionAnimation.BeginOffset = beginOffset;
            PositionAnimation.AutoReverse = autoReverse;
            PositionAnimation.RepeatCount = repeatCount;
            PositionAnimation.ProgressTransform = null;

            PositionAnimation.Apply = (v) =>
            {
                _sprite.Position = v;
            };
        }

        #endregion // Set Position Animation

        #region Set Rotation Animation

        public void SetRotationAnimation(float to, float duration)
        {
            SetRotationAnimation(_sprite.Rotation, to, duration, 0f, false, 0);
        }
        public void SetRotationAnimation(float from, float to, float duration)
        {
            SetRotationAnimation(from, to, duration, 0f, false, 0);
        }
        public void SetRotationAnimation(float from, float to, float duration,
            float beginOffset, bool autoReverse, int repeatCount)
        {
            if (RotationAnimation == null)
            { RotationAnimation = new FloatFromToAnimation(_clockManager); }
            else
            { RotationAnimation.Stop(); }

            RotationAnimation.From = from;
            RotationAnimation.To = to;
            RotationAnimation.Duration = duration;
            RotationAnimation.BeginOffset = beginOffset;
            RotationAnimation.AutoReverse = autoReverse;
            RotationAnimation.RepeatCount = repeatCount;
            RotationAnimation.ProgressTransform = null;

            RotationAnimation.Apply = (v) =>
            {
                _sprite.Rotation = v;
            };
        }

        #endregion // Set Rotation Animation

        #region Set Paralax Animation

        public void SetParalaxAnimation(float to, float duration)
        {
            SetParalaxAnimation(_sprite.ParalaxFactor, to, duration, 0f, false, 0);
        }
        public void SetParalaxAnimation(float from, float to, float duration)
        {
            SetParalaxAnimation(from, to, duration, 0f, false, 0);
        }
        public void SetParalaxAnimation(float from, float to, float duration,
            float beginOffset, bool autoReverse, int repeatCount)
        {
            if (ParalaxAnimation == null)
            { ParalaxAnimation = new FloatFromToAnimation(_clockManager); }
            else
            { ParalaxAnimation.Stop(); }

            ParalaxAnimation.From = from;
            ParalaxAnimation.To = to;
            ParalaxAnimation.Duration = duration;
            ParalaxAnimation.BeginOffset = beginOffset;
            ParalaxAnimation.AutoReverse = autoReverse;
            ParalaxAnimation.RepeatCount = repeatCount;
            ParalaxAnimation.ProgressTransform = null;

            ParalaxAnimation.Apply = (v) =>
            {
                _sprite.ParalaxFactor = v;
            };
        }

        #endregion // Set Paralax Animation

        #region Set Scale Animation

        public void SetScaleAnimation(Vector2 to, float duration)
        {
            SetScaleAnimation(_sprite.Scale, to, duration, 0f, false, 0);
        }
        public void SetScaleAnimation(Vector2 from, Vector2 to, float duration)
        {
            SetScaleAnimation(from, to, duration, 0f, false, 0);
        }
        public void SetScaleAnimation(Vector2 from, Vector2 to, float duration,
            float beginOffset, bool autoReverse, int repeatCount)
        {
            if (ScaleAnimation == null)
            { ScaleAnimation = new Vector2FromToAnimation(_clockManager); }
            else
            { ScaleAnimation.Stop(); }

            ScaleAnimation.From = from;
            ScaleAnimation.To = to;
            ScaleAnimation.Duration = duration;
            ScaleAnimation.BeginOffset = beginOffset;
            ScaleAnimation.AutoReverse = autoReverse;
            ScaleAnimation.RepeatCount = repeatCount;
            ScaleAnimation.ProgressTransform = null;

            ScaleAnimation.Apply = (v) =>
            {
                _sprite.Scale = v;
            };
        }

        #endregion // Set Scale Animation

        #region Set Tint Animation

        public void SetTintAnimation(Color to, float duration)
        {
            SetTintAnimation(_sprite.Tint, to, duration, 0f, false, 0);
        }
        public void SetTintAnimation(Color from, Color to, float duration)
        {
            SetTintAnimation(from, to, duration, 0f, false, 0);
        }
        public void SetTintAnimation(Color from, Color to, float duration,
            float beginOffset, bool autoReverse, int repeatCount)
        {
            if (TintAnimation == null)
            { TintAnimation = new ColorFromToAnimation(_clockManager); }
            else
            { TintAnimation.Stop(); }

            TintAnimation.From = from;
            TintAnimation.To = to;
            TintAnimation.Duration = duration;
            TintAnimation.BeginOffset = beginOffset;
            TintAnimation.AutoReverse = autoReverse;
            TintAnimation.RepeatCount = repeatCount;
            TintAnimation.ProgressTransform = null;

            TintAnimation.Apply = (v) =>
            {
                _sprite.Tint = v;
            };
        }

        #endregion // Set Tint Animation

        #region Set Alpha Animation

        public void SetAlphaAnimation(Byte to, float duration)
        {
            SetAlphaAnimation(_sprite.Alpha, to, duration, 0f, false, 0);
        }
        public void SetAlphaAnimation(Byte from, Byte to, float duration)
        {
            SetAlphaAnimation(from, to, duration, 0f, false, 0);
        }
        public void SetAlphaAnimation(Byte from, Byte to, float duration,
            float beginOffset, bool autoReverse, int repeatCount)
        {
            if (AlphaAnimation == null)
            { AlphaAnimation = new ByteFromToAnimation(_clockManager); }
            else
            { AlphaAnimation.Stop(); }

            AlphaAnimation.From = from;
            AlphaAnimation.To = to;
            AlphaAnimation.Duration = duration;
            AlphaAnimation.BeginOffset = beginOffset;
            AlphaAnimation.AutoReverse = autoReverse;
            AlphaAnimation.RepeatCount = repeatCount;
            AlphaAnimation.ProgressTransform = null;

            AlphaAnimation.Apply = (v) =>
            {
                _sprite.Alpha = v;
            };
        }

        #endregion // Set Alpha Animation

        #endregion // Methods

    }
}
