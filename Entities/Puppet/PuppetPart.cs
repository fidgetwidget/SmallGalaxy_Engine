using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SmallGalaxy_Engine.Animations;
using SmallGalaxy_Engine.Graphics;
using SmallGalaxy_Engine.Sprites;


namespace SmallGalaxy_Engine.Puppet
{

    public class PuppetPart : FrameSprite
    {

        protected int _id;
        protected Puppet _puppet; // the parent
        protected TransformKeyframeAnimation _tAnimation;
        protected ColorKeyframeAnimation _cAnimation;
        protected Vector2KeyframeAnimation _oAnimation;

        public PuppetPart(int id, string name, Puppet puppet)
            : this(id, name, null, puppet) { }
        public PuppetPart(int id, string name, Frame frame, Puppet puppet)
            : base(name, frame)
        {
            _id = id;
            _puppet = puppet;
            _tAnimation = new TransformKeyframeAnimation(puppet.AnimManager);
            _tAnimation.Apply = (v) =>
                {
                    SetPosition(v.x, v.y);
                    SetRotation(v.rotation);
                    SetScale(v.scaleX, v.scaleY);
                };

            _cAnimation = new ColorKeyframeAnimation(puppet.AnimManager);
            _cAnimation.Apply = (v) => { SetTint(v); };

            _oAnimation = new Vector2KeyframeAnimation(puppet.AnimManager);
            _oAnimation.Apply = (v) => { Origin = v; };
        }

        public void AddKeyframe(int index)
        {
            _tAnimation.SetKeyframe(index, this._transform);
            _cAnimation.SetKeyframe(index, this.Tint);
            _oAnimation.SetKeyframe(index, this.Origin);
        }

    }
}
