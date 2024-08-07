using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper.Entities
{
    public class EntityManager
    {
        private readonly List<IGameEntity> entities = new List<IGameEntity>();
        
        private readonly List<IGameEntity> entitiesToAdd = new List<IGameEntity>();
        private readonly List<IGameEntity> entitiesToRemove = new List<IGameEntity>();

        public IEnumerable<IGameEntity> Entities => new ReadOnlyCollection<IGameEntity>(entities);

        public void AddEntity(IGameEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity), "Null cannot be added as an entity");

            entitiesToAdd.Add(entity);
        }
        
        public void RemoveEntity(IGameEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity), "Null is not a entity.");
            
            entitiesToRemove.Add(entity);
        }

        public void UpdateEntities(GameTime gameTime)
        {
            foreach (IGameEntity entity in entities)
            {
                if(entitiesToRemove.Contains(entity))
                    continue;
                
                entity.Update(gameTime);
            }

            foreach (IGameEntity entity in entitiesToAdd)
            {
                entities.Add(entity);
            }
                        
            foreach (IGameEntity entity in entitiesToRemove)
            {
                entities.Remove(entity);
            }

            entitiesToAdd.Clear();
            entitiesToRemove.Clear();
        } 
        
        public void DrawEntities(SpriteBatch batch, GameTime gameTime)
        {
            foreach (IGameEntity entity in entities.OrderBy(e => e.DrawOrder))
            {
                entity.Draw(batch, gameTime);
            }
        }
        
        public void Clear()
        {
            entitiesToRemove.AddRange(entities);
        }

        public IEnumerable<T> GetEntitiesOfType<T>() where T : IGameEntity
        {
            return entities.OfType<T>();
        }
    }
}