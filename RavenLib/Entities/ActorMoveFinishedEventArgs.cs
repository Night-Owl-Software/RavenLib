using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace RavenLib.Entities
{
    public class ActorMoveFinishedEventArgs : EventArgs
    {
        public Vector2 Position { get; }

        public ActorMoveFinishedEventArgs(Vector2 position)
        {
            Position = position;
        }
    }
}
