using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace Minesweeper.Graphics
{
    public class Sprite(Texture2D texture, int x, int y, int width, int height)
    {
        public Texture2D texture { get; set; } = texture;
        public int x { get; set; } = x;
        public int y { get; set; } = y;
        public int width { get; set; } = width;
        public int height { get; set; } = height;
        public Color tintColor { get; set; } = Color.White;

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Rectangle(x, y, width, height), tintColor);
        }
    }
}