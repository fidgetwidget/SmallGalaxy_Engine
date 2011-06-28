using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SmallGalaxy_Engine.Animations;
using SmallGalaxy_Engine.Entities;
using SmallGalaxy_Engine.Utils;

namespace SmallGalaxy_Engine.Sprites
{

    public enum SpriteTypes
    {
        Sprite,
        FrameSprite,
        TextSprite,
        NumberOfTypes
    }    

    public class Sprite : Entity, ITintable, IPivotable
    {


        [Flags] // Allows for Multiple to be true
        protected enum FlipFlags
        {
            None = 0,
            FlipHorizontal = 1,
            FlipVertical = 2,
        }


        #region Fields

        private Sprite _parent;
        private SpriteCollection _children;
        private SpriteAnimationManager _animations;
        
        private Vector2 _paralax = Vector2.Zero;
        private float _paralaxFactor = 0;
        private Vector2 _origin = Vector2.Zero;
        private Color _tint = Color.White;

        private FlipFlags _flipMode = FlipFlags.None;

        private bool _paralaxEnabled = true;

        #endregion // Fields


        #region Properties

        public Sprite Parent { get { return _parent; } internal set { _parent = value; } }
        public SpriteCollection Children { get { return _children; } set { _children = value; } }
        public SpriteAnimationManager Animations { get { return _animations; } set { _animations = value; } }
        
        protected FlipFlags FlipMode { get { return _flipMode; } set { SetFlipMode(value); } }

        public Vector2 Paralax { get { return _paralax; } set { SetParalax(value.X, value.Y); } }
        public float ParalaxFactor { get { return _paralaxFactor; } set { SetParalaxFactor(value); } }
        public Vector2 Origin { get { return _origin; } set { SetPivot(value); } }
        public Color Tint { get { return _tint; } set { SetTint(value); } }
        public byte Alpha { get { return _tint.A; } set { SetAlpha(value); } }
        
        public bool ParalaxEnabled { get { return _paralaxEnabled; } set { _paralaxEnabled = value; } }

        #endregion // Properties


        #region Init

        public Sprite(string name) :
            base()
        {
            Name = name;
            _children = new SpriteCollection(this);
            _animations = new SpriteAnimationManager(this);
        }

        // Does NOT clone the Child Sprites
        public Sprite(Sprite clone)
        {
            Name = clone.Name + "_clone";
            _children = new SpriteCollection(this);
            _animations = new SpriteAnimationManager(this);

            Position = clone.Position;
            Rotation = clone.Rotation;
            Origin = clone.Origin;
            Tint = clone.Tint;
            _flipMode = clone._flipMode;
        }
        public Sprite(string name, Sprite clone)
            : this(clone)
        {
            Name = name;
        }

        // Does NOT clone the Child Sprites
        public override object Clone() { return new Sprite(this); }

        protected override void InitCore()
        {
            foreach (Sprite child in _children)
            {
                child.Initialize();
            }
        }
        protected override void LoadCore()
        {
            foreach (Sprite child in _children)
            {
                child.Load();
            }
        }

        public override void Unload()
        {
            foreach (Sprite child in _children)
            {
                child.Unload();
            }

            base.Unload();
        }

        

        public override void Dispose()
        {            
            foreach (Sprite child in _children)
            {
                child.Dispose();
            }
            _children.Clear();

            base.Dispose();
        }

        #endregion // Init


        #region Update

        public override void Update(float elapsedTime)
        {
            _animations.Update(elapsedTime);
            foreach (Sprite child in _children)
            {
                child.Update(elapsedTime);
            }
        }

        public virtual void UpdateParalax(Vector2 cameraPosition)
        {
            if (!_paralaxEnabled) return; // don't do this if it doesn't care

            Paralax = Vector2.Distance(WorldPosition, cameraPosition) *
                    (WorldPosition - cameraPosition == Vector2.Zero ?
                        Vector2.Zero :
                        Vector2.Normalize(WorldPosition - cameraPosition)) *
                    ParalaxFactor;

            foreach (Sprite child in _children)
            {
                child.UpdateParalax(cameraPosition);
            }
        }

        #endregion // Update


        #region Draw

        protected override void DrawCore(SpriteBatch batch)
        {
            Matrix identity = Matrix.Identity;
            Draw(batch, ref identity);
        }
        protected void Draw(SpriteBatch batch, ref Matrix parentTransform)
        {
            if (!IsInitialized) { throw new InvalidOperationException("entity not yet initialized, cannot be drawn"); }
            if (!IsVisible) { return; }

            Matrix local, global;
            Vector2 position, scale;
            float rotation;
            position = new Vector2(
                X + (_paralaxEnabled ? _paralax.X : 0), 
                Y + (_paralaxEnabled ? _paralax.Y : 0));
            rotation = Rotation;
            scale = Scale;
            MatrixHelper.GetTransformMatrix(ref position, ref rotation, ref scale, out local);
            Matrix.Multiply(ref local, ref parentTransform, out global);

            DrawCore(batch, ref global);

            foreach (Sprite child in _children)
            {   
                child.Draw(batch, ref global);
            }
        }        
        protected virtual void DrawCore(SpriteBatch batch, ref Matrix global) { }

        #endregion // Draw


        #region Methods

        protected override Vector2 GetWorldPosition()
        {
            if (_parent != null)
                return Position + _parent.WorldPosition;
            else
                return Position;  
        }

        protected virtual void SetFlipMode(FlipFlags flipMode)
        {
            _flipMode = flipMode;
        }

        protected virtual void SetParalax(float x, float y)
        {
            _paralax.X = x;
            _paralax.Y = y;
        }
        protected virtual void SetParalaxFactor(float value)
        {
            _paralaxFactor = value;
        }

        public void SetPivot(Vector2 pivot)
        {
            SetPivot(pivot.X, pivot.Y);
        }
        public virtual void SetPivot(float x, float y)
        {
            _origin.X = x;
            _origin.Y = y;
        }
        
        public void SetTint(Color value)
        {
            SetTint(value, value.A);
        }
        protected virtual void SetTint(Color rgb, byte alpha)
        {
            _tint = rgb;
            _tint.A = alpha;
        }
        protected virtual void SetAlpha(byte value)
        {
            SetTint(_tint, value);
        }

        #endregion // Methods
        
    }
}
