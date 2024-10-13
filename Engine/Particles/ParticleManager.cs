using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Minesweeper.Particles;

public static class ParticleManager
{
    private static List<Particle> particles = new List<Particle>();

    public static void SpawnParticle(Particle particle, Vector2 position)
    {
        particle.Position = position;
        particles.Add(particle);
    }

    public static void SpawnInCircle(Particle particle, Vector2 position, float radius, int numberOfParticles = 5,
        float speed = 100f)
    {
        Vector2 center = new Vector2(position.X - particle.Sprite.width / 2, position.Y - particle.Sprite.width / 2);

        for (int i = 0; i < numberOfParticles; i++)
        {
            float angle = MathHelper.TwoPi * i / numberOfParticles;

            Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            Vector2 particlePosition = center + direction * radius;

            Vector2 velocity = direction * speed;

            var newParticle = particle is PhysicsParticle ? new PhysicsParticle(velocity)
            {
                Sprite = particle.Sprite,
            } : new Particle(particle);

            SpawnParticle(newParticle, particlePosition);
        }
    }


    public static void Update(GameTime gameTime)
    {
        for (int i = particles.Count - 1; i >= 0; i--)
        {
            particles[i].Update(gameTime);

            if (!particles[i].IsAlive)
            {
                particles.RemoveAt(i);
            }
        }
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
        foreach (var particle in particles)
        {
            float size = particle.CurrentLife / particle.Lifespan;
            
            spriteBatch.Draw(particle.SpriteSheet, particle.Position,
                particle.Sprite != null
                    ? new Rectangle(particle.Sprite.x, particle.Sprite.y, particle.Sprite.width,
                        particle.Sprite.height) : null, particle.Color * size,
                particle.Rotation, new Vector2(0.5f * particle.Size * size, 0.5f * particle.Size * size),
                particle.Size * size, SpriteEffects.None,
                0f);
        }
    }
}