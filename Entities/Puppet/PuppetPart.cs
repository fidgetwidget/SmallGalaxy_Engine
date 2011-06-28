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
        protected KeyframesAnimation _animation;

        public PuppetPart(int id, string name, Puppet puppet)
            : this(id, name, null, puppet) { }
        public PuppetPart(int id, string name, Frame frame, Puppet puppet)
            : base(name, frame)
        {
            _id = id;
            _puppet = puppet;
            _animation = new KeyframesAnimation(puppet.AnimManager);
            _animation.Apply = (v) =>
            {
                Position = v.Position;
                Rotation = v.Rotation;
                Scale = v.Scale;
                Origin = v.Origin;
                Tint = v.Tint;
            };
        }

        public void AddKeyframe(int index)
        {
            _animation.SaveKeyframe(
                new Keyframe()
                {
                    Index = index,
                    Position = Position,
                    Rotation = Rotation,
                    Scale = Scale,
                    Origin = Origin,
                    Tint = Tint,
                });
        }

    }
}
