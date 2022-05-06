using System;
using System.Linq;
using Server.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Server.World
{
    public sealed class Room
    {
        private readonly Entity[] entities;
        private readonly Config ServerConfig;

        public string RoomName { get; private set; }
        public string RoomDescription { get; private set; }

        public int CurrentCount = 0;

        public Room(IServiceProvider serviceProvider)
        {
            ServerConfig = serviceProvider.GetService<Config>();
            entities = new Entity[ServerConfig.MaxCountPlayerForRoom];
        }

        public Room Initialize(string name, string description)
        {
            RoomName = name;
            RoomDescription = description;

            return this;
        }

        public bool IfExistEntity(Entity entity)
        {
            return entities.Any((item) => item.Template.Id.Equals(entity.Template.Id));
        }

        public Entity GetEntityById(int id)
        {
            return entities.FirstOrDefault((item) => item.Template.Equals(id));
        }

        public bool AddEntity(Entity entity)
        {
            if (CurrentCount >= ServerConfig.MaxCountPlayerForRoom)
                return false;

            entities[CurrentCount] = entity;
            CurrentCount++;
            return true;
        }

        public bool RemoveEntity(Entity entity)
        {
            if (!IfExistEntity(entity))
                return false;

            entities[GetEntityByRoomId(entity.Template.Id)] = null;
            CurrentCount--;
            return true;
        }

        private int GetEntityByRoomId(int id)
        {
            int result = 0;
            for (int iterator = 0; iterator < entities.Length; iterator++)
                if (entities[iterator] != null && (bool)entities[iterator]?.Template.Id.Equals(id))
                    result++;

            return result;
        }

        public void RemoveAllEntity()
        {
            entities.Select((item) => item = null);
            CurrentCount = 0;
        }
    }
}