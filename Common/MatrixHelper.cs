using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine
{
    public static class MatrixHelper
    {
        public static void GetTransformMatrix(ref Vector2 position, ref float rotation, ref Vector2 scale, out Matrix transform)
        {
            Matrix rotM, scaleM, posM, temp;

            Matrix.CreateRotationZ(rotation, out rotM);
            Matrix.CreateScale(scale.X, scale.Y, 1, out scaleM);
            Matrix.CreateTranslation(position.X, position.Y, 0, out posM);

            Matrix.Multiply(ref scaleM, ref rotM, out temp);
            Matrix.Multiply(ref temp, ref posM, out transform);
        }
        public static Matrix GetTransformMatrix(ref Vector2 position, ref float rotation, ref Vector2 scale)
        {
            Matrix rotM, scaleM, posM, temp, transform;

            Matrix.CreateRotationZ(rotation, out rotM);
            Matrix.CreateScale(scale.X, scale.Y, 1, out scaleM);
            Matrix.CreateTranslation(position.X, position.Y, 0, out posM);

            Matrix.Multiply(ref scaleM, ref rotM, out temp);
            Matrix.Multiply(ref temp, ref posM, out transform);
            return transform;
        }


        public static void DecomposeMatrix(ref Matrix matrix,
            out Vector2 position, out float rotation, out Vector2 scale)
        {
            Vector3 position3, scale3;
            Quaternion rotationQ;
            matrix.Decompose(out scale3, out rotationQ, out position3);
            Vector2 direction = Vector2.Transform(Vector2.UnitX, rotationQ);
            rotation = (float)Math.Atan2((double)(direction.Y), (double)(direction.X));
            position = new Vector2(position3.X, position3.Y);
            scale = new Vector2(scale3.X, scale3.Y);
        }

    }
}
