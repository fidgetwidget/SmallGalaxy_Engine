using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SmallGalaxy_Engine.Sprites;
using SmallGalaxy_Engine.Utils;

namespace SmallGalaxy_Engine.Scenes
{

    #region Value Item

    public enum MenuEntryValueItemTypes
    {
        Text,
        Boolean,
        Index,
        Number,
        NumberOfTypes
    }

    public struct MenuEntryValueItem
    {
        public string text;
        public string description;

        public MenuEntryValueItemTypes type;

        public bool BooleanValue;
        public int IndexValue;
        public float NumberValue;

        public static MenuEntryValueItem Empty
        {
            get { return new MenuEntryValueItem(); }
        }
    }

    #endregion // Value Item


    public class MenuEntryValues : TextSprite
    {

        #region Fields

        private List<MenuEntryValueItem> _values = new List<MenuEntryValueItem>();
        private int _selectedIndex;

        private TextSprite _leftArrow;
        private TextSprite _rightArrow;        
        private float _arrowOffset = 20;
        private bool _showArrows = false;

        protected Vector2 selectedArrowScale = Vector2.One * 1.5f;
        protected Vector2 naturalArrowScale = Vector2.One;

        #endregion // Fields


        #region Properties

        public List<MenuEntryValueItem> Values { get { return _values; } set { _values = value; } }
        public string Description 
        { 
            get 
            { 
                string description = string.Empty;
                if (_values != null) { description = SelectedValueItem.description; }
                return description;
            }
        }
        public MenuEntryValueItem SelectedValueItem
        {
            get
            {
                if (_values != null && _values.Count > _selectedIndex)
                { return _values[_selectedIndex]; }
                else
                { return MenuEntryValueItem.Empty; }
            }
        }

        public int SelectedIndex { get { return _selectedIndex; } set { SetSelectedIndex(value); } }

        public TextSprite LeftArrow { get { return _leftArrow; } set { _leftArrow = value; } }
        public TextSprite RightArrow { get { return _rightArrow; } set { _rightArrow = value; } }
        public float ArrowOffset { get { return _arrowOffset; } set { SetArrowOffset(value); } }
        public bool ShowArrows
        {
            get { return _showArrows; }
            set
            {
                _showArrows = value;                
                if (LeftArrow != null) LeftArrow.IsVisible = value;
                if (RightArrow != null) RightArrow.IsVisible = value;
            }
        }

        #endregion // Properties


        #region Events

        public event EventHandler SelectedValueChangedEvent;

        #endregion // Events


        #region Init

        public MenuEntryValues(string name)
            : base(name, FontManager.DescriptionFont)
        {            
            HorizontalAlignment = TextHorizontalAlignment.Left;
            Text = SelectedValueItem.text;
        }

        protected override void LoadCore()
        {
            base.LoadCore();
            InitArrows();
        }

        protected void InitArrows()
        {
            _leftArrow = new TextSprite(string.Format("{0}_{1}", Name, "lArrow"), FontManager.MessageFont, "<");
            _leftArrow.HorizontalAlignment = TextHorizontalAlignment.Center;
            _leftArrow.Position = new Vector2(-_arrowOffset, 0);
            _leftArrow.IsVisible = ShowArrows;
            _leftArrow.Load();

            _rightArrow = new TextSprite(string.Format("{0}_{1}", Name, "rArrow"), FontManager.MessageFont, ">");
            _rightArrow.HorizontalAlignment = TextHorizontalAlignment.Center;
            _rightArrow.Position = new Vector2(_arrowOffset + Size.X, 0);
            _rightArrow.IsVisible = ShowArrows;
            _rightArrow.Load();

            Children.Add(_leftArrow);
            Children.Add(_rightArrow);
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
            
            batch.DrawString(Font, Text, mPosition, Tint, mRotation, centerPoint, mScale,
                SpriteEffects.None, 0);

        }

        #endregion // Draw


        #region Update

        public override void Update(float elapsedTime)
        {
            if (_leftArrow != null && _leftArrow.Scale != naturalArrowScale)
            {
                _leftArrow.Scale = Vector2.SmoothStep(_leftArrow.Scale, naturalArrowScale, 0.5f);
            }
            if (_rightArrow != null && _rightArrow.Scale != naturalArrowScale)
            {
                _rightArrow.Scale = Vector2.SmoothStep(_rightArrow.Scale, naturalArrowScale, 0.5f);
            }
        }

        #endregion // Update


        #region Methods

        public void AddItem(string text, string description)
        {
            MenuEntryValueItem item = new MenuEntryValueItem()
            {
                text = text,
                description = description,
                type = MenuEntryValueItemTypes.Text,
            };
            AddItem(item);
        }
        public void AddItem(MenuEntryValueItem item)
        {
            Values.Add(item);
        }

        public virtual void NextValue()
        {
            if (_selectedIndex == _values.Count - 1)
            { _selectedIndex = 0; }
            else
            { _selectedIndex++; }
            RightArrow.Scale = selectedArrowScale;
            SelectedValueChanged();
        }
        public virtual void PreviousValue()
        {
            if (_selectedIndex == 0)
            { _selectedIndex = _values.Count - 1; }
            else
            { _selectedIndex--; }
            LeftArrow.Scale = selectedArrowScale;
            SelectedValueChanged();
        }

        protected virtual void SetArrowOffset(float value)
        {
            if (_arrowOffset != value)
            {
                _arrowOffset = value;
                if (LeftArrow != null) LeftArrow.Position = new Vector2(-value, 0);
                if (RightArrow != null) RightArrow.Position = new Vector2(value + Size.X, 0);
            }
        }
                
        protected virtual void SetSelectedIndex(int value)
        {
            if (value < 0) { value = 0; }
            if (value >= _values.Count) { value = _values.Count - 1; }
            _selectedIndex = value;

            SelectedIndexChanged();
        }

        protected virtual void SelectedIndexChanged()
        {
            Text = SelectedValueItem.text;
            LeftArrow.Position = new Vector2(-ArrowOffset, 0);
            RightArrow.Position = new Vector2(ArrowOffset + Size.X, 0);            
        }

        protected virtual void SelectedValueChanged()
        {
            if (SelectedValueChangedEvent != null)
                SelectedValueChangedEvent(this, EventArgs.Empty);
        }

        #endregion // Methods

    }


    #region Boolean Extension

    /// <summary>
    /// Keeping the MenuEntryValueItem format so that each boolean can have their own unique text and description
    /// this MenuEntryBooleanValues extention allows you to select based on its boolean value
    /// </summary>
    public class MenuEntryBooleanValues : MenuEntryValues
    {

        #region Fields

        public bool SelectedBooleanValue
        {
            get
            {
                bool boolean = false;
                if (Values != null) { boolean = SelectedValueItem.BooleanValue; }
                return boolean;
            }
            set
            {
                if (Values != null)
                {
                    for (int i = 0; i < Values.Count; i++)
                    {
                        var item = Values[i];
                        if (item.type == MenuEntryValueItemTypes.Boolean &&
                            item.BooleanValue == value)
                        {
                            SelectedIndex = i;
                            return;
                        }
                    }
                }
            }
        }

        #endregion // Fields


        #region Init

        public MenuEntryBooleanValues(string name)
            : base(name) 
        {
            // because we are dealing with boolean values... the capacity should really only be two
            Values.Capacity = 2;
        }

        #endregion // Init


        #region Add Items

        public void AddItem(string text, string description, bool boolean)
        {
            MenuEntryValueItem item = new MenuEntryValueItem()
            {
                text = text,
                description = description,
                type = MenuEntryValueItemTypes.Boolean,
                BooleanValue = boolean,
            };
            AddItem(item);
        }

        #endregion // Add Items

    }

    #endregion // Boolean Extension


    #region Index Extension

    /// <summary>
    /// Keeping the MenuEntryValueItem format so that each index can have their own unique text and description
    /// this MenuEntryValues extension allows you to keep a differently ordered list of indexs (perfect for enums)
    /// that can be selected based on their original IndexValue order rather than the list order.
    /// </summary>
    public class MenuEntryIndexValues : MenuEntryValues
    {
        
        #region Properties

        public int SelectedValue_Index
        {
            get
            {
                int index = 0;
                if (Values != null) { index = SelectedValueItem.IndexValue; }
                return index;
            }
            set
            {
                for (int i = 0; i < Values.Count; i++)
                {
                    var item = Values[i];
                    if (item.type == MenuEntryValueItemTypes.Index &&
                        item.IndexValue == value)
                    {
                        SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        #endregion // Properties


        #region Init

        public MenuEntryIndexValues(string name)
            : base(name) { }

        #endregion // Init


        #region Add Items

        public void AddItem(string text, string description, int index)
        {
            MenuEntryValueItem item = new MenuEntryValueItem()
            {
                text = text,
                description = description,
                type = MenuEntryValueItemTypes.Index,
                IndexValue = index,
            };
            AddItem(item);
        }

        #endregion // Add Items

    }

    #endregion // Index Extension


    #region Number Extension

    /// <summary>
    /// Removing the MenuEntryValueItem format and replacing it with a float number with min and max constraints
    /// this MenuEntryValue extension allows you to keep a float value that can be adjusted by a defined amount
    /// </summary>
    public class MenuEntryNumberValues : MenuEntryValues
    {

        #region Fields

        private float _numberValue;
        private bool _showPercentile = false;
        private float _minValue = float.MinValue, _maxValue = float.MaxValue;
        private float _incAmount = 1, _decAmount = 1;

        #endregion // Fields


        #region Properties

        public float NumberValue
        {
            get { return _numberValue; }
            set
            {
                _numberValue = MathHelper.Clamp(value, _minValue, _maxValue);

                Text = _numberValue.ToString();
                if (_showPercentile) { Text += " %"; }

                LeftArrow.Position = new Vector2(-ArrowOffset, 0);
                RightArrow.Position = new Vector2(ArrowOffset + Size.X, 0);
            }
        }

        public bool ShowPercentile { get { return _showPercentile; } set { _showPercentile = value; } }
        public float MaxValue { get { return _maxValue; } set { _maxValue = value; } }
        public float MinValue { get { return _minValue; } set { _minValue = value; } }
        public float IncreaseAmount { get { return _incAmount; } set { _incAmount = value; } }
        public float DecreaseAmount { get { return _decAmount; } set { _decAmount = value; } }

        #endregion // Fields


        #region Init

        public MenuEntryNumberValues(string name)
            : base(name) { }

        #endregion // Init


        #region Navigate

        public override void NextValue()
        {
            if (_numberValue == _maxValue || _numberValue + _incAmount > _maxValue)
            {
                _numberValue = _minValue;
            }
            else
            {
                _numberValue += _incAmount;
            }
            RightArrow.Scale = selectedArrowScale;
            base.SelectedIndexChanged();
        }

        public override void PreviousValue()
        {
            if (_numberValue == _minValue || _numberValue - _decAmount < _minValue)
            {
                _numberValue = _maxValue;
            }
            else
            {
                _numberValue -= _decAmount;
            }
            LeftArrow.Scale = selectedArrowScale;
            base.SelectedIndexChanged();
        }

        #endregion // Navigate

    }

    #endregion // Number Extension


}
