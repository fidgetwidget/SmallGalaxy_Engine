using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SmallGalaxy_Engine.Entities
{

    // for objects that have position, rotation, scale data
    public interface ITransformable { Matrix GetTransform(); void SetTransform(Matrix transform); void SetPosition(float x, float y); void SetRotation(float rotation); void SetScale(float x, float y); }

    // for objects that have an origin
    public interface IPivotable { void SetPivot(Vector2 pivot); }

    // for objects that have a color tint
    public interface ITintable { void SetTint(Color tint); }

    public abstract class Entity : IDisposable, ICloneable, ITransformable
    {

        #region Fields

        private string _name;

        private Transform _transform = new Transform(Vector2.Zero, 0f, Vector2.One);

        private bool _isInitialized = false;
        private bool _isLoaded = false;
        private bool _isVisible = true;
        private bool _isHidden = false;

        #endregion // Fields


        #region Properties

        public string Name { get { return _name; } set { _name = value; } }

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
        public bool IsVisible { get { return _isHidden ? false : _isVisible; } set { _isVisible = value; } }
        public bool IsHidden { get { return _isHidden; } set { _isHidden = value; } } 

        #endregion // Properties


        #region Events

        public event EventHandler PositionChangedEvent;
        public event EventHandler RotationChangedEvent;

        public event EventHandler PreInitEvent;
        public event EventHandler InitCompleteEvent;
        public event EventHandler PreLoadEvent;
        public event EventHandler LoadCompleteEvent;

        #endregion // Events


        #region Init

        public Entity() { }
        public virtual object Clone()
        {
            return null;
        }

        public void Initialize()
        {
            if (_isInitialized) { return; }

            if (PreInitEvent != null)
                PreInitEvent(null, EventArgs.Empty);

            InitCore();
            _isInitialized = true;

            if (InitCompleteEvent != null)
                InitCompleteEvent(null, EventArgs.Empty);
        }
        protected virtual void InitCore() { }

        public void Load() 
        {
            if (!_isInitialized) { Initialize(); }
            if (_isLoaded) { return; }

            if (PreLoadEvent != null)
                PreLoadEvent(null, EventArgs.Empty);

            LoadCore();
            _isLoaded = true;

            if (LoadCompleteEvent != null)
                LoadCompleteEvent(null, EventArgs.Empty);
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

            DrawCore(batch);
        }
        protected virtual void DrawCore(SpriteBatch batch) { }

        #endregion // Draw


        #region Methods

        public Matrix GetTransform() { return _transform.GetTransform(); }
        public void SetTransform(Matrix transform) { _transform.SetTransform(transform); }

        protected virtual Vector2 GetWorldPosition()
        {
            return Position;
        }

        public virtual void SetPosition(float x, float y)
        {
            _transform.x = x;
            _transform.y = y;

            if (PositionChangedEvent != null)
                PositionChangedEvent(null, EventArgs.Empty);
        }

        public virtual void SetRotation(float rotation)
        {
            _transform.rotation = rotation;
            if (RotationChangedEvent != null)
                RotationChangedEvent(null, EventArgs.Empty);
        }
        public virtual void SetScale(float x, float y)
        {
            _transform.scaleX = x;
            _transform.scaleY = y;
        }

        public override string ToString()
        {
            return "{Name:" + Name + " X:" + X + " Y:" + Y + " Rotation:" + Rotation + "}";
        }        

        #endregion // Methods

    }
}
