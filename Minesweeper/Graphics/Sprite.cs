using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace Minesweeper.Graphics
{
    public class Sprite
    {
        public Texture2D texture { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Color tintColor { get; set; } = Color.White;

        public Sprite(Texture2D texture, int x, int y, int width, int height)
        {
            this.texture = texture;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Rectangle(x, y, width, height), tintColor);
        }
    }
}
