using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;



namespace SmallGalaxy_Engine.Physics
{
    public class PhysicsTileBit
    {

        #region Constants

        private const float Default_Density = 1.75f;
        private const float Default_Restitution = 0.5f;
        private const float Default_Friction = 0.5f;
        private const Category Default_CollisionCategory = Category.Cat10;
        

        #endregion Constants

        #region Fields

        private Vector2 Falling_Velocity = Vector2.UnitY * (-15f);

        private Fixture _rect;
        private Body _body;
        private bool _falling = false;

        #endregion // Fields

        #region Properties

        public bool Enabled
        {
            get { return _body.Enabled; }
            set { _body.Enabled = value; }
        }

        #endregion // Properties

        #region Init

        public PhysicsTileBit(float x, float y, int width, int height)
            : this(x, y, width, height, Default_Density, Default_Friction, Default_Restitution) { }

        public PhysicsTileBit(float x, float y, int width, int height, float density, float friction, float restitution)
        {
            _rect = FixtureFactory.CreateRectangle(PhysicsWorld.GetInstance(), ConvertUnits.ToSimUnits(width), ConvertUnits.ToSimUnits(height), density, ConvertUnits.ToSimUnits(new Vector2(x, y)));
            _body = _rect.Body;
            _body.BodyType = BodyType.Kinematic;
            _body.LinearVelocity = Vector2.Zero;

            _rect.Friction = friction;
            _rect.Restitution = restitution;
            _rect.CollisionFilter.AddCollisionCategory(Default_CollisionCategory);
        }

        #endregion // Init

        #region Methods

        private void BeginFalling()
        {
            _body.LinearVelocity = Falling_Velocity;
        }

        public void Fall()
        {
            BeginFalling();
        }

        #endregion // Methods

    }
}