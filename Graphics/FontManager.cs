using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace SmallGalaxy_Engine
{
    public static class FontManager
    {

        #region Fields

        private static Dictionary<string, SpriteFont> _fonts;

        #endregion // Fields


        #region Properties

        public static Dictionary<string, SpriteFont> Fonts { get { return _fonts; } }
        // Default Fonts
        public static SpriteFont HeaderFont { get { return _fonts["Header"]; } }
        public static SpriteFont MessageFont { get { return _fonts["Message"]; } }
        public static SpriteFont DescriptionFont { get { return _fonts["Description"]; } }
        public static SpriteFont DebugFont { get { return _fonts["Debug"]; } }

        #endregion // Properties


        #region Init

        public static void Initialize()
        {
            _fonts = new Dictionary<string, SpriteFont>();
        }

        /// <summary>
        /// Load the fonts from the Content pipeline.
        /// </summary>
        public static void Load(ContentManager content)
        {
            // check the parameters
            if (content == null)
            {
                throw new ArgumentNullException("contentManager");
            }

            #region Default Fonts
            // Multi Line Text Fonts
            var headerFont = content.Load<SpriteFont>(@"Fonts/HeaderFont");
            headerFont.LineSpacing += 10;
            Save(headerFont, "Header");

            var messageFont = content.Load<SpriteFont>(@"Fonts/MessageFont");
            messageFont.LineSpacing += 8;
            Save(messageFont, "Message");

            var descriptionFont = content.Load<SpriteFont>(@"Fonts/DescriptionFont");
            descriptionFont.LineSpacing += 6;
            Save(descriptionFont, "Description");

            // Other Fonts
            var debugFont = content.Load<SpriteFont>(@"Fonts/DebugFont");
            Save(debugFont, "Debug");
            #endregion // Default Fonts

        }


        /// <summary>
        /// Release all references to the fonts.
        /// </summary>
        public static void Unload()
        {
            foreach (string font in _fonts.Keys)
            {
                _fonts[font] = null;
            }
            _fonts.Clear();
        }

        #endregion // Init


        #region Methods

        public static SpriteFont Get(string name)
        {
            if (_fonts.ContainsKey(name) && !string.IsNullOrEmpty(name))
                return _fonts[name];
            else
                return null;
        }

        public static void Save(SpriteFont font, string name)
        {
            if (_fonts.ContainsKey(name) && !string.IsNullOrEmpty(name))
            {
                if (_fonts[name] != font)
                    throw new ArgumentException(string.Format("Font name {0} already taken", name));
            }
            else
            {
                _fonts.Add(name, font);
            }
        }

        #endregion // Methods


        #region Text Helper Methods

        /// <summary>
        /// Adds newline characters to a string so that it fits within a certain size.
        /// </summary>
        /// <param name="text">The text to be modified.</param>
        /// <param name="maximumCharactersPerLine">
        /// The maximum length of a single line of text.
        /// </param>
        /// <param name="maximumLines">The maximum number of lines to draw.</param>
        /// <returns>The new string, with newline characters if needed.</returns>
        public static string BreakTextIntoLines(string text,
            int maximumCharactersPerLine, int maximumLines)
        {
            if (maximumLines <= 0)
            {
                throw new ArgumentOutOfRangeException("maximumLines");
            }
            if (maximumCharactersPerLine <= 0)
            {
                throw new ArgumentOutOfRangeException("maximumCharactersPerLine");
            }

            // if the string is trivial, then this is really easy
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            // if the text is short enough to fit on one line, then this is still easy
            if (text.Length < maximumCharactersPerLine)
            {
                return text;
            }

            // construct a new string with carriage returns
            StringBuilder stringBuilder = new StringBuilder(text);
            int currentLine = 0;
            int newLineIndex = 0;
            while (((text.Length - newLineIndex) > maximumCharactersPerLine) &&
                (currentLine < maximumLines))
            {
                text.IndexOf(' ', 0);
                int nextIndex = newLineIndex;
                while ((nextIndex >= 0) && (nextIndex < maximumCharactersPerLine))
                {
                    newLineIndex = nextIndex;
                    nextIndex = text.IndexOf(' ', newLineIndex + 1);
                }
                stringBuilder.Replace(' ', '\n', newLineIndex, 1);
                currentLine++;
            }

            return stringBuilder.ToString();
        }


        /// <summary>
        /// Adds new-line characters to a string to make it fit.
        /// </summary>
        /// <param name="text">The text to be drawn.</param>
        /// <param name="maximumCharactersPerLine">
        /// The maximum length of a single line of text.
        /// </param>
        public static string BreakTextIntoLines(string text,
            int maximumCharactersPerLine)
        {
            // check the parameters
            if (maximumCharactersPerLine <= 0)
            {
                throw new ArgumentOutOfRangeException("maximumCharactersPerLine");
            }

            // if the string is trivial, then this is really easy
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            // if the text is short enough to fit on one line, then this is still easy
            if (text.Length < maximumCharactersPerLine)
            {
                return text;
            }

            // construct a new string with carriage returns
            StringBuilder stringBuilder = new StringBuilder(text);
            int currentLine = 0;
            int newLineIndex = 0;
            while (((text.Length - newLineIndex) > maximumCharactersPerLine))
            {
                text.IndexOf(' ', 0);
                int nextIndex = newLineIndex;
                while ((nextIndex >= 0) && (nextIndex < maximumCharactersPerLine))
                {
                    newLineIndex = nextIndex;
                    nextIndex = text.IndexOf(' ', newLineIndex + 1);
                }
                stringBuilder.Replace(' ', '\n', newLineIndex, 1);
                currentLine++;
            }

            return stringBuilder.ToString();
        }

        public static string ListIntoString(List<string> list)
        {
            if (list.Count <= 0)
            {
                throw new ArgumentException("list");
            }

            string text = "";

            for (int i = 0; i < list.Count; i++)
            {
                text += list[i] + "\n";
            }

            return text;
        }


        /// <summary>
        /// Break text up into separate lines to make it fit.
        /// </summary>
        /// <param name="text">The text to be broken up.</param>
        /// <param name="font">The font used ot measure the Width of the text.</param>
        /// <param name="rowWidth">The maximum Width of each line, in pixels.</param>
        public static List<string> BreakTextIntoList(string text, SpriteFont font,
            int rowWidth)
        {
            // check parameters
            if (font == null)
            {
                throw new ArgumentNullException("font");
            }
            if (rowWidth <= 0)
            {
                throw new ArgumentOutOfRangeException("rowWidth");
            }

            // create the list
            List<string> lines = new List<string>();

            // check for trivial text
            if (String.IsNullOrEmpty("text"))
            {
                lines.Add(String.Empty);
                return lines;
            }

            // check for text that fits on a single line
            if (font.MeasureString(text).X <= rowWidth)
            {
                lines.Add(text);
                return lines;
            }

            // break the text up into words
            string[] words = text.Split(' ');

            // add words until they go over the length
            int currentWord = 0;
            while (currentWord < words.Length)
            {
                int wordsThisLine = 0;
                string line = String.Empty;
                while (currentWord < words.Length)
                {
                    string testLine = line;
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
                    if ((wordsThisLine > 0) &&
                        (font.MeasureString(testLine).X > rowWidth))
                    {
                        break;
                    }
                    line = testLine;
                    wordsThisLine++;
                    currentWord++;
                }
                lines.Add(line);
            }
            return lines;
        }

        #endregion // Text Helper Methods

    }
}
