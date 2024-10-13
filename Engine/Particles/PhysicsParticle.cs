using Microsoft.Xna.Framework;

namespace Minesweeper.Particles;

public class PhysicsParticle : Particle
{
    public Vector2 Velocity;
    public float Gravity = 98f;
    private int InitialDirection => Velocity.X > 0 ? 1 : -1;

    public PhysicsParticle(Vector2 velocity = default)
    {
        Velocity = velocity;
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