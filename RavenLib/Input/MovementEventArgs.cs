﻿using System;

namespace RavenLib.Input
{
    public class MovementEventArgs : EventArgs
    {
        /// <summary>
        /// Ranges from 0.0 to 1.0; Refers to the amount of pressure applied to analog sticks.
        /// For Keyboard Keys, this is always 1.0
        /// </summary>
        public float Tilt { get; }

        /// <summary>
        /// The amount of time (in seconds) that has passed since the last frame.
        /// </summary>
        public float DeltaTime { get; }

        public bool IsGrounded { get; }

        public MovementEventArgs(float tilt, float deltaTime)
        {
            Tilt = tilt;
            DeltaTime = deltaTime;
        }
    }
}
