using System;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine
{
    public interface ITileable<T> where T : class, ITileable<T>, new()
    {

        void SetMap(Map<T> map);

        Point GetCoord();
        void SetCoord(int col, int row);

    }
}
