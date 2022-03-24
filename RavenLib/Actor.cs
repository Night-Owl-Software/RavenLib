using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RavenLib.Graphics;
using RavenLib.Graphics.Animation;

namespace RavenLib
{
    public class Actor : Entity, IEntity
    {
        protected string _entityID;
        protected Vector2 _previousPosition;
        protected Vector2 _center;
        protected Vector2 _velocity;
        protected Texture2D _spriteSheet;
        protected Rectangle _drawRectangle;
        protected Dictionary<string, Animation> _animationMap;
        protected AnimationManager _animationManager;
        protected Animation _currentAnimation;
        protected float _gravityForce;
        protected float _gravityDirection;
        protected bool _isJumping;
        protected bool _isMoving;
        protected bool _isGrounded;

        public Actor(string entityId, Vector2 position, Vector2 size, bool solid, bool visisble, bool enabled,
            string spriteSheetId = "DebugTexture", float gravityForce = 0f, float gravityDirection = 0f, bool isGrounded) :
            base(position, size, solid, visisble, enabled)
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

        protected void SetActorCollisionbox()
        {
            SetCollisionbox();
            SetCenter();
        }

        protected void SetCenter()
        {
            _center = new Vector2(
                _collisionBox.Right - ((int)_size.X / 2),
                _collisionBox.Bottom - ((int)_size.Y / 2));
        }

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

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            _animationManager.Draw(spriteBatch, _drawRectangle);
        }
    }
}
