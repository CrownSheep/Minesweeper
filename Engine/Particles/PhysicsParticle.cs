using Microsoft.Xna.Framework;

namespace Minesweeper.Particles;

public class PhysicsParticle : Particle
{
    public const float DEFAULT_GRAVITY = 98f;
    public Vector2 Velocity;
    public float Gravity;
    
    private int InitialDirection => Velocity.X > 0 ? 1 : -1;

    public PhysicsParticle(Vector2 velocity = default, float gravity = DEFAULT_GRAVITY)
    {
        Velocity = velocity;
        Gravity = gravity;
    }
    
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Velocity += new Vector2(0, Gravity) * deltaTime;
        Position += Velocity * deltaTime;

        float targetRotation = MathHelper.PiOver2 * (Velocity.Y / 200f);
        Rotation = MathHelper.Lerp(Rotation, InitialDirection * targetRotation, 5f * deltaTime);
    }
}