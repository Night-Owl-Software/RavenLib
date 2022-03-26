using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RavenLib.Graphics.Animation
{
    /// <summary>
    /// Contains the necessary data to run through a single set of images from a provided spritesheet texture.
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// How to handle reaching the last frame of an animation
        /// </summary>
        public enum AnimationLoopType
        {
            /// <summary>
            /// Immediately cut back to frame 0
            /// </summary>
            CutToFirst,
            /// <summary>
            /// Reverse the animation and play backwards to frame 0
            /// </summary>
            Reverse
        }

        private Texture2D _spriteTexture;       // Image to pull the animation from
        private Vector2 _framePosition;         // The current frame's position on the spritesheet
        private Vector2 _frameSize;             // The width (X) and height (Y) of the frame
        private Rectangle _frame;               // What actually gets drawn to screen
        private int _frameIndex;                // The current frame number (between 0 and _frameLast
        private int _frameLast;                 // The final frame number
        private AnimationLoopType _loopType;    // Dictates how to proceed once _frameLast is reached
        private bool _reverseLoopActive;        // Used by the AnimationLoopType.Reverse method to play the animation backward

        public Rectangle Frame
        {
            get { return _frame; }
        }

        public Animation(Texture2D spritesheet, Vector2 framePos, Vector2 frameSize, int frameCount, AnimationLoopType loopType = AnimationLoopType.CutToFirst)
        {
            _spriteTexture = spritesheet;
            _framePosition = framePos;
            _frameSize = frameSize;
            UpdateFrameRect();
            _frameIndex = 0;
            _frameLast = frameCount - 1;
            _loopType = loopType;
            _reverseLoopActive = false;
        }

        /// <summary>
        /// Calculates the position of the source rectangle to cut out of the _spriteTexture
        /// </summary>
        private void UpdateFrameRect()
        {
            int _frameX = (int)_framePosition.X + (int)_frameSize.X * _frameIndex;
            _frame = new Rectangle(_frameX, (int)_framePosition.Y, (int)_frameSize.X, (int)_frameSize.Y);
        }

        /// <summary>
        /// Moves to the next frame or begins a loop if on the last frame
        /// </summary>
        public void Next()
        {
            switch (_loopType)
            {
                case AnimationLoopType.CutToFirst:
                    {
                        // If On Last Frame, Cut to First Frame, Otherwise, go to next frame
                        if (_frameIndex == _frameLast)
                        {
                            _frameIndex = 0;
                        }
                        else
                        {
                            _frameIndex++;
                        }
                        break;
                    }
                case AnimationLoopType.Reverse:
                    {
                        if (_reverseLoopActive)
                        {
                            // Play Animation Reverse Until Reaching the Beginning
                            if (_frameIndex == 0)
                            {
                                _reverseLoopActive = false;
                                _frameIndex++;
                            }
                            else
                            {
                                _frameIndex--;
                            }
                        }
                        else
                        {
                            // Play Animation Forward Until Reaching the End
                            if (_frameIndex == _frameLast)
                            {
                                _reverseLoopActive = true;
                                _frameIndex--;
                            }
                            else
                            {
                                _frameIndex++;
                            }
                        }
                        break;
                    }
            }

            UpdateFrameRect();
        }

        /// <summary>
        /// Draws the _frame cut from the _spriteTexture onto the provided SpriteBatch
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw the frame to</param>
        /// <param name="destRect">The rectangle to draw the frame into</param>
        public void Draw(SpriteBatch spriteBatch, Rectangle destRect)
        {
            spriteBatch.Draw(_spriteTexture, destRect, _frame, Color.White);
        }

        /// <summary>
        /// Creates a copy of the Animation and returns it
        /// </summary>
        /// <returns>Copy of the Animation</returns>
        public Animation Clone()
        {
            int _frameCount = _frameLast + 1;
            return new Animation(_spriteTexture, _framePosition, _frameSize, _frameCount, _loopType);
        }
    }
}
