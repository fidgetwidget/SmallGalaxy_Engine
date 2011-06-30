using System;
using Microsoft.Xna.Framework;

namespace SmallGalaxy_Engine
{
    public interface ITileable
    {

        Point GetCoord();
        void SetCoord(int col, int row);

    }
}
