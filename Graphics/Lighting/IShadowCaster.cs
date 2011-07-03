using System;
using SmallGalaxy_Engine.Entities;
using SmallGalaxy_Engine.Primitives;

namespace SmallGalaxy_Engine.Lighting
{
    public interface IShadowCaster
    {
        bool HasEdges();
        Vertices GetEdges();
    }
}
