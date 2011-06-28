using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SmallGalaxy_Engine.Primitives;
using SmallGalaxy_Engine.Graphics;

namespace SmallGalaxy_Engine.Sprites
{
    public class FittedFrameSprite : FrameSprite
    {

        #region Fields

        private Rectangle _destinationRect;

        #endregion // Fields


        #region Properties

        public Rectangle DestinationRectangle { get { return _destinationRect; } set { SetDestinationRect(value); } }
        public int Width { get { return _destinationRect.Width; } set { _destinationRect.Width = value; } }
        public int Height { get { return _destinationRect.Height; } set { _destinationRect.Height = value; } }

        #endregion // Fields


        #region Init

        protected FittedFrameSprite(string name)
            : base(name) 
        {
            _destinationRect = Rectangle.Empty;
        }

        public FittedFrameSprite(string name, Frame frame)
            : this(name, frame, Rectangle.Empty) { }
        public FittedFrameSprite(string name, Frame frame, Rectangle destinationRectangle)
            : base(name, frame)
        {
            _destinationRect = destinationRectangle;
            if (Frame != null) { Frame.Center = Vector2.Zero; }
        }
        public FittedFrameSprite(FittedFrameSprite clone)
            : base(clone)
        {
            _destinationRect = clone.DestinationRectangle;
        }

        public override object Clone()
        {
            return new FittedFrameSprite(this);
        }

        #endregion // Init


        #region Draw

        protected override void DrawCore(SpriteBatch batch, ref Matrix transform)
        {
            Vector2 mPosition, mScale;
            float mRotation;

            MatrixHelper.DecomposeMatrix(ref transform, out mPosition, out mRotation, out mScale);

            Rectangle dRect = DestinationRectangle;
            dRect.X = (int)mPosition.X;
            dRect.Y = (int)mPosition.Y;
            dRect.Width = (int)(dRect.Width * mScale.X);
            dRect.Height = (int)(dRect.Height * mScale.Y);

            batch.Draw(Frame.Texture, dRect, Frame.SourceRectangle, Tint, mRotation, Frame.Center + Origin,
                Frame.FlipMode, 0);
        }

        #endregion // Draw


        #region Methods

        protected virtual void SetDestinationRect(Rectangle value)
        {
            _destinationRect = value;
            SetPosition(value.X, value.Y);
        }

        public override void SetPosition(float x, float y)
        {
            base.SetPosition(x, y);
            _destinationRect.X = (int)x;
            _destinationRect.Y = (int)y;
        }

        #endregion // Methods

    }
}
