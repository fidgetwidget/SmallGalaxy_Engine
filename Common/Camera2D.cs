using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SmallGalaxy_Engine.Animations;
using SmallGalaxy_Engine.Entities;

namespace SmallGalaxy_Engine
{
    public class Camera2D : Entity
    {

        #region Fields

        private Viewport _viewport;        
        private float _width;
        private float _height;
        private float _zoom = 1;

        private Vector2 _minPos;
        private Vector2 _maxPos;
        private float _minZoom;
        private float _maxZoom;

        private Vector2 _origin;
        private Matrix _transform;

        #endregion // Fields


        #region Properties

        public Viewport Viewport { get { return _viewport; } protected set { _viewport = value; } }

        public float Zoom { get { return _zoom; } set { SetZoom(value); } }

        public float ViewWidth { get { return _width; } set { _width = value; } }
        public float ViewHeight { get { return _height; } set { _height = value; } }

        public Rectanglef ViewArea
        {
            get
            {
                return new Rectanglef(
                    Position.X - (_width / 2f),
                    Position.Y - (_height / 2f),
                    _width, _height);
            }
        }
        public Rectanglef ZoomArea
        {
            get
            {
                return new Rectanglef(
                    Position.X - (_width / _zoom) / 2f,
                    Position.Y - (_height / _zoom) / 2f,
                    _width / _zoom,
                    _height / _zoom
                    );
            }
        }

        public Vector2 MinPosition { get { return _minPos; } set { _minPos = value; } }
        public float MinPosition_X { get { return _minPos.X; } set { _minPos.X = value; } }
        public float MinPosition_Y { get { return _minPos.Y; } set { _minPos.Y = value; } }
        public Vector2 MaxPosition { get { return _maxPos; } set { _maxPos = value; } }
        public float MaxPosition_X { get { return _maxPos.X; } set { _maxPos.X = value; } }
        public float MaxPosition_Y { get { return _maxPos.Y; } set { _maxPos.Y = value; } }

        public float MinZoom { get { return _minZoom; } set { _minZoom = value; } }
        public float MaxZomm { get { return _maxZoom; } set { _maxZoom = value; } }

        public Vector2 Origin { get { return _origin; } protected set { _origin = value; } }
        public Matrix Transform { get { return _transform; } set { _transform = value; } }

        #endregion // Fields


        #region Animations

        protected ClockManager clockManager;
        public Vector2FromToAnimation PositionAnimation;
        public FloatFromToAnimation RotationAnimation;
        public FloatFromToAnimation ZoomAnimation;

        #endregion // Animations


        #region Init

        public Camera2D(Viewport viewport)
        {
            _viewport = viewport;
            _width = viewport.Width;
            _height = viewport.Height;

            _minPos = Vector2.Zero;
            _maxPos = new Vector2(ViewWidth, ViewHeight);
            _minZoom = 0f;
            _maxZoom = 5f;

            clockManager = new ClockManager();
        }

        #endregion // Init


        #region Update

        public override void Update(float elapsedTime)
        {
            clockManager.Update(elapsedTime);
        }

        #endregion // Update


        #region Methods

        public Matrix GetTransform()
        {
            Transform =
                    Matrix.Identity *
                    Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                    Matrix.CreateScale(new Vector3(_zoom, _zoom, _zoom));
            Origin = ViewArea.Origin / _zoom;
            return Transform;
        }

        public override void SetPosition(float x, float y)
        {
            x = MathHelper.Clamp(x, _minPos.X, _maxPos.X);
            y = MathHelper.Clamp(y, _minPos.Y, _maxPos.Y);
            base.SetPosition(x, y);
        }

        protected virtual void SetZoom(float value)
        {
            _zoom = MathHelper.Clamp(value, _minZoom, _maxZoom);
        }

        public void SetPositionAnimation(Vector2 to, float duration)
        {
            SetPositionAnimation(Position, to, duration, 0f, false, 0);
        }
        public void SetPositionAnimation(Vector2 from, Vector2 to, float duration,
            float beginOffset, bool autoReverse, int repeatCount)
        {
            if (PositionAnimation == null)
            { PositionAnimation = new Vector2FromToAnimation(clockManager); }
            else
            { PositionAnimation.Stop(); }

            PositionAnimation.From = from;
            PositionAnimation.To = to;
            PositionAnimation.Duration = duration;
            PositionAnimation.BeginOffset = beginOffset;
            PositionAnimation.AutoReverse = autoReverse;
            PositionAnimation.RepeatCount = repeatCount;

            PositionAnimation.Apply = (v) =>
            {
                Position = v;
            };
        }

        public void SetRotationAnimation(float to, float duration)
        {
            SetRotationAnimation(Rotation, to, duration, 0f, false, 0);
        }
        public void SetRotationAnimation(float from, float to, float duration,
            float beginOffset, bool autoReverse, int repeatCount)
        {
            if (RotationAnimation == null)
            { RotationAnimation = new FloatFromToAnimation(clockManager); }
            else
            { RotationAnimation.Stop(); }

            RotationAnimation.From = from;
            RotationAnimation.To = to;
            RotationAnimation.Duration = duration;
            RotationAnimation.BeginOffset = beginOffset;
            RotationAnimation.AutoReverse = autoReverse;
            RotationAnimation.RepeatCount = repeatCount;

            RotationAnimation.Apply = (v) =>
            {
                Rotation = v;
            };
        }

        public void SetZoomAnimation(float to, float duration)
        {
            SetZoomAnimation(_zoom, to, duration, 0f, false, 0);
        }
        public void SetZoomAnimation(float from, float to, float duration,
            float beginOffset, bool autoReverse, int repeatCount)
        {
            if (ZoomAnimation == null)
            { ZoomAnimation = new FloatFromToAnimation(clockManager); }
            else
            { ZoomAnimation.Stop(); }

            ZoomAnimation.From = from;
            ZoomAnimation.To = to;
            ZoomAnimation.Duration = duration;
            ZoomAnimation.BeginOffset = beginOffset;
            ZoomAnimation.AutoReverse = autoReverse;
            ZoomAnimation.RepeatCount = repeatCount;

            ZoomAnimation.Apply = (v) =>
            {
                _zoom = v;
            };
        }

        #endregion // Methods

    }
}
