using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper.Graphics
{
    public class AnimatedSprite
    {
        private List<SpriteAnimationFrame> frames = [];

        public SpriteAnimationFrame this[int index] => GetFrame(index);

        public int FrameCount => frames.Count;

        public SpriteAnimationFrame CurrentFrame
        {
            get
            {
                return frames
                    .Where(f => f.TimeStamp <= PlaybackProgress)
                    .OrderBy(f => f.TimeStamp)
                    .LastOrDefault();
            }
        }

        public float Duration
        {
            get
            {
                if (!frames.Any())
                    return 0;

                return frames.Max(f => f.TimeStamp);
            }
        }

        public bool IsPlaying { get; private set; }
        public float PlaybackProgress { get; private set; }
        public bool ShouldLoop { get; set; } = true;

        public void AddFrame(Sprite sprite, float timeStamp)
        {
            frames.Add(new SpriteAnimationFrame(sprite, timeStamp));
        }

        public void Update(GameTime gameTime)
        {
            if (IsPlaying)
            {
                PlaybackProgress += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (PlaybackProgress > Duration)
                {
                    if (ShouldLoop)
                        PlaybackProgress -= Duration;
                    else
                        Stop();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            SpriteAnimationFrame frame = CurrentFrame;
            if (frame != null)
                frame.Sprite.Draw(spriteBatch, position);
        }

        public void Play()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
            PlaybackProgress = 0;
        }

        public SpriteAnimationFrame GetFrame(int index)
        {
            if (index < 0 || index >= frames.Count)
                throw new ArgumentOutOfRangeException(nameof(index),
                    "A frame with index " + index + " does not exist in this animation.");

            return frames[index];
        }

        public void Clear()
        {
            Stop();
            frames.Clear();
        }

        public static AnimatedSprite createSimpleAnimation(Texture2D texture, Point startPos, int width, int height,
            Point offset, int frameCount, float frameLength)
        {
            if (texture == null)
                throw new ArgumentNullException(nameof(texture));

            AnimatedSprite anim = new AnimatedSprite();
            for (int i = 0; i < frameCount; i++)
            {
                Sprite sprite = new Sprite(texture, startPos.X + i * offset.X, startPos.Y + i * offset.Y, width,
                    height);
                anim.AddFrame(sprite, frameLength * i);
                if (i == frameCount - 1)
                    anim.AddFrame(sprite, frameLength * (i + 1));
            }

            return anim;
        }
    }
}