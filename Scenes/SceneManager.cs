using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

using SmallGalaxy_Engine.Primitives;
using SmallGalaxy_Engine.Utils;

namespace SmallGalaxy_Engine.Scenes
{
    public class SceneManager : DrawableGameComponent
    {

        #region Fields

        private ContentManager _contentManager;
        private SpriteBatch _spriteBatch;
        private ShapeBatch _shapeBatch;

        private Dictionary<string, Scene> _gameScenes = new Dictionary<string, Scene>();
        private List<Scene> _activeGameScenes = new List<Scene>();

        private StringBuilder sb = new StringBuilder();

        private bool _isInitialized = false;

        #endregion // Fields


        #region Properties

        public ContentManager ContentManager { get { return _contentManager; } }
        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }
        public ShapeBatch ShapeBatch { get { return _shapeBatch; } }

        public List<Scene> ActiveScenes { get { return _activeGameScenes; } }

        public bool IsInitialized { get { return _isInitialized; } }

        #endregion // Properties


        #region Init

        public SceneManager(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            base.Initialize();
            _isInitialized = true;
            Debug.DebugSystem.Instance.SceneMangerUtil.SceneManager = this;
        }

        protected override void LoadContent()
        {
            _contentManager = Game.Content;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _shapeBatch = new ShapeBatch(GraphicsDevice, _spriteBatch);
        }

        protected override void UnloadContent()
        {
            foreach (Scene screen in _gameScenes.Values)
            {
                screen.Dispose();
            }
        }

        #endregion // Init


        #region Update

        public override void Update(GameTime gameTime)
        {
            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop through the screens in reverse order
            for (int screenIndex = _activeGameScenes.Count - 1; screenIndex >= 0; screenIndex--)
            {
                if (screenIndex > _activeGameScenes.Count - 1) { return; }

                // Pop the topmost screen off the waiting list.
                Scene screen = _activeGameScenes[screenIndex];

                // Update the screen.
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.IsActive)
                {
                    // If this is the first isActive screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        screen.CheckInput(elapsedTime);
                        otherScreenHasFocus = true;
                    }

                    // If this is an isActive non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
        }

        #endregion // Update


        #region Draw

        public override void Draw(GameTime gameTime)
        {
            foreach (Scene screen in _activeGameScenes)
            {
                screen.Draw(SpriteBatch, gameTime);
            }
        }

        #endregion // Draw


        #region Methods

        public string GetSceneList()
        {
            sb.Clear();
            foreach (Scene s in _gameScenes.Values)
            {
                sb.Append("[");
                sb.Append(s.Name);
                sb.Append("]");
            }
            return sb.ToString();
        }
        public string GetActiveSceneList()
        {
            sb.Clear();
            for (int i = 0; i < ActiveScenes.Count; i++)
            {
                sb.Append("[");
                sb.Append(ActiveScenes[i].Name);
                sb.Append("]");
                if (i < ActiveScenes.Count - 1)
                    sb.Append("->");
            }
            return sb.ToString();            
        }

        public bool ContainsScene(string sceneName)
        {
            return _gameScenes.ContainsKey(sceneName);
        }

        public void LoadScene(Scene scene)
        {
            LoadScene(scene, scene.Name);
        }
        public void LoadScene(Scene scene, string sceneName)
        {
            if (_gameScenes.ContainsKey(sceneName) && !string.IsNullOrEmpty(sceneName))
            {
                if (_gameScenes[sceneName] != scene)
                    throw new ArgumentException(string.Format("GameScene '{0}' has already been loaded"));
                scene.Load(_contentManager);
            }
            else
            {
                _gameScenes.Add(scene.Name, scene);
                scene.Manager = this;
                scene.Load(_contentManager);
            }
        }

        public Scene GetScene(string screenName)
        {
            if (_gameScenes.ContainsKey(screenName))
            {
                return _gameScenes[screenName];
            }
            return null;
        }

        public void ExitScene(Scene scene) 
        {
            scene.ExitScreen(false);
        }
        public void ExitScene(string sceneName)
        {
            if (_gameScenes.ContainsKey(sceneName))
            {
                ExitScene(_gameScenes[sceneName]);
            }
        }

        public void RemoveScene(Scene scene)
        {
            _activeGameScenes.Remove(scene);
        }

        public bool DisposeScene(Scene scene)
        {
            if (_gameScenes.ContainsValue(scene))
            {
                foreach (string screenName in _gameScenes.Keys)
                {
                    if (_gameScenes[screenName] == scene)
                    {
                        _gameScenes.Remove(screenName);
                        break;
                    }
                }
            }
            
            if (_activeGameScenes.Contains(scene))
            {
                _activeGameScenes.Remove(scene);
                scene.Dispose();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DisposeScene(string sceneName)
        {
            if (_gameScenes.ContainsKey(sceneName))
            {
                if (_activeGameScenes.Contains(_gameScenes[sceneName]))
                    _activeGameScenes.Remove(_gameScenes[sceneName]);

                _gameScenes[sceneName].Dispose();

                _gameScenes.Remove(sceneName);
                return true;
            }
            return false;
        }

        public void ActivateScene(Scene scene)
        {
            LoadScene(scene);
            _activeGameScenes.Add(scene);
        }
        public bool ActivateScene(string sceneName)
        {
            if (_gameScenes.ContainsKey(sceneName))
            {
                ActivateScene(_gameScenes[sceneName]);
                return true;
            }
            return false;
        }

        #endregion // Methods

    }
}
