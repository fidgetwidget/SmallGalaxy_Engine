using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SmallGalaxy_Engine.Colliders;
using SmallGalaxy_Engine.Entities;

namespace SmallGalaxy_Engine.Scenes
{
    public abstract class EditorPallet : Entity
    {

        #region Fields

        protected EditorScene _editor;
        protected AABB _localBounds;

        #endregion // Fields


        #region Properties

        public AABB Bounds { get { return _localBounds + Position; } }
        public bool HasFocus { get { return (Bounds.Intersects(_editor.CursorPosition)); } }

        #endregion // Properties


        #region Init

        protected EditorPallet(EditorScene scene) { _editor = scene; }

        public EditorPallet(EditorScene scene, float width, float height) : this(scene)
        {            
            _localBounds = new AABB(0, 0, width, height);
        }

        #endregion // Init

    }
}
