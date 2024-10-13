using System;

namespace Minesweeper.Graphics
{
    public class SpriteAnimationFrame
    {
        private Sprite sprite;

        public Sprite Sprite
        {
            get => sprite;
            set => sprite = value ?? throw new ArgumentNullException(nameof(value), "The sprite cannot be null");
        }

        public float TimeStamp { get; }

        public SpriteAnimationFrame(Sprite sprite, float timeStamp)
        {
            Sprite = sprite;
            TimeStamp = timeStamp;
        }
    }
}