using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmallGalaxy_Engine.Sprites
{
    public class NumericTextSprite : TextSprite
    {

        #region Properties

        public float FloatNumber
        {
            get { return (float)number; }
            set
            {
                number = value;
                if (ShowSign)
                {
                    if (number > 0)
                        Text = "+" + String.Format(FormatString, number);
                    else
                        Text = String.Format(FormatString, number);
                }
                else
                    Text = String.Format(FormatString, number);
            }
        }
        public int IntNumber
        {
            get { return (int)number; }
            set
            {
                number = value;
                if (ShowSign)
                {
                    if (number > 0)
                        Text = "+" + String.Format(FormatString, number);
                    else
                        Text = String.Format(FormatString, number);
                }
                else
                    Text = String.Format(FormatString, number);
            }
        }
        protected double number;

        public string FormatString = "{0:g}";
        public bool ShowSign = true;

        #endregion // Fields


        #region Init

        public NumericTextSprite(string name, SpriteFont font)
            : base(name, font) { }
        public NumericTextSprite(string name, SpriteFont font, int number)
            : this(name, font)
        {
            IntNumber = number;
        }
        public NumericTextSprite(string name, SpriteFont font, float number)
            : this(name, font)
        {
            FloatNumber = number;
        }

        public NumericTextSprite(NumericTextSprite clone)
            : base(clone)
        {
            IntNumber = clone.IntNumber;
            FloatNumber = clone.FloatNumber;
        }

        #endregion // Init

    }
}
