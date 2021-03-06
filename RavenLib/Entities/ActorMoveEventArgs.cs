using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace RavenLib.Entities
{
    public class ActorMoveEventArgs : EventArgs
    {
        public Rectangle Collisionbox { get; }

        public ActorMoveEventArgs(Rectangle proposedCollisionbox)
        {
            Collisionbox = proposedCollisionbox;
        }
    }
}
