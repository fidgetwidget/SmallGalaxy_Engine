using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SmallGalaxy_Engine.Sprites;
using SmallGalaxy_Engine.Utils;

namespace SmallGalaxy_Engine.Scenes
{
    /// <summary>
    /// Helper class represents a single entry in a MenuScreen. By default this
    /// just draws the entry text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    public class MenuEntry : TextSprite
    {

        #region Fields

        private string _description;
        private string _selectedText;
        private MenuEntryValues _values;

        private bool _isEnabled = true;
        private bool _isSelected = false;

        #endregion // Fields

        #region Properties

        public string Description { get { return _description; } set { _description = value; } }
        public string SelectedText { get { return _selectedText; } protected set { _selectedText = value; } }
        public MenuEntryValues Values { get { return _values; } set { _values = value; } }
        public bool HasValues { get { return (_values != null); } }

        public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = value; } }        
        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; FocusChanged(); } }

        #endregion // Properties


        #region Events

        public event EventHandler SelectedEvent;
        public event EventHandler FocusChangedEvent;

        #endregion // Events


        #region Init

        public MenuEntry(string name, SpriteFont font)
            : base(name, font) 
        { 
            _isEnabled = true; 
        }

        public MenuEntry(string name, SpriteFont font, string text)
            : this(name, font)
        {
            Text = text;
            _selectedText = "[" + text.ToUpper() + "]";
        }

        #endregion // Init


        #region Draw

        protected override void DrawCore(SpriteBatch batch, ref Matrix global)
        {
            // No Text, Nothing to Draw
            if (string.IsNullOrEmpty(Text)) { return; }

            Vector2 centerPoint = Vector2.Zero;
            if (HorizontalAlignment == TextHorizontalAlignment.Center)
            {
                centerPoint.X = Size.X / 2.0f;
            }
            else if (HorizontalAlignment == TextHorizontalAlignment.Right)
            {
                centerPoint.X = Size.X;
            }

            if (VerticalAlignment == TextVerticalAlignment.Center)
            {
                centerPoint.Y = Size.Y / 2.0f;
            }
            else if (VerticalAlignment == TextVerticalAlignment.Bottom)
            {
                centerPoint.Y = Size.Y;
            }

            Vector2 mPosition, mScale;
            float mRotation;
            MatrixHelper.DecomposeMatrix(ref global, out mPosition, out mRotation, out mScale);

            batch.DrawString(Font, (IsSelected ? _selectedText : Text), mPosition, Tint, mRotation, centerPoint, mScale,
                SpriteEffects.None, 0);
        }

        #endregion // Draw


        #region Methods

        protected override void SetText(string text)
        {
            base.SetText(text);
            _selectedText = "[" + Text.ToUpper() + "]";
        }

        protected internal virtual void OnSelectEntry()
        {
            if (SelectedEvent != null)
                SelectedEvent(this, EventArgs.Empty);
        }

        protected virtual void FocusChanged()
        {
            if (IsSelected) { InvalidateMeasure(_selectedText); }
            else { InvalidateMeasure(Text); }

            if (FocusChangedEvent != null)
                FocusChangedEvent(this, EventArgs.Empty);
        }

        protected void InvalidateMeasure(string text)
        {
            Size = Font.MeasureString(text);
        }

        #endregion // Methods

    }
}
