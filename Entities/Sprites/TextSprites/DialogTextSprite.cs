using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SmallGalaxy_Engine.Primitives;

namespace SmallGalaxy_Engine.Sprites
{
    public class DialogTextSprite : TextSprite
    {

        #region Fields

        protected float maxWidth = float.MaxValue;
        protected List<TextSprite> textLines = new List<TextSprite>();

        #endregion // Fields


        #region Properties
        
        public new string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value.Trim();
                InvalidateMeasure();
                TextChanged();
            }
        }

        public float MaxWidth
        {
            get { return maxWidth; }
            set
            {
                maxWidth = value;
                InvalidateMeasure();
            }
        }        

        public float Width { get; protected set; }
        public float Height { get { return Font.LineSpacing * textLines.Count; } }

        public Rectanglef TextArea
        {
            get { return new Rectanglef(Vector2.Zero, Width, Height); }
        }
        public new Vector2 Size
        {
            get { return new Vector2(MaxWidth, Height); }
        }

        #endregion // Fields


        #region Init

        public DialogTextSprite(string name, SpriteFont font)
            : this(name, font, "") { }
        public DialogTextSprite(string name, SpriteFont font, string text)
            : base(name, font, text) { }
        public DialogTextSprite(DialogTextSprite clone)
            : base(clone)
        {
            MaxWidth = clone.MaxWidth;
        }

        public override object Clone()
        {
            return new DialogTextSprite(this);
        }


        #endregion // Init


        #region Draw

        protected override void DrawCore(SpriteBatch batch, ref Matrix global)
        {
            // No Text, Nothing to Draw
            if (string.IsNullOrEmpty(Text)) { return; }

            // TODO: Handle FlipMode - horz (flip align & SpriteEffect) : vert (flip align, SpriteEffect & draw textLines in reverse order) ?

            Vector2 centerPoint = Vector2.Zero;
            if (VerticalAlignment == TextVerticalAlignment.Center)
            {
                centerPoint.Y = TextArea.Height * 0.5f;
            }
            else if (VerticalAlignment == TextVerticalAlignment.Bottom)
            {
                centerPoint.Y = TextArea.Height;
            }

            Vector2 mPosition, mScale;
            float mRotation;
            MatrixHelper.DecomposeMatrix(ref global, out mPosition, out mRotation, out mScale);

            for (int i = 0; i < textLines.Count; i++)
            {
                if (HorizontalAlignment == TextHorizontalAlignment.Center)
                {
                    centerPoint.X = Font.MeasureString(textLines[i].Text).X * 0.5f;
                }
                else if (HorizontalAlignment == TextHorizontalAlignment.Right)
                {
                    centerPoint.X = Font.MeasureString(textLines[i].Text).X;
                }
                batch.DrawString(Font, textLines[i].Text, mPosition + new Vector2(0, Font.LineSpacing * i), Tint, mRotation,
                    centerPoint, mScale, SpriteEffects.None, 0);
            }
        }

        #endregion // Draw


        #region Methods

        protected override void InvalidateMeasure()
        {
            // No Text, Nothing to Build
            if (string.IsNullOrEmpty(Text)) { return; }

            Width = 0;
            textLines.Clear();
            string[] lines = Text.Split('\n');
            string line;
            for (int i = 0; i < lines.Length; i++)
            {
                line = lines[i];

                if (float.IsNaN(MaxWidth) || Font.MeasureString(line).X * Scale.X <= MaxWidth)
                {
                    TextSprite textSprite = new TextSprite(string.Format("{0}_{1}", Name, i), Font, line);
                    textSprite.HorizontalAlignment = this.HorizontalAlignment;
                    textSprite.VerticalAlignment = this.VerticalAlignment;
                    textSprite.Load();
                    textLines.Add(textSprite);

                    Width = Math.Max(Width, Font.MeasureString(line).X);
                }
                else
                {
                    // break the text up into words
                    string[] words = line.Split(' ');

                    // add words until they go over the length
                    int currentWord = 0;
                    while (currentWord < words.Length)
                    {
                        int wordsThisLine = 0;
                        string newLine = String.Empty;
                        while (currentWord < words.Length)
                        {
                            string testLine = newLine;
                            if (testLine.Length < 1)
                            {
                                testLine += words[currentWord];
                            }
                            else if ((testLine[testLine.Length - 1] == '.') ||
                                (testLine[testLine.Length - 1] == '?') ||
                                (testLine[testLine.Length - 1] == '!'))
                            {
                                testLine += "  " + words[currentWord];
                            }
                            else
                            {
                                testLine += " " + words[currentWord];
                            }

                            if (wordsThisLine > 0 && Font.MeasureString(testLine).X > MaxWidth)
                            {
                                break;
                            }

                            newLine = testLine;
                            wordsThisLine++;
                            currentWord++;
                        }

                        TextSprite textSprite = new TextSprite(string.Format("{0}_{1}", Name, i), Font, newLine);
                        textSprite.HorizontalAlignment = this.HorizontalAlignment;
                        textSprite.VerticalAlignment = this.VerticalAlignment;
                        textSprite.Load();
                        textLines.Add(textSprite);

                        Width = Math.Max(Width, Font.MeasureString(newLine).X);
                    }
                }
            }
        }

        #endregion // Methods

    }
}
