using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RavenLib.Graphics.Animation
{
    public class AnimationManager
    {
        private Animation _animation;
        private float _frameTimer;
        private float _maxTimer;
        private bool _paused;

        public AnimationManager(Animation animation, float maxTimer, bool paused = false)
        {
            _animation = animation;
            _maxTimer = maxTimer;
            _paused = paused;
            _frameTimer = maxTimer;
        }

        public void SwapAnimation(Animation animation)
        {
            _animation = animation;
            _frameTimer = _maxTimer;
        }

        public void SwapAnimation(Animation animation, float maxTimer, bool paused = false)
        {
            _maxTimer = maxTimer;
            _paused = paused;
            SwapAnimation(animation);
        }

        public void Update(GameTime gameTime)
        {
            if (_paused || _animation == null)
            {
                return;
            }

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

        public void Draw(SpriteBatch spriteBatch, Rectangle destRect)
        {
            _animation.Draw(spriteBatch, destRect);
        }
    }
}
