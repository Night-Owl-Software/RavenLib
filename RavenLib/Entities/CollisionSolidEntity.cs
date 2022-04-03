using System;
using Microsoft.Xna.Framework;

namespace RavenLib.Entities
{
    /// <summary>
    /// Contains data pertinent to handling collision checking with Sloped collision objects
    /// </summary>
    public class SlopeData
    {
        /// <summary>
        /// The first collidable pixel from the top of the slope, on ceiling / inverted slopes, this would be the first pixel to touch air
        /// </summary>
        public int SlopeStart;
        /// <summary>
        /// Formatted as [X,Y] where each X pixels in the slope changes by Y height. For slopes upward, Y should be negative. If downward, positive.
        /// </summary>
        public Vector2 SlopeRate;
    }

    /// <summary>
    /// Contains data pertinent to handling collision checking with One-Way Solids
    /// </summary>
    public class OneWayData
    {
        /// <summary>
        /// From which direction can an Entity object pass through this object
        /// </summary>
        public CollisionSolidEntity.OneWayPassthru PassThruDirection;
    }


    public class CollisionSolidEntity : Entity, IEntity
    {
        public enum OneWayPassthru
        {
            PassThruNone,
            PassThruBottom,
            PassThruTop,
            PassThruRight,
            PassThruLeft,
            PassThruHorizontal,
            PassThruVertical
        }

        protected bool _isOneWay;   // Ignored collision handling if Entity object is moving through in the specified direction
        protected OneWayData _oneWayData; // Data handed off for one-way collisions
        protected bool _isSlope;        // Engages additional collision methods from Entity objects if TRUE
        protected SlopeData _slopeData; // Data handed off for sloped collisions

        // Assume CollisionSolidEntity is not a slope or one-way by default
        public CollisionSolidEntity(Vector2 position, Vector2 size, bool visible, bool enabled) :
            base(position, size, true, visible, enabled)  
        {
            _isOneWay = false;

            _isSlope = false;
        }

        // Configure the CollisionSolidEntity into a Slope
        public void SetSlopeData(int slopeHeightLeft, Vector2 slopeRate)
        {
            _isSlope = true;
            _slopeData = new SlopeData
            {
                SlopeStart = slopeHeightLeft,
                SlopeRate = slopeRate
            };
        }

        // Configure the CollisionSolidEntity into a One-Way
        public void SetOneWayData(OneWayPassthru passThruAllowed)
        {
            _isOneWay = true;
            _oneWayData = new OneWayData
            {
                PassThruDirection = passThruAllowed
            };
        }

        /// <summary>
        /// Returns if the collision object is a slope and the slope data pertient if TRUE
        /// </summary>
        /// <param name="slopeData"></param>
        /// <returns></returns>
        public bool IsSlope(out SlopeData slopeData)
        {
            if (_isSlope)
            {
                slopeData = _slopeData;
            }
            else
            {
                slopeData = null;
            }

            return _isSlope;
        }

        /// <summary>
        /// Returns if the collision object is a one-way and pertinent data if TRUE
        /// </summary>
        /// <param name="oneWayData"></param>
        /// <returns></returns>
        public bool IsOneWay(out OneWayData oneWayData)
        {
            if (_isOneWay)
            {
                oneWayData = _oneWayData;
            }
            else
            {
                oneWayData = null;
            }

            return _isOneWay;
        }
    }
}
