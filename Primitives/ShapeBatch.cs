#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace SmallGalaxy_Engine.Primitives
{

    public class ShapeBatch
    {
        private SpriteBatch _batch;
        protected Texture2D _pixelTexture;

        protected bool _hasBegun = false;
        

        public ShapeBatch(GraphicsDevice device, SpriteBatch batch)
        {
            if (device == null)
            {
                throw new ArgumentNullException("graphicsDevice");
            }

            _pixelTexture = new Texture2D(device, 1, 1);
            Color[] whitePixels = new Color[] { Color.White };
            _pixelTexture.SetData<Color>(whitePixels);

            _batch = batch;
        }        

        public void DrawShape(Shape shape, Color color)
        {
            DrawShape(shape, color, 1);
        }
        public void DrawShape(Shape shape, Color color, float thickness)
        {
            if (shape.Verticies.verticies == null) { return; }
            Vector2 v1, v2, position, origin, scale;
            float distance, angle;

            for (int i = shape.Verticies.Length - 1; i >= 1; --i)
            {
                v1 = shape.Verticies[i - 1];
                v2 = shape.Verticies[i];
                distance = Vector2.Distance(v1, v2);
                angle = (float)Math.Atan2((double)(v2.Y - v1.Y),
                                           (double)(v2.X - v1.X));
                //origin = Vector2.One * 0.5f;
                //position = v1 + 0.5f * (v2 - v1);
                scale = new Vector2(distance, thickness);

                _batch.Draw(_pixelTexture, v1, null, color, angle, new Vector2(0, 0.5f), scale, SpriteEffects.None, 0);
            }
        }

        // Optional Begin and End Methods
        public void Begin(ref Matrix transform)
        {
            _batch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, transform);
            _hasBegun = true;
        }
        public void End() 
        {
            _batch.End();
            _hasBegun = false;
        }


    }
}
