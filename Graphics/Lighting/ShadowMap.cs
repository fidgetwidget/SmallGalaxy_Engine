using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SmallGalaxy_Engine.Entities;
using SmallGalaxy_Engine.Primitives;

namespace SmallGalaxy_Engine.Lighting
{
    
    // The Shadow Map is used to generate a Render Target
    // with all the shadows creted by light sources and Shadow Casters
    // Helpful for generating LOS or when creating Lighting/Shadow Shader Effects
    public class ShadowMap
    {

        private GraphicsDevice _device;
        private PrimitiveBatch _pBatch;
        private RenderTarget2D _stencil;

        public RenderTarget2D RenderTarget { get { return _stencil; } }

        public void Load(GraphicsDevice device)
        {
            _device = device;
            _pBatch = new PrimitiveBatch(device);
            _stencil = new RenderTarget2D(device, device.Viewport.Width, device.Viewport.Height);
        }

        public void Begin(ref Rectangle area, Color color) 
        {
            _device.RasterizerState = RasterizerState.CullNone;
            _device.SetRenderTarget(_stencil);
            _device.Clear(color);
            _pBatch.Projection = Matrix.CreateOrthographicOffCenter(
                area.Left, area.Right, area.Bottom, area.Top, 0, 1);
            _pBatch.Begin(PrimitiveType.TriangleList); 
        }
        public void End() 
        { 
            _pBatch.End();
            _device.SetRenderTarget(null);
        }

        public void DrawLight(Vector2 position, float lightRange, Color color)
        {
            Vertices cEdges = Shape.CreateCircle(position, lightRange, 32).vertices;
            for (int i = 0; i < 32; i++)
            {
                _pBatch.AddVertex(position, color);
                _pBatch.AddVertex(cEdges[i], color);
                _pBatch.AddVertex(i == 31 ? cEdges[0] : cEdges[i + 1], color);
            }
        }

        public void DrawShadows(IShadowCaster sc, Vector2 light, float lightRange)
        {
            DrawShadows(sc, light, lightRange, 0, Color.Black);
        }

        public void DrawShadows(IShadowCaster sc, Vector2 light, float lightRange, Color color)
        {
            DrawShadows(sc, light, lightRange, 0, color);
        }

        public void DrawShadows(IShadowCaster sc, Vector2 light, float lightRange, float penetration, Color color)
        {
            if (!sc.HasEdges()) { return; }
            Vertices vertices = sc.GetEdges();

            for (int i = 0; i < vertices.Length - 1; i += 2)
            {
                Vector2 start, end;
                start = vertices[i];
                end = vertices[i + 1];
                if (penetration != 0)
                {
                    start = LineHelper.RotateAboutOrigin(
                        light + new Vector2(0, -(Vector2.Distance(light, start) + penetration)), 
                        light,
                        LineHelper.AngleBetween(light, start));
                    end = LineHelper.RotateAboutOrigin(
                        light + new Vector2(0, -(Vector2.Distance(light, end) + penetration)),
                        light,
                        LineHelper.AngleBetween(light, end));
                }

                if (DoesEdgeCastShadow(start, end, light))
                {
                    _pBatch.AddVertex(start, color);
                    _pBatch.AddVertex(ProjectPoint(start, light, lightRange), color);
                    _pBatch.AddVertex(ProjectPoint(end, light, lightRange), color);

                    _pBatch.AddVertex(start, color);
                    _pBatch.AddVertex(end, color);
                    _pBatch.AddVertex(ProjectPoint(end, light, lightRange), color);
                }
            }
        }
       
        
        private bool DoesEdgeCastShadow(Vector2 start, Vector2 end, Vector2 light)
        {
            Vector2 startToEnd = Vector2.Subtract(end, start);
            Vector2 normal = new Vector2(startToEnd.Y, -1 * startToEnd.X);

            Vector2 lightToStart = Vector2.Subtract(start, light);
            if (Vector2.Dot(normal, lightToStart) < 0)
                return true;
            else
                return false;

        }

        // returns point projected from lightToPoint beyond point
        // light<-(distance)->point<-(distance)->(result)
        private Vector2 ProjectPoint(Vector2 point, Vector2 light)
        {
            Vector2 lightToPoint = Vector2.Subtract(point, light);
            return Vector2.Add(point, lightToPoint);
        }

        // returns point projected from lightToPoint beyond point up to lightRange
        // light<-(distance)->point<-(lightRange)->(result)
        private Vector2 ProjectPoint(Vector2 point, Vector2 light, float lightRange)
        {
            Vector2 lightToPoint = Vector2.Subtract(point, light);
            float lengthNeeded = lightRange - lightToPoint.Length();
            Vector2 vectorToAdd = new Vector2()
            {
                X = lightToPoint.X,
                Y = lightToPoint.Y,
            };
            vectorToAdd.Normalize();
            vectorToAdd = Vector2.Multiply(vectorToAdd, lengthNeeded);

            Vector2 projectedPoint = Vector2.Add(point, vectorToAdd);
            return projectedPoint;
        }

    }
}
