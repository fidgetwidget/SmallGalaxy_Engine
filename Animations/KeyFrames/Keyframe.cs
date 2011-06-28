using System;
using Microsoft.Xna.Framework;
using SmallGalaxy_Engine.Entities;

namespace SmallGalaxy_Engine.Animations
{    

    // TODO: Optimize to only apply on changed values
    public class Keyframe : IComparable<Keyframe>
    {

        #region Fields

        private int _index;

        // Tranformable
        private Transform _transform = new Transform(Vector2.Zero, 0f, Vector2.One);

        // Pivotable
        private Vector2 _origin = Vector2.Zero; // offset

        // Tintable 
        private Color _tint = Color.White;

        //private bool _isTransformable, _isPivotable, _isTintable;

        #endregion // Fields


        #region Properties

        public int Index { get { return _index; } set { _index = value; } }

        public Vector2 Position { get { return _transform.position; } set { _transform.position = value; } }
        public float Rotation { get { return _transform.rotation; } set { _transform.rotation = value; } }
        public Vector2 Scale { get { return _transform.scale; } set { _transform.scale = value; } }

        public Matrix Transform { get { return _transform.GetTransform(); } set { _transform.SetTransform(value); } }

        public Vector2 Origin { get { return _origin; } set { _origin = value; } }
        public Color Tint { get { return _tint; } set { _tint = value; } }

        #endregion // Properties


        #region Init

        public Keyframe() {}
        public Keyframe(Keyframe clone)
        {
            _index = clone._index;
            _transform = clone._transform;
            _origin = clone._origin;
            _tint = clone._tint;
        }
        public Keyframe(int index, Matrix transform, Vector2 origin, Color tint)
        {
            _index = index;
            _transform.SetTransform(transform);
            _origin = origin;
            _tint = tint;
        }


        #endregion // Init


        #region Methods

        public static Keyframe Lerp(Keyframe from, Keyframe to, float amount)
        {
            // to reduce garbage, reuse the from Keyframe -- this may cause issues, so needs to be tested
            from.Transform = Matrix.Lerp(from.Transform, to.Transform, amount); // not sure if this is faster than breaking it down into position,rotation,scale... if its not faster, then don't bother
            from.Origin = Vector2.Lerp(from.Origin, to.Origin, amount);
            from.Tint = Color.Lerp(from.Tint, to.Tint, amount);

            return from;
        }

        public static void Lerp(ref Keyframe from, ref Keyframe to, float amount, out Keyframe result)
        {
            result = new Keyframe()
            {
                Transform = Matrix.Lerp(from.Transform, to.Transform, amount), // not sure if this is faster than breaking it down into position,rotation,scale... if its not faster, then don't bother
                Origin = Vector2.Lerp(from.Origin, to.Origin, amount),
                Tint = Color.Lerp(from.Tint, to.Tint, amount)
            };
        }

        #endregion // Methods


        #region IComparable

        // Compare based on Time
        public int CompareTo(Keyframe other)
        {
            if (this._index > other._index) { return -1; }
            else if (this._index < other._index) { return 1; }
            else { return 0; }
        }

        #endregion // IComparable


    }
}
