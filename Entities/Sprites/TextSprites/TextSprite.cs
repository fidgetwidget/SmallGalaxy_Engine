using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SmallGalaxy_Engine.Animations;

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

    public enum TextSpriteTypes
    {
        Text,
        NumericText,
        Dialog,
        NumberOfTypes
    }

    public class TextSprite : Sprite
    {

        #region Fields

        private SpriteFont _font;
        private string _text;
        private TextHorizontalAlignment _hAlign;
        private TextVerticalAlignment _vAlign;
        private Vector2 _size;

        private TextSpriteAnimationManager _textAnimations;

        #endregion // Fields
        

        #region Properties

        public SpriteFont Font { get { return _font; } protected set { _font = value; } }
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                InvalidateMeasure();
                TextChanged();
            }
        }
        public TextSpriteAnimationManager TextAnimations { get { return _textAnimations; } set { _textAnimations = value; } }

        public TextHorizontalAlignment HorizontalAlignment { get { return _hAlign; } set { _hAlign = value; } }
        public TextVerticalAlignment VerticalAlignment { get { return _vAlign; } set { _vAlign = value; } }
        public Vector2 Size { get { return _size; } protected set { _size = value; } }
        
        #endregion // Properties


        #region Events

        public event EventHandler TextChangedEvent;

        #endregion // Events


        #region Init

        protected TextSprite(string name)
            : base(name)
        {
            _hAlign = TextHorizontalAlignment.Left;
            _vAlign = TextVerticalAlignment.Center;

            _textAnimations = new TextSpriteAnimationManager(this);
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
            : base(clone)
        {
            _hAlign = clone.HorizontalAlignment;
            _vAlign = clone.VerticalAlignment;
            _font = clone.Font;
            _text = clone.Text;
            _size = clone.Size;
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

            batch.DrawString(Font, Text, mPosition, Tint, mRotation, centerPoint, mScale,
                (SpriteEffects)FlipMode, 0);
        }

        #endregion // Draw


        #region Methods

        protected virtual void InvalidateMeasure()
        {
            if (string.IsNullOrEmpty(_text))
                _size = Vector2.Zero;
            else
                _size = _font.MeasureString(_text);
        }

        protected virtual void TextChanged()
        {
            if (TextChangedEvent != null)
                TextChangedEvent(this, EventArgs.Empty);
        }

        #endregion // Methods

    }
}
