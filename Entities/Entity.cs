using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SmallGalaxy_Engine.Entities
{
    
    public abstract class Entity : IDisposable, ICloneable, ITransformable
    {

        #region Fields

        protected string _name;
        protected Entity _parent;
        private List<Entity> _children;

        protected Transform _transform = new Transform(Vector2.Zero, 0f, Vector2.One); // not all entities need rotation or scale, but they are cheap

        private bool _isInitialized = false;
        private bool _isLoaded = false;
        private bool _isVisible = true;

        #endregion // Fields


        #region Properties

        public string Name { get { return _name; } set { _name = value; } }
        protected int childCount { get { return _children.Count; } }

        public Vector2 Position { get { return _transform.position; } set { SetPosition(value.X, value.Y); } }
        public float X { get { return _transform.x; } set { SetPosition(value, _transform.y); } }
        public float Y { get { return _transform.y; } set { SetPosition(_transform.x, value); } }

        public float Rotation { get { return _transform.rotation; } set { SetRotation(value); } }

        public Vector2 Scale { get { return _transform.scale; } set { SetScale(value.X, value.Y); } }
        public float ScaleX { get { return _transform.scaleX; } set { SetScale(value, _transform.scaleY); } }
        public float ScaleY { get { return _transform.scaleY; } set { SetScale(_transform.scaleX, value); } }

        public Matrix Transform { get { return GetTransform(); } set { SetTransform(value); } }

        public Vector2 WorldPosition { get { return GetWorldPosition(); } }

        public bool IsInitialized { get { return _isInitialized; } }
        public bool IsLoaded { get { return _isLoaded; } }
        public bool IsVisible { get { return _isVisible; } set { _isVisible = value; } }

        #endregion // Properties


        #region Init

        public Entity() { }
        public virtual object Clone()
        {
            return null;
        }

        public void Initialize()
        {
            if (_isInitialized) { return; }

            InitCore();
            _isInitialized = true;
        }
        protected virtual void InitCore() { }

        public void Load() 
        {
            if (!_isInitialized) { Initialize(); }
            if (_isLoaded) { return; }

            LoadCore();
            _isLoaded = true;
        }
        protected virtual void LoadCore() { }

        public virtual void Unload()
        {
            _isLoaded = false;
        }
        public virtual void Dispose() 
        {
            _isLoaded = false;
            _isInitialized = false;
        }

        #endregion // Init


        #region Update

        public virtual void Update(float elapsedTime) { }

        #endregion // Update


        #region Draw

        public void Draw(SpriteBatch batch)
        {
            if (!IsInitialized) { throw new InvalidOperationException("entity not yet initialized, cannot be drawn");  }
            if (!IsVisible) { return; }

            Matrix identity = Matrix.Identity;

            Draw(batch, ref identity);
        }
        public virtual void Draw(SpriteBatch batch, ref Matrix parent)
        {
            if (!IsInitialized) { throw new InvalidOperationException("entity not yet initialized, cannot be drawn"); }
            if (!IsVisible) { return; }

            Matrix local, global;
            local = _transform.GetTransform();
            Matrix.Multiply(ref local, ref parent, out global);

            DrawCore(batch, ref global);

            if (_children == null) { return; }

            foreach (Entity child in _children)
            {
                child.Draw(batch, ref global);
            }
        }
        protected virtual void DrawCore(SpriteBatch batch, ref Matrix global) { }

        #endregion // Draw


        #region Methods

        public void AddChild(Entity e)
        {
            if (_children == null) { _children = new List<Entity>(); }
            e._parent = this;
            _children.Add(e);
        }
        public void RemoveChild(Entity e)
        {
            if (_children == null) { return; }
            e._parent = null;
            _children.Remove(e);
        }        

        protected virtual Vector2 GetWorldPosition()
        {
            if (_parent != null)
                return Position + _parent.WorldPosition;
            else
                return Position;  
        }

        #region Transform
        public Matrix GetTransform() { return _transform.GetTransform(); }
        public void SetTransform(Matrix transform) { _transform.SetTransform(transform); }

        public Vector2 GetPosition() { return _transform.position; }
        public virtual void SetPosition(float x, float y)
        {
            _transform.x = x;
            _transform.y = y;
        }

        public float GetRotation() { return _transform.rotation; }
        public virtual void SetRotation(float rotation)
        {
            _transform.rotation = rotation;
        }

        public Vector2 GetScale() { return _transform.scale; }
        public virtual void SetScale(float x, float y)
        {
            _transform.scaleX = x;
            _transform.scaleY = y;
        }
        #endregion // Transform

        public override string ToString()
        {
            return "{Name:" + Name + " X:" + X + " Y:" + Y + " Rotation:" + Rotation + "}";
        }        

        #endregion // Methods

    }
}
