using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmallGalaxy_Engine.Scenes
{
    /// <summary>
    /// The loading screen coordinates transitions between the menu system and the
    /// game itself. Normally one screen will transition off at the same time as
    /// the next screen is transitioning on, but for larger transitions that can
    /// take a longer time to load their data, we want the menu system to be entirely
    /// gone before we start loading the game. This is done as follows:
    /// 
    /// - Tell all the existing screens to transition off.
    /// - Activate a loading screen, which will transition on at the same time.
    /// - The loading screen watches the state of the previous screens.
    /// - When it sees they have finished transitioning off, it activates the real
    ///   next screen, which may take a long time to load its data. The loading
    ///   screen will be the only thing displayed while this load is taking place.
    /// </summary>
    /// <remarks>
    /// This class is similar to one of the same name in the GameStateManagement sample.
    /// </remarks>
    public class LoadingScreen : Scene
    {

        #region Fields

        protected GraphicsDevice graphicsDevice;

        private string _loadingText;

        protected bool loadingIsSlow;
        protected bool otherScreensAreGone;

        protected Thread backgroundThread;

        protected GameTime loadStartTime;
        protected TimeSpan loadAnimationTimer;

        protected EventHandler loadNextScreen;
        protected EventWaitHandle backgroundThreadExit;

        #endregion // Fields


        #region Properties

        public string LoadingText { get { return _loadingText; } set { _loadingText = value; } }

        #endregion // Properties


        #region Init

        /// <summary>
        /// The constructor is private: loading screens should
        /// be activated via the static Load method instead.
        /// </summary>
        protected LoadingScreen(SceneManager screenManager, bool loadingIsSlow,
            EventHandler loadNextScreen)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.loadNextScreen = loadNextScreen;

            BeginTime = TimeSpan.FromSeconds(0.5);

            if (loadingIsSlow)
            {
                backgroundThread = new Thread(BackgroundWorkerThread);
                backgroundThreadExit = new ManualResetEvent(false);

                graphicsDevice = screenManager.GraphicsDevice;
            }
        }

        /// <summary>
        /// Activates the loading screen.
        /// </summary>
        public static void Load(SceneManager screenManager, EventHandler loadNextScreen,
            bool loadingIsSlow)
        {
            // Tell all the current screens to transition off.
            foreach (Scene screen in screenManager.ActiveScenes)
                screen.ExitScreen(true);

            // Create and activate the loading screen.
            LoadingScreen loadingScreen = new LoadingScreen(screenManager, loadingIsSlow, loadNextScreen);

            screenManager.ActivateScene(loadingScreen);
        }

        #endregion // Init


        #region Update

        /// <summary>
        /// Updates the loading screen.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // If all the previous screens have finished transitioning
            // off, it is transitionTime to actually perform the load.
            if (otherScreensAreGone)
            {
                if (backgroundThread != null)
                {
                    loadStartTime = gameTime;
                    backgroundThread.Start();
                }

                Manager.DisposeScene(this);

                loadNextScreen(this, EventArgs.Empty);

                // Signal the background thread to exit, then wait for it to do so.                
                if (backgroundThread != null)
                {
                    backgroundThreadExit.Set();
                    backgroundThread.Join();
                }

                // Once the load has finished, we use ResetElapsedTime to tell
                // the  game timing mechanism that we have just finished a very
                // long frame, and that it should not try to catch up.
                Manager.Game.ResetElapsedTime();
            }
        }

        #endregion // Update


        #region Draw

        /// <summary>
        /// Draws the loading screen.
        /// </summary>
        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            // If we are the only isActive screen, that means all the previous screens
            // must have finished transitioning off. We check for this in the Draw
            // method, rather than in Update, because it isn't enough just for the
            // screens to be gone: in order for the transition to look good we must
            // have actually drawn a frame without them before we perform the load.
            if ((State == ScreenState.Active) &&
                (Manager.ActiveScenes.Count == 1))
            {
                otherScreensAreGone = true;
            }

            // The gameplay screen takes a while to load, so we display a loading
            // message while that is going on, but the menus load very quickly, and
            // it would look silly if we flashed this up for just a fraction of a
            // second while returning from the game to the menus. This parameter
            // tells us how long the loading is going to take, so we know whether
            // to bother drawing the message.
            if (loadingIsSlow)
            {
                SpriteFont font = FontManager.HeaderFont;
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                string message = "Loading";

                // Center the text in the viewport.
                Viewport viewport = Manager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = font.MeasureString(message);
                Vector2 textPosition = (viewportSize - textSize) / 2;

                Color color = new Color(255, 255, 255, TransitionAlpha);

                // Animate the number of dots after our "Loading..." message.
                loadAnimationTimer += gameTime.ElapsedGameTime;

                int dotCount = (int)(loadAnimationTimer.TotalSeconds * 5) % 4;

                message += new string('.', dotCount);

                // Draw the text.
                batch.Begin();
                batch.DrawString(FontManager.HeaderFont, message, textPosition, color);
                batch.End();
            }
        }

        #endregion // Draw


        #region Background Thread

        /// <summary>
        /// Worker thread draws the loading animation and updates the network
        /// session while the load is taking place.
        /// </summary>
        void BackgroundWorkerThread()
        {
            long lastTime = Stopwatch.GetTimestamp();

            // EventWaitHandle.WaitOne will return true if the exit signal has
            // been triggered, or false if the timeout has expired. We use the
            // timeout to update at regular intervals, then break out of the
            // loop when we are signalled to exit.
            while (!backgroundThreadExit.WaitOne(1000 / 30, false))
            {
                GameTime gameTime = GetGameTime(ref lastTime);
                SpriteBatch batch = this.Manager.SpriteBatch;
                DrawLoadAnimation(batch, gameTime);
            }
        }

        /// <summary>
        /// Works out how long it has been since the last background thread update.
        /// </summary>
        GameTime GetGameTime(ref long lastTime)
        {
            long currentTime = Stopwatch.GetTimestamp();
            long elapsedTicks = currentTime - lastTime;
            lastTime = currentTime;

            TimeSpan elapsedTime = TimeSpan.FromTicks(elapsedTicks *
                                                      TimeSpan.TicksPerSecond /
                                                      Stopwatch.Frequency);

            return new GameTime(loadStartTime.TotalGameTime + elapsedTime, elapsedTime);
        }


        /// <summary>
        /// Calls directly into our Draw method from the background worker thread,
        /// so as to update the load animation in parallel with the actual loading.
        /// </summary>
        void DrawLoadAnimation(SpriteBatch batch, GameTime gameTime)
        {
            if ((graphicsDevice == null) || graphicsDevice.IsDisposed)
                return;

            try
            {
                graphicsDevice.Clear(new Color(15, 22, 28, 200));

                // Draw the loading screen.
                Draw(batch, gameTime);

                graphicsDevice.Present();
            }
            catch
            {
                // If anything went wrong (for instance the graphics device was lost
                // or reset) we don't have any good way to recover while running on a
                // background thread. Setting the device to null will stop us from
                // rendering, so the main game can deal with the problem later on.
                graphicsDevice = null;
            }
        }

        #endregion // Background Thread
    }
}
