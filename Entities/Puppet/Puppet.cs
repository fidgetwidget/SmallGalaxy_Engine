using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SmallGalaxy_Engine.Animations;
using SmallGalaxy_Engine.Entities;
using SmallGalaxy_Engine.Graphics;
using SmallGalaxy_Engine.Sprites;

namespace SmallGalaxy_Engine.Puppet
{

    // The Puppet is basically a Sprite with a collection of FrameSprite Children 
    // using Keyframe animations instead of the more linear animations of the Sprite Class
    public class Puppet : Entity
    {

        #region Fields

        protected ClockManager _animManager;

        #endregion // Fields


        #region Properties

        public ClockManager AnimManager { get { return _animManager; } }

        #endregion // Properties


        #region Init

        public Puppet(string name)
            : base() 
        {
            _name = name;
        }

        #endregion // Init


        #region Update

        public override void Update(float elapsedTime)
        {
            _animManager.Update(elapsedTime);
        }

        #endregion // Update


        #region Methods

        public void AddPart(string name, Frame frame)
        {
            PuppetPart p = new PuppetPart(childCount, name, frame, this);
            AddChild(p);
        }

        #endregion // Methods


    }
}
