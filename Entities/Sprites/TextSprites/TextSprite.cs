using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SmallGalaxy_Engine.Entities;

namespace SmallGalaxy_Engine.Sprites
{
    #region Text Alignment Enums

    public enum TextHorizontalAlignment
    {
        Left,
        Center,
        Right,
    }
    public enum TextVerticalAlignment
    {
        Center,
        Top,
        Bottom,
    }

    #endregion // Text Alignment Enums

    public class TextSprite : Entity, ITintable
    {

        [Flags] // Allows for Multiple to be true
        protected enum FlipFlags
        {
            None = 0,
            FlipHorizontal = 1,
            FlipVertical = 2,
        }

        #region Fields

        private SpriteFont _font;
        private string _text;
        private Color _tint = Color.White;
        private TextHorizontalAlignment _hAlign;
        private TextVerticalAlignment _vAlign;
        private Vector2 _size;

        private FlipFlags _flipMode = FlipFlags.None;

        #endregion // Fields
        

        #region Properties

        public SpriteFont Font { get { return _font; } protected set { _font = value; } }
        public string Text { get { return _text; } set { SetText(value); } }

        public TextHorizontalAlignment HorizontalAlignment { get { return _hAlign; } set { _hAlign = value; } }
        public TextVerticalAlignment VerticalAlignment { get { return _vAlign; } set { _vAlign = value; } }
        public Vector2 Size { get { return _size; } protected set { _size = value; } }
        public Color Tint { get { return _tint; } set { SetTint(value); } }
        public byte Alpha { get { return _tint.A; } set { SetAlpha(value); } }
        
        #endregion // Properties


        #region Init

        protected TextSprite(string name) : base()
        {
            _name = name;
            _hAlign = TextHorizontalAlignment.Left;
            _vAlign = TextVerticalAlignment.Center;
        }

        public TextSprite(string name, SpriteFont font)
            : this(name)
        {
            if (font == null) { throw new ArgumentNullException("font", "null SpriteFonts not accepted"); }
            _font = font;
        }
        
        public TextSprite(string name, SpriteFont font, string text)
            : this(name, font)
        {
            _text = text;
            InvalidateMeasure();
        }

        public TextSprite(TextSprite clone)
        {
            _name = clone._name + "_clone";
            _transform = clone._transform;
            _tint = clone._tint;

            _hAlign = clone.HorizontalAlignment;
            _vAlign = clone.VerticalAlignment;

            _font = clone._font;
            _text = clone._text;
            _size = clone._size;

            _flipMode = clone._flipMode;
        }

        #endregion // Init


        #region Update

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            //TextAnimations.Update(elapsedTime);
        }

        #endregion // Update


        #region Draw

        protected override void DrawCore(SpriteBatch batch, ref Matrix global)
        {
            // if the string is empty, then there is nothing to draw
            if (string.IsNullOrEmpty(_text)) { return; }

            #region Set Center
            Vector2 centerPoint = Vector2.Zero;
            if (HorizontalAlignment == TextHorizontalAlignment.Center)
            {
                centerPoint.X = Size.X * 0.5f;
            }
            else if (HorizontalAlignment == TextHorizontalAlignment.Right)
            {
                centerPoint.X = Size.X;
            }

            if (VerticalAlignment == TextVerticalAlignment.Center)
            {
                centerPoint.Y = Size.Y * 0.5f;
            }
            else if (VerticalAlignment == TextVerticalAlignment.Bottom)
            {
                centerPoint.Y = Size.Y;
            }
            #endregion // Set Center

            Vector2 mPosition, mScale;
            float mRotation;
            MatrixHelper.DecomposeMatrix(ref global, out mPosition, out mRotation, out mScale);

            batch.DrawString(Font, Text, mPosition, _tint, mRotation, centerPoint, mScale,
                (SpriteEffects)_flipMode, 0);
        }

        #endregion // Draw


        #region Methods

        protected virtual void SetText(string text)
        {
            _text = text;
            InvalidateMeasure();
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

        protected virtual void SetFlipMode(FlipFlags flipMode)
        {
            _flipMode = flipMode;
        }

        protected virtual void InvalidateMeasure()
        {
            if (string.IsNullOrEmpty(_text))
                _size = Vector2.Zero;
            else
                _size = _font.MeasureString(_text);
        }

        #endregion // Methods

    }
}
