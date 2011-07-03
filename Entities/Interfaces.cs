using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmallGalaxy_Engine.Entities
{
    // for objects that have position, rotation, scale data
    public interface ITransformable 
    { 
        Matrix GetTransform(); 
        void SetTransform(Matrix transform);
        Vector2 GetPosition();
        void SetPosition(float x, float y);
        float GetRotation();
        void SetRotation(float rotation);
        Vector2 GetScale();
        void SetScale(float x, float y); 
    }

    // for objects that have an origin
    public interface IPivotable 
    {
        Vector2 GetPivot();
        void SetPivot(float x, float y); 
    }

    // for objects that have a color tint
    public interface ITintable 
    {
        Color GetTint();
        void SetTint(Color tint); 
    }

}
