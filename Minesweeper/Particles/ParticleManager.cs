using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.DataHolders;
using Minesweeper.Entities;
using Minesweeper.Particles;

namespace Minesweeper.Particles;

public static class ParticleManager
{
    private static List<Particle> particles = new List<Particle>();
    private static Random random = new Random();

    public static void SpawnParticle(Particle particle, Vector2 position)
    {
        particle.Position = position;
        particles.Add(particle);
    }

    public static void SpawnInCircle(Particle particle, Vector2 position, float radius, int numberOfParticles = 5,
        float speed = 100f)
    {
        Vector2 center = new Vector2(position.X, position.Y);

        for (int i = 0; i < numberOfParticles; i++)
        {
            // Calculate the angle for this particle
            float angle = MathHelper.TwoPi * i / numberOfParticles;

            // Calculate the direction for the particle to move in
            Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            // Calculate the initial position of the particle on the circle's edge
            Vector2 particlePosition = center + direction * radius;

            // Calculate the velocity by scaling the direction with the speed
            Vector2 velocity = direction * speed;

            var newParticle = particle is PhysicsParticle ? new PhysicsParticle(velocity) : new Particle(particle);

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
                        particle.Sprite.height) : null, particle.Color,
                particle.Rotation, new Vector2(0.5f * particle.Size * size, 0.5f * particle.Size * size),
                particle.Size * size, SpriteEffects.None,
                0f);
        }
    }
}