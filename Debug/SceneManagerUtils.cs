#region Using Statements

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SmallGalaxy_Engine;
using SmallGalaxy_Engine.Debug;
using SmallGalaxy_Engine.Scenes;
using SmallGalaxy_Engine.Utils;

#endregion

namespace SmallGalaxy_Engine.Debug
{
    public class SceneMangerUtil : GameComponent
    {

        #region Fields

        // Reference for debug manager.
        private DebugManager debugManager;
        private SceneManager sceneManager;
        
        // stringBuilder for Coord draw.
        private StringBuilder stringBuilder = new StringBuilder();

        #endregion
        
        #region Properties

        public SceneManager SceneManager { get { return sceneManager; } set { SetSceneManager(value); } }

        #endregion // Properties
        
        #region Init

        public SceneMangerUtil(Game game)
            : base(game) { }

        public override void Initialize()
        {
            // Get debug manager from game service.
            debugManager = Game.Services.GetService(typeof(DebugManager)) as DebugManager;

            if (debugManager == null)
                throw new InvalidOperationException("DebugManager is not registered.");

            // Register 'scenes' command if debug command is registered as a service.
            IDebugCommandHost host = Game.Services.GetService(typeof(IDebugCommandHost)) as IDebugCommandHost;
            if (host != null) { host.RegisterCommand("sm", "Scene Manager Services", this.CommandExecute); }
            
            base.Initialize();
        }

        #endregion // Init

        #region Methods

        /// <summary>
        /// FPS command implementation.
        /// </summary>
        private void CommandExecute(IDebugCommandHost host, string command, IList<string> arguments)
        {
            foreach (string arg in arguments) { arg.ToLower(); }

            if (arguments.Contains("list")) { ShowList(); }

            if (arguments.Contains("open"))
            {
                int index = arguments.IndexOf("open");
                string sceneToOpen = arguments[index + 1];

                if (sceneManager.ContainsScene(sceneToOpen))
                {
                    // activate the selected scene
                    sceneManager.ActivateScene(sceneToOpen);
                }
            }

            if (arguments.Contains("close"))
            {
                int index = arguments.IndexOf("close");
                string sceneToClose = arguments[index + 1];

                if (sceneManager.ContainsScene(sceneToClose))
                {
                    sceneManager.ExitScene(sceneToClose);
                }
            }
            // TODO: allow loading and disposing of scenes
        }

        private void SetSceneManager(SceneManager value)
        {
            sceneManager = value;
        }

        private void ShowList()
        {
            DebugSystem.Instance.DebugCommandUI.Echo(string.Format("{0}", sceneManager.GetSceneList()));
            DebugSystem.Instance.DebugCommandUI.Echo(string.Format("Active - {0}", sceneManager.GetActiveSceneList()));
        }

        #endregion // Methods
        
    }
}
