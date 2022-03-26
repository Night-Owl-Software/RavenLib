using Microsoft.Xna.Framework;

namespace RavenLib.Entities
{
    /// <summary>
    /// Provides an interface for all interactable objects in the game space
    /// </summary>
    public interface IEntity
    {
        Rectangle GetCollisionbox();
        bool IsSolid();
    }
}
