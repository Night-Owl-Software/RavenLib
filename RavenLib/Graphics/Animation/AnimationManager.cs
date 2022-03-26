using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RavenLib.Graphics.Animation
{
    /// <summary>
    /// Handles updating and drawing the provided animation
    /// </summary>
    public class AnimationManager
    {
        private Animation _animation;   // Animation object to work with
        private float _frameTimer;      // Current timer -- counts down to 0
        private float _maxTimer;        // _frameTimer is reset to this after hitting 0
        private bool _paused;           // Stops counting down _frameTimer if TRUE

        public AnimationManager(Animation animation, float maxTimer, bool paused = false)
        {
            _animation = animation;
            _maxTimer = maxTimer;
            _paused = paused;
            _frameTimer = maxTimer;
        }

        /// <summary>
        /// Change the Animation but retain the previous frame timer and paused values
        /// </summary>
        /// <param name="animation">The Animation to begin playing</param>
        public void SwapAnimation(Animation animation)
        {
            _animation = animation;
            _frameTimer = _maxTimer;
        }

        /// <summary>
        /// Change the Animation and change other animation timer values
        /// </summary>
        /// <param name="animation">Animation to begin playing</param>
        /// <param name="maxTimer">Time between frame changes</param>
        /// <param name="paused">Indicates if the animation starts out paused</param>
        public void SwapAnimation(Animation animation, float maxTimer, bool paused = false)
        {
            _maxTimer = maxTimer;
            _paused = paused;
            SwapAnimation(animation);
        }

        /// <summary>
        /// Processes frame timer and updates the animation as needed
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Stop processing immediately if we don't have an animation or are paused
            if (_paused || _animation == null)
            {
                return;
            }

            // Otherwise, proceed to check and count down timer
            if (_frameTimer > 0)
            {
                _frameTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            }
            else
            {
                _animation.Next();
                _frameTimer = _maxTimer;
            }
        }

        /// <summary>
        /// Draws the animation's current frame onto the provided SpriteBatch
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to render to</param>
        /// <param name="destRect">Rectangle to render to</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle destRect)
        {
            _animation.Draw(spriteBatch, destRect);
        }
    }
}
