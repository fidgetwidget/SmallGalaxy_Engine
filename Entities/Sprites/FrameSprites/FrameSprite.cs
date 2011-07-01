using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SmallGalaxy_Engine.Entities;
using SmallGalaxy_Engine.Graphics;

namespace SmallGalaxy_Engine.Sprites
{

    public class FrameSprite : Entity
    {

        [Flags] // Allows for Multiple to be true
        protected enum FlipFlags
        {
            None = 0,
            FlipHorizontal = 1,
            FlipVertical = 2,
        }

        #region Fields

        private Frame _frame;

        private Vector2 _paralax = Vector2.Zero;
        private float _paralaxFactor = 0;
        private Vector2 _origin = Vector2.Zero;
        private Color _tint = Color.White;

        private FlipFlags _flipMode = FlipFlags.None;
        private Rectangle _destinationRect = Rectangle.Empty;
        private bool _paralaxEnabled = true;

        #endregion // Fields


        #region Properties
        
        public Frame Frame{ get { return _frame; } set { SetFrame(value); } }
        public float FrameWidth { get { return _frame.SourceRectangle.Width; } }
        public float FrameHeight { get { return _frame.SourceRectangle.Height; } }
        public Vector2 Size { get { return new Vector2(FrameWidth, FrameHeight); } }

        protected FlipFlags FlipMode { get { return _flipMode; } set { SetFlipMode(value); } }

        public Vector2 Paralax { get { return _paralax; } set { SetParalax(value.X, value.Y); } }
        public float ParalaxFactor { get { return _paralaxFactor; } set { SetParalaxFactor(value); } }
        public Vector2 Origin { get { return _origin; } set { SetPivot(value.X, value.Y); } }
        public Color Tint { get { return _tint; } set { SetTint(value); } }
        public byte Alpha { get { return _tint.A; } set { SetAlpha(value); } }

        public bool IsFitted { get { return !_destinationRect.IsEmpty; } }

        public bool ParalaxEnabled { get { return _paralaxEnabled; } set { _paralaxEnabled = value; } }

        #endregion // Properties


        #region Init

        protected FrameSprite(string name) : base() 
        {
            _name = name;
        }

        public FrameSprite(string name, Frame frame)
            : this(name)
        {
            Frame = frame;
        }
        public FrameSprite(string name, Frame frame, Rectangle destinationRectangle)
            : this(name, frame)
        {
            _destinationRect = destinationRectangle;
        }
        public FrameSprite(FrameSprite clone) : base() 
        {            
            _name = clone._name + "_clone";
            _frame = clone._frame;
            _destinationRect = clone._destinationRect;
            
            _transform = clone._transform;
            _origin = clone._origin;
            _tint = clone._tint;

            _paralaxFactor = clone._paralaxFactor;
            _paralaxEnabled = clone._paralaxEnabled;

            _flipMode = clone._flipMode;
        }

        public override object Clone()
        {
            return new FrameSprite(this);
        }

        #endregion // Init


        #region Draw

        protected override void DrawCore(SpriteBatch batch, ref Matrix transform)
        {
            if (Frame == null) { return; }
            Vector2 mPosition, mScale;
            float mRotation;

            MatrixHelper.DecomposeMatrix(ref transform, out mPosition, out mRotation, out mScale);
            
            if (IsFitted)
            {
                // TODO: destination rectangle as a child entity 
                Rectangle dRect = _destinationRect;
                dRect.X = (int)mPosition.X;
                dRect.Y = (int)mPosition.Y;
                dRect.Width = (int)(_destinationRect.Width * ScaleX);
                dRect.Height = (int)(_destinationRect.Height * ScaleY);

                batch.Draw(Frame.Texture, dRect, Frame.SourceRectangle, _tint, mRotation, _origin,
                    Frame.FlipMode, 0);
            }
            else
                batch.Draw(Frame.Texture, mPosition, Frame.SourceRectangle, _tint, mRotation, _origin, mScale,
                    Frame.FlipMode, 0);
                
        }

        #endregion // Draw


        #region Methods

        public virtual void SetFrame(Frame value)
        {
            _frame = value;
            _origin.X = _frame.Center.X;
            _origin.Y = _frame.Center.Y;
            _flipMode = (FlipFlags)_frame.FlipMode;
        }

        public virtual void SetSourceRectangle(Rectangle sourceRectangle)
        {
            _frame.SourceRectangle = sourceRectangle;
        }

        protected virtual void SetDestinationRect(Rectangle value)
        {
            _destinationRect = value;
            if (IsFitted)
                SetPosition(value.X, value.Y);
        }

        public override void SetPosition(float x, float y)
        {
            base.SetPosition(x, y);
            if (IsFitted)
            {
                _destinationRect.X = (int)x;
                _destinationRect.Y = (int)y;
            }
        }

        protected virtual void SetFlipMode(FlipFlags flipMode)
        {
            _flipMode = flipMode;
            Frame.FlipMode = (SpriteEffects)flipMode;
        }

        protected virtual void SetParalax(float x, float y)
        {
            _paralax.X = x;
            _paralax.Y = y;
        }
        protected virtual void SetParalaxFactor(float value)
        {
            _paralaxFactor = value;
        }

        public Vector2 GetPivot() { return _origin; }
        public virtual void SetPivot(float x, float y)
        {
            _origin.X = x;
            _origin.Y = y;
            _frame.Center = Origin;
        }

        public Color GetTint() { return _tint; }
        public void SetTint(Color value)
        {
            SetTint(value, value.A);
        }
        protected virtual void SetTint(Color rgb, byte alpha)
        {
            _tint = rgb;
            _tint.A = alpha;
        }
        protected virtual void SetAlpha(byte value)
        {
            SetTint(_tint, value);
        }

        #endregion // Methods

    }
}
