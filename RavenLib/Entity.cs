using Microsoft.Xna.Framework;

namespace RavenLib
{
    public class Entity : IEntity
    {
        protected Vector2 _position;
        protected Vector2 _size;
        protected Rectangle _collisionBox;
        protected bool _isSolid;
        protected bool _isVisible;
        protected bool _isEnabled;

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


        public Rectangle GetCollisionbox()
        {
            return _collisionBox;
        }

        protected void SetCollisionbox()
        {
            _collisionBox = new Rectangle(
                (int)_position.X,
                (int)_position.Y,
                (int)_size.X,
                (int)_size.Y);
        }

        public bool IsSolid()
        {
            return _isSolid;
        }
    }
}
