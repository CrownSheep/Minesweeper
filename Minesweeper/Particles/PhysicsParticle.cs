using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.Graphics;

namespace Minesweeper.Particles;

public class PhysicsParticle : Particle
{
    public Vector2 Velocity;
    public float Gravity = 98f;
    private int InitialRotation => Velocity.X > 0 ? 1 : -1;
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Velocity += new Vector2(0, Gravity) * deltaTime;
        Position += Velocity * deltaTime;

        float targetRotation = MathHelper.PiOver2 * (Velocity.Y / 200f);
        Rotation = MathHelper.Lerp(Rotation, InitialRotation * targetRotation, 5f * deltaTime);
    }
}