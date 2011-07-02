using System;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine.Entities
{
    public struct Transform
    {
        public Vector2 position, scale;
        public float rotation;
        public float x { get { return position.X; } set { position.X = value; } }
        public float y { get { return position.Y; } set { position.Y = value; } }
        public float scaleX { get { return scale.X; } set { scale.X = value; } }
        public float scaleY { get { return scale.Y; } set { scale.Y = value; } }

        public Transform(Vector2 position, float rotation, Vector2 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
        public Transform(Matrix transform)
        {
            MatrixHelper.DecomposeMatrix(ref transform, out position, out rotation, out scale);
        }

        public Matrix GetTransform() { return MatrixHelper.GetTransformMatrix(ref position, ref rotation, ref scale); }
        public void SetTransform(Matrix transform) { MatrixHelper.DecomposeMatrix(ref transform, out position, out rotation, out scale); }

        public static Transform Lerp(Transform from, Transform to, float amount)
        {
            return new Transform(
                Vector2.Lerp(from.position, to.position, amount),
                from.rotation + (to.rotation - from.rotation) * amount,
                Vector2.Lerp(from.scale, to.scale, amount));
        }
    }
}
