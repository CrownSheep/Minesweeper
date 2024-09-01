using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.Graphics;

namespace Minesweeper.Particles;

public class Particle
{
    public Vector2 Position = new(0, 0);
    public float Lifespan = 2;
    public float CurrentLife = 2;
    public Color Color = Color.White;
    public float Size = 1;
    public float Rotation = 0;
    public Sprite Sprite;
    private Texture2D spriteSheet;
    public Texture2D SpriteSheet
    {
        get => spriteSheet ?? Sprite?.texture;
        set => spriteSheet = value;
    }
    public bool IsAlive => CurrentLife > 0;
    
    

    public virtual void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        CurrentLife -= deltaTime;
    }
}