using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmallGalaxy_Engine.Graphics
{
    public class Frame
    {

        #region Properties

        public Texture2D Texture;
        public Rectangle SourceRectangle;
        public Vector2 Center;
        public SpriteEffects FlipMode;

        #endregion // Properties


        #region Init

        public Frame(Texture2D texture)
        {
            Texture = texture;
            SourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Center = new Vector2((float)texture.Width / 2f, (float)texture.Height / 2f);
            FlipMode = SpriteEffects.None;
        }
        public Frame(Texture2D texture, Rectangle sourceRect)
        {
            Texture = texture;
            SourceRectangle = sourceRect;
            Center = new Vector2((float)sourceRect.Width / 2f, (float)sourceRect.Height / 2f);
            FlipMode = SpriteEffects.None;
        }
        public Frame(Texture2D texture, Vector2 center, SpriteEffects flipMode)
        {
            Texture = texture;
            SourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Center = center;
            FlipMode = flipMode;
            FixCenter();
        }
        public Frame(Texture2D texture, Rectangle sourceRect, Vector2 center, SpriteEffects flipMode)
        {
            Texture = texture;
            SourceRectangle = sourceRect;
            Center = center;
            FlipMode = flipMode;
            FixCenter();
        }

        public Frame(Frame frame)
        {
            Texture = frame.Texture;
            SourceRectangle = frame.SourceRectangle;
            Center = frame.Center;
            FlipMode = frame.FlipMode;
        }

        #endregion // Init


        private void FixCenter()
        {
            if ((FlipMode & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically)
            {
                Center = new Vector2(Center.X, SourceRectangle.Height - Center.Y);
            }

            if ((FlipMode & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
            {
                Center = new Vector2(SourceRectangle.Width - Center.X, Center.Y);
            }
        }

        public override string ToString()
        {
            return string.Format("sr:{0} center:{1} flip:{2}", SourceRectangle, Center, FlipMode); 
        }

    }
}
