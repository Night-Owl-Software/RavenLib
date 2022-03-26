using Microsoft.Xna.Framework;

namespace RavenLib.Entities
{
    /// <summary>
    /// A game object that is capable of interacting with other game objects
    /// </summary>
    public class Entity : IEntity
    {
        protected Vector2 _position;        // Game space position
        protected Vector2 _size;            // The width (X) and height (Y) of the Entity
        protected Rectangle _collisionBox;  // Overlapping this triggers a Collided event
        protected bool _isSolid;            // Determines if the entity repel objects outside of its collisionbox upon collision
        protected bool _isVisible;          // Determines if the entity runs a Draw method
        protected bool _isEnabled;          // Determines if the entity runs an Update method

        public Vector2 Position
        {
            get { return _position; }
            protected set { _position = value; }
        }
        public Vector2 Size
        {
            get { return _size; }
            protected set { _size = value; }
        }
        public Rectangle Collisionbox
        {
            get { return GetCollisionbox(); }
            protected set { }
        }
        public bool Solid
        {
            get { return _isSolid; }
            protected set { _isSolid = value; }
        }
        public bool Visible
        {
            get { return _isVisible; }
            protected set { _isVisible = value; }
        }
        public bool Enabled
        {
            get { return _isEnabled; }
            protected set { _isEnabled = value; }
        }


        public Entity(Vector2 position, Vector2 size, bool solid, bool visisble, bool enabled)
        {
            _position = position;
            _size = size;
            SetCollisionbox();

            _isSolid = solid;
            _isVisible = visisble;
            _isEnabled = enabled;
        }

        /// <summary>
        /// Returns the Entity's CollisionBox
        /// </summary>
        /// <returns></returns>
        public Rectangle GetCollisionbox()
        {
            return _collisionBox;
        }

        /// <summary>
        /// Recalculates the Entity's CollisionBox using Position and Size
        /// </summary>
        protected virtual void SetCollisionbox()
        {
            _collisionBox = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)_size.X,
                (int)_size.Y);
        }

        /// <summary>
        /// Returns if the Entity is Solid
        /// </summary>
        /// <returns></returns>
        public bool IsSolid()
        {
            return _isSolid;
        }
    }
}
