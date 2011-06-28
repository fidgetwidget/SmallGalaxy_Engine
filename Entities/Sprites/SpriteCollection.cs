using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SmallGalaxy_Engine.Sprites
{
    public class SpriteCollection : Collection<Sprite>
    {

        #region Fields

        private Sprite _parent;

        #endregion // Fields


        #region Init

        public SpriteCollection(Sprite parent) { _parent = parent; }

        #endregion // Init


        #region Methods

        protected override void ClearItems()
        {
            foreach (var s in Items)
                s.Parent = null;
            base.ClearItems();
        }

        protected override void InsertItem(int index, Sprite item)
        {
            item.Parent = _parent;
            if (_parent != null && _parent.IsLoaded)
            {
                item.Load();
            }
            base.InsertItem(index, item);
        }
        protected override void RemoveItem(int index)
        {
            Items[index].Parent = null;
            base.RemoveItem(index);
        }
        protected override void SetItem(int index, Sprite item)
        {
            item.Parent = _parent;
            base.SetItem(index, item);
        }

        #endregion // Methods

    }
}
