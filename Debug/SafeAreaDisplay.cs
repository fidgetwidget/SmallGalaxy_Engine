using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SmallGalaxy_Engine.Utils;

namespace SmallGalaxy_Engine.Debug
{
    public class SafeAreaDisplay : DrawableGameComponent
    {

        #region Fields

        private DebugManager debugManager;

        #endregion // Fields


        #region Init

        public SafeAreaDisplay(Game game)
            : base(game) { }

        public override void Initialize()
        {
            // Get debug manager from game service.
            debugManager =
                Game.Services.GetService(typeof(DebugManager)) as DebugManager;

            if (debugManager == null)
                throw new InvalidOperationException("DebugManaer is not registered.");

            // Register 'fps' command if debug command is registered as a service.
            IDebugCommandHost host =
                                Game.Services.GetService(typeof(IDebugCommandHost))
                                                                as IDebugCommandHost;

            if (host != null)
            {
                host.RegisterCommand("safeArea", "Safe Area Display", this.CommandExecute);
                Visible = false;
            }

            base.Initialize();
        }

        #endregion // Init


        #region Methods

        private void CommandExecute(IDebugCommandHost host,
                                    string command, IList<string> arguments)
        {
            if (arguments.Count == 0)
                Visible = !Visible;

            foreach (string arg in arguments)
            {
                switch (arg.ToLower())
                {
                    case "on":
                        Visible = true;
                        break;
                    case "off":
                        Visible = false;
                        break;
                }
            }
        }

        #endregion // Methods


        #region Draw

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = debugManager.SpriteBatch;
            GraphicsDevice device = debugManager.GraphicsDevice;

            Rectangle safeArea = SafeArea.GetSafeArea(device);

            // Draw
            spriteBatch.Begin();
            spriteBatch.Draw(debugManager.WhiteTexture, safeArea, new Color(0, 60, 0, 128));
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion  // Draw

    }
}
