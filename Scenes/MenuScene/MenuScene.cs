using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SmallGalaxy_Engine.Entities;
using SmallGalaxy_Engine.Utils;

namespace SmallGalaxy_Engine.Scenes
{
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    public abstract class MenuScene : Scene
    {

        #region Fields

        private List<MenuEntry> _menuEntries = new List<MenuEntry>();
        private int _selectedIndex = 0;
        private int _prevSelectedIndex = 0;

        #endregion // Fields


        #region Properties

        public List<MenuEntry> MenuEntries { get { return _menuEntries; } set { _menuEntries = value; } }
        public MenuEntry SelectedEntry
        {
            get
            {
                if (_selectedIndex < 0 || _selectedIndex >= _menuEntries.Count)
                    return null;
                return MenuEntries[_selectedIndex];
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex != value && value >= 0 && value <= MenuEntries.Count)
                {
                    _prevSelectedIndex = _selectedIndex;
                    _selectedIndex = value;
                    SelectedIndexChanged();
                }
            }
        }
        public int PreviousSelectedIndex { get { return _prevSelectedIndex; } }

        #endregion // Fields


        #region Events

        public event EventHandler SelectedEntryChangedEvent;

        #endregion // Events


        #region Init

        /// <summary>
        /// Constructor
        /// </summary>
        public MenuScene()
        {
            BeginTime = TimeSpan.FromSeconds(0.5);
            EndTime = TimeSpan.FromSeconds(0.5);
        }

        public override void Dispose()
        {
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntries[i].Dispose();
            }
            MenuEntries.Clear();
        }

        #endregion // Init


        #region Check Input
        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        protected override void CheckInputCore(float elapsedTime)
        {
            // Move to the previous menu entry?
            if (InputManager.IsActionTriggered(InputManager.Action.CursorUp))
            {
                PreviousValidEntry();
            }

            // Move to the next menu entry?
            if (InputManager.IsActionTriggered(InputManager.Action.CursorDown))
            {
                NextValidEntry();
            }

            // Accept or cancel the menu?
            if (InputManager.IsActionTriggered(InputManager.Action.Ok))
            {
                OnSelectEntry();
            }
            else if (InputManager.IsActionTriggered(InputManager.Action.Back))
            {
                OnCancel();
            }

        }

        #endregion // Check Input


        #region Draw

        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(SpriteBatch batch, GameTime gameTime)
        {
            batch.Begin();
            // Draw each menu entry in turn.
            foreach (MenuEntry menuEntry in MenuEntries)
            {
                menuEntry.Draw(batch);
            }
            batch.End();
        }

        protected virtual void DrawDescription()
        {
            String description = "";

            if (MenuEntries[_selectedIndex].HasValues)
            {
                description = MenuEntries[_selectedIndex].Values.Description;
            }

            if (string.IsNullOrEmpty(description))
            {
                description = MenuEntries[_selectedIndex].Description;
            }

            if (!string.IsNullOrEmpty(description))
            {

                SpriteBatch batch = Manager.SpriteBatch;
                GraphicsDevice graphics = Manager.GraphicsDevice;

                Vector2 descriptionPosition = new Vector2(
                    SafeArea.GetSafeArea(graphics, 0.8f).Left + 5f,
                    SafeArea.GetSafeArea(graphics, 0.8f).Bottom - 30f +
                        (FontManager.DescriptionFont.MeasureString(description).Y * TransitionPosition));

                Color color = Color.White;
                color.A = TransitionAlpha;

                batch.DrawString(FontManager.DescriptionFont, "( " + description + " )",
                    descriptionPosition, color);
            }
        }

        #endregion // Draw


        #region Event Methods

        protected virtual void NextValidEntry()
        {
            if (_selectedIndex == MenuEntries.Count - 1) { _selectedIndex = 0; }
            else { _selectedIndex++; }

            if (!SelectedEntry.IsEnabled || !SelectedEntry.IsVisible)
                NextValidEntry();
        }
        protected virtual void PreviousValidEntry()
        {
            if (_selectedIndex == 0) { _selectedIndex = MenuEntries.Count - 1; }
            else { _selectedIndex--; }

            if (!MenuEntries[_selectedIndex].IsEnabled || !MenuEntries[_selectedIndex].IsVisible)
                PreviousValidEntry();
        }

        protected virtual void OnSelectEntry()
        {
            MenuEntries[_selectedIndex].OnSelectEntry();
        }
        protected virtual void OnCancel()
        {
            ExitScreen();
        }

        protected virtual void SelectedIndexChanged()
        {
            MenuEntries[_selectedIndex].IsSelected = true;
            MenuEntries[_prevSelectedIndex].IsSelected = false;

            if (SelectedEntryChangedEvent != null)
                SelectedEntryChangedEvent(this, EventArgs.Empty);
        }

        #endregion // Event Methods


        #region Event Listeners

        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }

        #endregion // Event Listeners

    }
}
