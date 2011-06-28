using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace SmallGalaxy_Engine.Scenes
{

    #region Screen State Enumeration

    /// <summary>
    /// Enum describes the screen transition state.
    /// </summary>
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
        InBackground,
        Paused,
    }

    #endregion // Screen State Enumeration

    /// <summary>
    /// The Abstract Game Scene Class: 
    /// handles the core functions of a Game Scene
    /// Updates, Initialization, Transition, and Input Handeling
    /// </summary>
    public abstract class Scene : IDisposable
    {

        #region Fields

        private string _sceneName;

        private SceneManager _sceneManager;
        private TextureManager _textureManager;

        private TimeSpan _transitionOnTime = TimeSpan.Zero;
        private TimeSpan _transitionOffTime = TimeSpan.Zero;

        private float _transitionPosition = 1; // 0 -> fully active 1 -> fully off/hidden

        private ScreenState _currentState = ScreenState.TransitionOn;
        private ScreenState _previousState = ScreenState.TransitionOff;

        private bool _isInitialized = false;
        private bool _isLoaded = false;
        private bool _isExiting = false;
        private bool _isPopup = false;

        protected bool disposeOnExit = false;
        protected bool inputDisabled = false;        
        protected bool otherScreenHasFocus = false;

        #endregion // Fields


        #region Properties

        public string Name { get { return _sceneName; } set { _sceneName = value; } }

        [ContentSerializerIgnore]
        public SceneManager Manager { get { return _sceneManager; } set { _sceneManager = value; } }

        public TextureManager TextureManager { get { return _textureManager; } set { _textureManager = value; } }

        /// <summary>
        /// The time it takes before the scene is fully active
        /// </summary>
        public TimeSpan BeginTime { get { return _transitionOnTime; } set { _transitionOnTime = value; } }
        /// <summary>
        /// The time it takes before the scene transitions out
        /// </summary>
        public TimeSpan EndTime { get { return _transitionOffTime; } set { _transitionOffTime = value; } }

        public float TransitionPosition { get { return _transitionPosition; } protected set { _transitionPosition = value; } }
        public byte TransitionAlpha { get { return (byte)(255 - _transitionPosition * 255); } }

        public ScreenState State 
        { 
            get { return _currentState; }
            set
            {
                _previousState = _currentState;
                _currentState = value;
            }
        }

        public bool Paused { get { return _currentState == ScreenState.Paused; } }
        public bool InTransition
        {
            get
            {
                return _currentState == ScreenState.TransitionOn ||
                    _currentState == ScreenState.TransitionOff;
            }
        }
        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus &&
                    (_currentState == ScreenState.TransitionOn ||
                    _currentState == ScreenState.Active);
            }
        }
        public bool IsInitialized { get { return _isInitialized; } }
        public bool IsLoaded { get { return _isLoaded; } }
        public bool IsExiting { get { return _isExiting; } }
        public bool IsPopup { get { return _isPopup; } set { _isPopup = value; } }

        #endregion // Properties


        #region Events

        public event EventHandler PreInitEvent;
        public event EventHandler InitCompleteEvent;
        public event EventHandler PreLoadEvent;
        public event EventHandler LoadCompleteEvent;

        public event EventHandler PrePauseEvent;
        public event EventHandler OnPauseEvent;
        public event EventHandler BeginEvent;
        public event EventHandler EndEvent;

        public event EventHandler ExitEvent;

        #endregion // Events


        #region Init

        public void Initialize()
        {
            if (PreInitEvent != null)
                PreInitEvent(null, EventArgs.Empty);

            _textureManager = TextureManager.GetInstance(_sceneName);

            OnInit();
            _isInitialized = true;

            if (InitCompleteEvent != null)
                InitCompleteEvent(null, EventArgs.Empty);
        }
        protected virtual void OnInit() { }

        public void Load(ContentManager contentManager)
        {
            if (!IsInitialized) { Initialize(); }

            if (!IsLoaded)
            {
                if (PreLoadEvent != null)
                    PreLoadEvent(null, EventArgs.Empty);

                _textureManager.ContentManager = contentManager;
                LoadTextures(contentManager);
                LoadCore();
                _isLoaded = true;

                if (LoadCompleteEvent != null)
                    LoadCompleteEvent(null, EventArgs.Empty);
            }

            Begin();
        }
        protected virtual void LoadTextures(ContentManager contentManager) { }
        protected virtual void LoadCore() { }

        public virtual void Dispose() { }

        #endregion // Init


        #region Update

        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (Paused) { return; }
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (IsExiting)
            {
                State = ScreenState.TransitionOff;
                if (!UpdateTransition(gameTime, _transitionOffTime, 1))
                {
                    OnExitScreen();
                }
                else if (coveredByOtherScreen)
                {
                    // If the screen is covered by another, it should transition off.
                    if (UpdateTransition(gameTime, _transitionOffTime, 1))
                    {
                        // Still busy transitioning.
                        State = ScreenState.TransitionOff;
                    }
                    else
                    {
                        // Transition finished!
                        State = ScreenState.Hidden;
                    }
                }
                else if (otherScreenHasFocus)
                {
                    State = ScreenState.InBackground;
                }
                else
                {
                    // Otherwise the screen should transition on and become isActive.
                    if (UpdateTransition(gameTime, _transitionOnTime, -1))
                    {
                        // Still busy transitioning.
                        State = ScreenState.TransitionOn;
                    }
                    else
                    {
                        // Transition finished!
                        State = ScreenState.Active;
                    }
                }
            }

            // any other updating uses elapsed time
            UpdateCore((float)gameTime.ElapsedGameTime.TotalSeconds);

        }        

        private bool UpdateTransition(GameTime gameTime, TimeSpan transitionTime, int direction)
        {
            // How much should we move by?
            float transitionDelta;

            if (transitionTime == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          transitionTime.TotalMilliseconds);

            // Update the transition Position.
            _transitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if ((_transitionPosition <= 0) || (_transitionPosition >= 1))
            {
                _transitionPosition = MathHelper.Clamp(_transitionPosition, 0, 1);
                return false;
            }

            // Otherwise we are still busy transitioning.
            return true;
        }

        protected virtual void UpdateCore(float elapsedTime) { }

        #endregion // Update


        #region CheckInput

        public void CheckInput(float elapsedTime)
        {
            if (inputDisabled) { return; }
            CheckInputCore(elapsedTime);
        }

        protected virtual void CheckInputCore(float elapsedTime) { }

        #endregion // CheckInput


        #region Draw

        public virtual void Draw(SpriteBatch batch, GameTime gameTime) { }

        #endregion // Draw


        #region Methods

        public void ExitScreen() 
        {
            if (EndTime == TimeSpan.Zero)
            {
                OnExitScreen();
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                _isExiting = true;
                End();
            }
        }
        public void ExitScreen(bool disposeOnExit)
        {
            this.disposeOnExit = disposeOnExit;
            ExitScreen();
        }
        protected virtual void OnExitScreen() 
        {
            if (ExitEvent != null)
                ExitEvent(this, EventArgs.Empty);
            _isExiting = false;
            if (disposeOnExit)
            { Manager.DisposeScene(this); }
            else
            { Manager.RemoveScene(this); }
        }

        /// <summary>
        /// Toggles the Screen State between Paused and its previous Active State
        /// </summary>
        public void Pause()
        {
            if (!IsActive) { return; } // Don't pause an already inactive Screen State

            if (PrePauseEvent != null)
                PrePauseEvent(this, EventArgs.Empty);
            
            OnPause();
            State = _currentState != ScreenState.Paused ? ScreenState.Paused : _previousState;

            if (OnPauseEvent != null)
                OnPauseEvent(this, EventArgs.Empty);
        }
        protected virtual void OnPause() { }

        /// <summary>
        /// Called when the scene becomes the active scene
        /// </summary>
        protected virtual void Begin() 
        {
            if (BeginEvent != null)
                BeginEvent(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the scene is exiting
        /// </summary>
        protected virtual void End() 
        {
            if (EndEvent != null)
                EndEvent(this, EventArgs.Empty);
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion // Methods

    }
}
