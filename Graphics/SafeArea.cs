using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmallGalaxy_Engine.Utils
{
    public static class SafeArea
    {

        #region Internal Values

        static Rectangle savedArea = Rectangle.Empty;
        static float defaultPercent = 0.8f;
        static readonly Dictionary<float, Rectangle> prevValues = new Dictionary<float, Rectangle>();

        #endregion // Internal Values


        /// <summary>
        /// Save a unique Safe Area
        /// </summary>
        public static void SetSafeArea(Rectangle rectangle)
        {
            savedArea = rectangle;
        }

        /// <summary>
        /// Get the Safe area (saved safe area if available, otherwise use the default safe area)
        /// </summary>
        public static Rectangle GetSafeArea(GraphicsDevice device)
        {
            if (savedArea == Rectangle.Empty) { return GetSafeArea(device, defaultPercent); }
            return savedArea;
        }

        /// <summary>
        /// Get a Safe area using a percentage of the screen
        /// </summary>
        public static Rectangle GetSafeArea(GraphicsDevice device, float percent, bool save)
        {
            Rectangle retval;

            if (prevValues.TryGetValue(percent, out retval))
                return retval;

            retval = new Rectangle(
                device.Viewport.X,
                device.Viewport.Y,
                device.Viewport.Width,
                device.Viewport.Height);

            float border = (1 - percent) / 2;

            retval.X = (int)(border * retval.Width);
            retval.Y = (int)(border * retval.Height);
            retval.Width = (int)(percent * retval.Width);
            retval.Height = (int)(percent * retval.Height);

            prevValues.Add(percent, retval);
            if (save) { savedArea = retval; }

            return retval;
        }
        public static Rectangle GetSafeArea(GraphicsDevice device, float percent)
        {
            return GetSafeArea(device, percent, false);
        }
    }
}
