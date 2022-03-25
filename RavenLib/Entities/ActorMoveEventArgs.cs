using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace RavenLib.Entities
{
    public class ActorMoveEventArgs : EventArgs
    {
        public Rectangle Collisionbox { get; }
        public Rectangle Landingbox { get; }

        public ActorMoveEventArgs(Rectangle proposedCollisionbox, Rectangle proposedLandingbox)
        {
            Collisionbox = proposedCollisionbox;
            Landingbox = proposedLandingbox;
        }
    }
}
