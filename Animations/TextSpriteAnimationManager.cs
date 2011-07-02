using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using SmallGalaxy_Engine.Animations;
using SmallGalaxy_Engine.Sprites;

namespace SmallGalaxy_Engine.Animations
{

    public class TextSpriteAnimationManager
    {

        #region Fields

        protected TextSprite TextSprite;
        protected ClockManager ClockManager;

        public StringWriterAnimation StringWriterAnimation;

        #endregion // Fields


        #region Init

        public TextSpriteAnimationManager(TextSprite textSprite)
        {
            TextSprite = textSprite;
            ClockManager = new ClockManager();
        }

        #endregion // Init


        #region Update

        public void Update(float elapsedTime)
        {
            ClockManager.Update(elapsedTime);
        }

        #endregion // Update


        #region Methods

        public void SetStringWriterAnimation(string stringToWrite, float duration)
        {
            SetStringWriterAnimation(string.Empty, stringToWrite, duration, 0f);
        }
        public void SetStringWriterAnimation(string stringToWrite, float duration, float beginOffset)
        {
            SetStringWriterAnimation(string.Empty, stringToWrite, duration, 0f);
        }

        public void SetStringWriterAnimation(string stringToAppendTo, string stringToWrite,
            float duration, float beginOffset)
        {
            if (StringWriterAnimation == null)
            { StringWriterAnimation = new StringWriterAnimation(ClockManager); }
            else
            { StringWriterAnimation.Stop(); }

            StringWriterAnimation.StringToAppendTo = stringToAppendTo;
            StringWriterAnimation.StringToWrite = stringToWrite;
            StringWriterAnimation.Duration = duration;
            StringWriterAnimation.BeginOffset = beginOffset;
            StringWriterAnimation.ProgressTransform = null;

            StringWriterAnimation.Apply = (v) =>
            {
                TextSprite.Text = v;
            };
        }

        #endregion // Methods

    }
}
