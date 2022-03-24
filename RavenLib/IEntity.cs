using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace RavenLib
{
    public interface IEntity
    {
        Rectangle GetCollisionbox();
        bool IsSolid();
    }
}
