using Microsoft.Xna.Framework;
using FarseerPhysics;
using FarseerPhysics.Dynamics;


namespace SmallGalaxy_Engine.Physics
{
    //Used to enforce a singleton pattern for World
    public static class PhysicsWorld
    {
        #region Fields

        private static World _world;
        private static Vector2 Default_Gravity = Vector2.UnitY * (-25f);

        #endregion // Fields

        #region Properties

        public static Vector2 Gravity
        {
            get
            {
                if (_world != null)
                    return _world.Gravity;
                else
                    return Vector2.Zero;
            }
            set
            {
                if (_world != null)
                    _world.Gravity = value;
            }
        }

        #endregion // Properties

        #region Methods

        public static World GetInstance()
        {
            if (_world == null)
                _world = new World(Default_Gravity);
            return _world;
        }

        #endregion // Methods
    }


}