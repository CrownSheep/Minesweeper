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
    public float Rotation;
    public bool IsAlive => CurrentLife > 0;
    public Sprite Sprite;
    private Texture2D spriteSheet;
    public Texture2D SpriteSheet
    {
        get
        {
            Texture2D texture = new Texture2D(Globals.SpriteBatch.GraphicsDevice, 1, 1);
            texture.SetData([Color.White]);
            return spriteSheet ?? Sprite?.texture ?? texture;
        }
        set => spriteSheet = value;
    }
    
    public Particle()
    {
        Reset();
    }

    public void Reset(Vector2 position = default, float lifespan = 2, Color color = default, float size = 1, float rotation = 0)
    {
        Position = position;
        Lifespan = lifespan;
        CurrentLife = lifespan;
        Color = color == default ? Color.White : color;
        Size = size;
        Rotation = rotation;
    }

    public Particle(Particle particle)
    {
        Position = particle.Position;
        Lifespan = particle.Lifespan;
        CurrentLife = particle.CurrentLife;
        Color = particle.Color;
        Size = particle.Size;
        Rotation = particle.Rotation;
        Sprite = particle.Sprite;
    }

    public virtual void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        CurrentLife -= deltaTime;
    }
}