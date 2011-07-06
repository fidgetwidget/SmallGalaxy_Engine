using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using SmallGalaxy_Engine;
using SmallGalaxy_Engine.Animations;
using SmallGalaxy_Engine.Colliders;
using SmallGalaxy_Engine.Debug;
using SmallGalaxy_Engine.Graphics;

namespace SmallGalaxy_Engine.Entities
{

    public abstract class Actor : Entity
    {

        #region Fields

        protected Frame _frame;
        protected FrameSet _frameset;
        protected FrameAnimationManager _animations;
        protected SpriteEffects _spriteEffect;
        protected float _previousBottom = 0f;
        protected AABB _localBounds;

        // Movement and Jumping Constants (not const for the sake of loading from a file)
        protected float MOVE_ACCELERATION, MAX_MOVE_SPEED,
            GROUND_DRAG_FACTOR, AIR_DRAG_FACTOR;
        protected float MAX_JUMP_TIME, JUMP_LAUNCH_VELOCITY,
            GRAVITY_ACCELERATION, WALL_DRAG_FACTOR, MAX_FALL_SPEED, JUMP_CONTROL;

        protected float MAX_WALL_TIME = 0.33f;

        protected Vector2 _velocity;
        protected float _movement; // input movement
        protected float _jumpTime, _wallTime;
        protected int _wallJumpDirection, _onWallDirection;
        protected bool _isOnGround, _isWallSliding,
            _isOnWall, _wasOnWall,
            _isJumping, _wasJumping, _isWallJumping;

        protected bool _canWallJump = false;
        protected bool _canWallSlide = false;

        #endregion // Fields


        #region Properties
        
        public Frame Frame { get { return _frame; } set { SetFrame(value); } }
        public FrameAnimationManager Animations { get { return _animations; } }
        public AABB Bounds { get { return Position + _localBounds; } set { SetLocalBounds(value); } }

        public bool IsOnGround { get { return _isOnGround; } }
        public bool IsWallSliding { get { return _isWallSliding; } }
        public bool IsJumping { get { return _isJumping; } }
        public float JumpTime { get { return _jumpTime; } }
        public bool IsWallJumping { get { return _isWallJumping; } }

        #endregion // Properties


        #region Init

        public Actor() : base() { _isWallJumping = false; }
        public Actor(Frame frame)
            : base()
        {
            _frame = frame;
            _animations = new FrameAnimationManager(_frame);
            _isWallJumping = false;
        }

        public virtual void LoadTextures(TextureManager textureManager) { }

        #endregion // Init


        #region Update

        public override void Update(float elapsedTime)
        {
            _animations.Update(elapsedTime);

            HandleInput(elapsedTime);
            HandleMovement(elapsedTime);
            HandlePhysics(elapsedTime);

            UpdateAnimation();

            _movement = 0;
            _isJumping = false;
        }

        protected virtual void UpdateAnimation() { }

        // For not Movement Input
        protected virtual void HandleInput(float elapsedTime) { }

        // AI or Input Driven Jumping, Running & Climbing
        protected virtual void HandleMovement(float elapsedTime) { }

        // Based on Movement, Jumping & Collisions, Effect Position
        protected virtual void HandlePhysics(float elapsedTime)
        {
            Vector2 prevPosition = Position;

            if (!_isWallJumping || ( ( (_movement > 0 && _wallJumpDirection == 1) || (_movement < 0 && _wallJumpDirection == -1) ) && _jumpTime > MAX_JUMP_TIME / 2f) )
                _velocity.X += _movement * MOVE_ACCELERATION * elapsedTime;

            _velocity.Y = MathHelper.Clamp(_velocity.Y + ((GRAVITY_ACCELERATION * elapsedTime)),
                -MAX_FALL_SPEED * (_isWallSliding ? WALL_DRAG_FACTOR : 1),
                MAX_FALL_SPEED * (_isWallSliding ? WALL_DRAG_FACTOR : 1));

            _velocity.Y = HandleJump(_velocity.Y, elapsedTime);

            if (_isOnGround)
                _velocity.X *= GROUND_DRAG_FACTOR;
            else if (_isWallJumping)
                _velocity.X *= (AIR_DRAG_FACTOR * 0.9f);
            else
            {
                _velocity.X *= AIR_DRAG_FACTOR;
            }

            _velocity.X = MathHelper.Clamp(_velocity.X, -MAX_MOVE_SPEED, MAX_MOVE_SPEED);

            Position += _velocity * elapsedTime;
            
            ResolveCollisions(); // This will adjust the Position based on the shallow collision depth

            SetPosition((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            if (_isOnWall)
            {
                _wallTime += elapsedTime;
                if (_canWallSlide && _wallTime <= MAX_WALL_TIME)
                    _isWallSliding = true;
            }
            else
            {
                _isWallSliding = false;
                _wallTime = 0f;
            }
            _wasOnWall = _isOnWall;
        }

        protected virtual float HandleJump(float yVel, float elapsedTime)
        {
            if (_isJumping)
            {
                // Begin or Continue Jump
                if ((!_wasJumping && _isOnGround) || (!_wasJumping && _isWallSliding) || _jumpTime > 0)
                {
                    if (_jumpTime == 0)
                    {
                        if (_isOnGround)
                        {
                            _isWallJumping = false;
                            BeginJump();                            
                        }
                        if (_canWallJump && _isWallSliding)
                        {
                            _isWallJumping = true;
                            _wallJumpDirection = _onWallDirection;
                            BeginWallJump();
                        }
                    }
                    _jumpTime += elapsedTime;
                }

                // If we are in the Ascent of the Jump
                if (0 < _jumpTime && _jumpTime <= MAX_JUMP_TIME)
                {
                    if (_isWallJumping)
                    {
                        // TODO: make this variable (so equipment can increase the wall jump distance: both up and away)
                        yVel = JUMP_LAUNCH_VELOCITY * 0.75f * (1.0f - (float)Math.Pow(_jumpTime / MAX_JUMP_TIME, JUMP_CONTROL));
                        _velocity.X += (_wallJumpDirection * (MOVE_ACCELERATION) * (1f - (float)Math.Pow(_jumpTime / MAX_JUMP_TIME, 0.025f)));
                    }
                    else
                        yVel = JUMP_LAUNCH_VELOCITY * (1.0f - (float)Math.Pow(_jumpTime / MAX_JUMP_TIME, JUMP_CONTROL));
                }
                else // Decending
                {
                    _isWallJumping = false;
                    _jumpTime = 0;
                }
            }
            else
            {
                // not jumping, or cancle a jump in progress
                _jumpTime = 0;
            }
            _wasJumping = _isJumping;

            return yVel;
        }

        protected virtual void ResolveCollisions() { }

        #endregion // Update


        #region Draw

        protected override void DrawCore(SpriteBatch batch, ref Matrix identity)
        {
            batch.Draw(_frame.Texture, Position, _frame.SourceRectangle, Color.White, 
                Rotation, _frame.Center, Scale, _frame.FlipMode, 0); 
        }

        #endregion // Draw


        #region Methods

        protected virtual void BeginJump() { }
        protected virtual void BeginWallJump() { }

        protected void SetFrame(Frame frame)
        {
            _frame = frame;
            _animations = new FrameAnimationManager(_frame);
        }
        public void SetLocalBounds(AABB localBounds)
        {
            _localBounds = localBounds;
        }

        public void AddFrameAnimation(string name, int fps, FrameSet set)
        {
            if (_animations == null) { throw new ArgumentNullException("frame", "frame must be set before a new animation can be added"); }
            _animations.AddFrameAnimation(name, fps, set);
        }

        public void Reset(Vector2 position)
        {
            Position = position;
            _isJumping = false;
        }

        #endregion // Methods

    }
}
