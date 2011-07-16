using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SmallGalaxy_Engine.Entities;
using SmallGalaxy_Engine.Utils;

namespace SmallGalaxy_Engine.Scenes
{
    public abstract class EditorScene : Scene
    {

        #region Fields

        protected EditorScene _parent;
        protected Vector2 _cursorPosition;

        #endregion // Fields

        #region Properties

        public Vector2 CursorPosition { get { return _cursorPosition; } }

        #endregion // Properties


        #region Init

        public EditorScene() : base() { IsPopup = true; }
        public EditorScene(EditorScene parent) : this() { _parent = parent; }

        #endregion // Init

    }
}
