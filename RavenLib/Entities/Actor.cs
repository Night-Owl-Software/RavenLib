using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RavenLib.Graphics;
using RavenLib.Graphics.Animation;

namespace RavenLib.Entities
{
    /// <summary>
    /// More sophisticated Entities that can move about the game space
    /// </summary>
    public class Actor : Entity, IEntity
    {
        /// <summary>
        /// Triggered any time the Actor attempts to change its position
        /// </summary>
        public event EventHandler<ActorMoveEventArgs> Moved;
        /// <summary>
        /// Triggered any time the Actor finalizes a position change
        /// </summary>
        public event EventHandler<ActorMoveFinishedEventArgs> MoveFinished;

        protected string _entityID; // Reference ID for Actor. Not always used, but available in case needed
        protected Vector2 _previousPosition;    // The last position the Actor was in
        protected Vector2 _center;  // The center of the Actor, useful for position awareness to other Actors
        protected Vector2 _velocity;    // The horizontal (X) and vertical (Y) rates of movement
        protected Texture2D _spriteSheet;   // The spritesheet used by the Actor for its Animations
        protected Rectangle _drawRectangle; // The rectangle that the current animation's current frame is drawn to
        protected Dictionary<string, Animation> _animationMap;  // The set of animations available to the actor
        protected AnimationManager _animationManager;   // Handles updating and drawing the current animation
        protected Animation _currentAnimation;  // The active animation being handled by the _animationManager
        protected float _gravityForce;  // The amount of force that the Actor is being influenced by
        protected float _gravityDirection;  // The direction of the _gravityForce -- in a platformer, this would be toward the bottom of the screen
        protected bool _isJumping;  // Determines if the Actor is actively moving upward
        protected bool _isMoving;   // Determines if the Actor is actively changing position
        protected bool _isGrounded; // Determines if the Actor has a solid object below it

        public Actor(string entityId, Vector2 position, Vector2 size, bool solid, bool visible, bool enabled, bool isGrounded,
            string spriteSheetId = "DebugTexture", float gravityForce = 0f, float gravityDirection = 0f) :
            base(position, size, solid, visible, enabled)
        {
            /* Create new Constructor for Entity */
            _entityID = entityId;
            _previousPosition = position;
            _velocity = Vector2.Zero;
            _spriteSheet = GFXManager.GetSpriteSheet(spriteSheetId);
            _animationMap = new Dictionary<string, Animation>(); // <-- This should be loaded via JSON
            _currentAnimation = null;
            _animationManager = new AnimationManager(_currentAnimation, 1.0f);
            _gravityDirection = gravityDirection;
            _gravityForce = gravityForce;
            _isJumping = false;
            _isGrounded = isGrounded;
            _isMoving = false;

            SetActorCollisionbox();
        }

        /// <summary>
        /// Calculates the Actor's CollisionBox and Center
        /// </summary>
        protected void SetActorCollisionbox()
        {
            SetCollisionbox();
            SetCenter();
        }

        /// <summary>
        /// Calculates the Actor's Center using its CollisionBox and Size
        /// </summary>
        protected void SetCenter()
        {
            _center = new Vector2(
                _collisionBox.Right - ((int)_size.X / 2),
                _collisionBox.Bottom - ((int)_size.Y / 2));
        }

        /// <summary>
        /// Calculcates the Actor's rendering rectangle by Position and Size
        /// </summary>
        protected void SetDrawRectangle()
        {
            int _width = _currentAnimation.Frame.Width;
            int _height = _currentAnimation.Frame.Height;

            _drawRectangle = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                _width,
                _height);
        }

        /// <summary>
        /// Moves the Actor out of a collision with solid entities. Otherwise, triggers a Collided event with the offending object
        /// </summary>
        /// <param name="entity">The entity that this object has collided with</param>
        protected virtual void HandleCollision(IEntity entity)
        {
            bool isSolid = entity.IsSolid();

            if (isSolid)
            {
                CollisionSolidEntity _solidEntity = entity as CollisionSolidEntity;

                // Get the Entity's Collisionbox for calculations
                Rectangle _entityCollisionBox = _solidEntity.GetCollisionbox();

                // Get Center Point for CollisionObject and This to figure
                // placement relationship between colliding objects
                Point _entityCenter = new Point(
                    (_entityCollisionBox.Left + _entityCollisionBox.Right) / 2,
                    (_entityCollisionBox.Top + _entityCollisionBox.Bottom) / 2);

                Point _myCenter = new Point(
                    (_collisionBox.Left + _collisionBox.Right) / 2,
                    (_collisionBox.Top + _collisionBox.Bottom) / 2);

                // Set our base adjustment variables
                int _adjustedX = _collisionBox.Left;
                int _adjustedY = _collisionBox.Top;

                // Get the collionbox intersecting overlap
                Rectangle _intersect = Rectangle.Intersect(_entityCollisionBox, _collisionBox);

                // Assume nothing about the collision's relationship
                bool _isCollisionLeft = false;
                bool _isCollisionRight = false;
                bool _isCollisionAbove = false;
                bool _isCollisionBelow = false;

                // Check if the Collision's Center point is to our left or right
                if (_entityCenter.X > _myCenter.X) { _isCollisionRight = true; }
                else if (_entityCenter.X < _myCenter.X) { _isCollisionLeft = true; }

                // Check if the Collision's Center point is above or below us
                if (_entityCenter.Y > _myCenter.Y) { _isCollisionBelow = true; }
                else if (_entityCenter.Y < _myCenter.Y) { _isCollisionAbove = true; }

                // Attempt to adjust Left/Right first
                if (_isCollisionLeft)
                {
                    // Adjust to the RIGHT
                    _adjustedX = _entityCollisionBox.Left + _intersect.Width;
                }

                if (_isCollisionRight)
                {
                    // Adjust to the LEFT
                    _adjustedX = _entityCollisionBox.Left - _intersect.Width;
                }

                // Check if the resulting move still results in a collision
                Rectangle _proposed = new Rectangle(_adjustedX, _adjustedY, _collisionBox.Width, _collisionBox.Height);
                int _totalMove = Math.Abs(_collisionBox.X - _adjustedX);

                // If Left/Right does NOT fix the problem, then adjust Up/Down instead
                // Also check if the left/right move to fix is too drastic (e.g. moving us off a ledge, rather than above it)
                if (_proposed.Intersects(_entityCollisionBox) || _totalMove > _intersect.Height)
                {
                    // Get the current horizontal speed before adjusting velocity
                    float _hspeed = _velocity.X;

                    if (_isCollisionAbove)
                    {
                        _adjustedY = _collisionBox.Top + _intersect.Height;
                        
                    }

                    if (_isCollisionBelow)
                    {
                        _adjustedY = _collisionBox.Top - _intersect.Height;

                        // If we are jumping / falling, land at this time
                        if (!_isGrounded)
                        {
                            _isGrounded = true;
                        }
                    }

                    // Adjust the vertical speed to stop up/down movement
                    _velocity = new Vector2(_hspeed, 0);

                    // Update the Position to the new Y, but keep the original X
                    _position = new Vector2(_position.X, _adjustedY);
                }
                else
                {
                    // Get the current vertical speed before adjusting velocity
                    float _vspeed = _velocity.Y;

                    // If we didn't need to adjust up/down, just
                    // update our X position
                    _velocity = new Vector2(0f, _vspeed);
                    _position = new Vector2(_adjustedX, _position.Y);
                }

                // Update Collision and Drawing Rectangles once all calculations are done
                SetActorCollisionbox();
                SetDrawRectangle();
            }
        }

        /// <summary>
        /// Sets all necessary variables before invoking the Moved event
        /// </summary>
        /// <param name="newPosition"></param>
        protected virtual void StageMovement(Vector2 newPosition)
        {
            _previousPosition = _position;
            _position = newPosition;
            SetActorCollisionbox();
            SetDrawRectangle();
        }

        /// <summary>
        /// Processes any Updates to the Actor
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            _position = _position + _velocity;
            SetActorCollisionbox();
            SetDrawRectangle();
            _animationManager.Update(gameTime);
        }

        /// <summary>
        /// Performs the Draw to the SpriteBatch
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            _animationManager.Draw(spriteBatch, _drawRectangle);
        }

        /// <summary>
        /// Triggers the Moved event
        /// </summary>
        protected void OnMove()
        {
            Moved?.Invoke(this, new ActorMoveEventArgs(_collisionBox));
        }

        /// <summary>
        /// Triggers the MoveFinished event
        /// </summary>
        /// <param name="newPosition"></param>
        protected void OnMoveFinished(Vector2 newPosition)
        {
            MoveFinished?.Invoke(this, new ActorMoveFinishedEventArgs(newPosition));
        }
    }
}
