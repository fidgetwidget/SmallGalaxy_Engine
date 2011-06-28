using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SmallGalaxy_Engine.Graphics;

namespace SmallGalaxy_Engine.Sprites
{

    public class FrameSprite : Sprite
    {

        #region Fields

        private Frame _frame;

        #endregion // Fields


        #region Properties

        public Frame Frame{ get { return _frame; } set { SetFrame(value); } }
        public float FrameWidth { get { return _frame.SourceRectangle.Width; } }
        public float FrameHeight { get { return _frame.SourceRectangle.Height; } }
        public Vector2 Size { get { return new Vector2(FrameWidth, FrameHeight); } }        

        #endregion // Properties


        #region Init

        protected FrameSprite(string name)
            : base(name) { }
        public FrameSprite(string name, Frame frame)
            : this(name)
        {
            Frame = frame;            
        }
        public FrameSprite(FrameSprite clone)
            : base(clone)
        {
            Frame = clone.Frame;
        }

        public override object Clone()
        {
            return new FrameSprite(this);
        }

        #endregion // Init


        #region Update

        //public override void Update(float elapsedTime)
        //{
        //    base.Update(elapsedTime);
        //    FrameAnimations.Update(elapsedTime);
        //}

        #endregion // Update


        #region Draw

        protected override void DrawCore(SpriteBatch batch, ref Matrix transform)
        {
            if (Frame == null) { return; }
            Vector2 mPosition, mScale;
            float mRotation;

            MatrixHelper.DecomposeMatrix(ref transform, out mPosition, out mRotation, out mScale);

            batch.Draw(Frame.Texture, mPosition, Frame.SourceRectangle, Tint, mRotation, Origin, mScale,
                Frame.FlipMode, 0);
        }

        #endregion // Draw


        #region Methods

        public virtual void SetFrame(Frame value)
        {
            _frame = value;
            base.SetPivot(_frame.Center.X, _frame.Center.Y);
            base.SetFlipMode((FlipFlags)_frame.FlipMode);
        }

        public virtual void SetSourceRectangle(Rectangle sourceRectangle)
        {
            _frame.SourceRectangle = sourceRectangle;
        }

        protected override void SetFlipMode(FlipFlags flipMode)
        {
            base.SetFlipMode(flipMode);
            Frame.FlipMode = (SpriteEffects)flipMode;
        }

        public override void SetPivot(float x, float y)
        {
            base.SetPivot(x, y);
            _frame.Center = Origin;
        }

        #endregion // Methods

    }
}
