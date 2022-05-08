using System;
using Server.Models;
using Server.Exceptions;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Server.World
{
    public sealed class Room
    {
        private readonly ConcurrentDictionary<int, Entity> EntityObjects;
        private readonly Config ServerConfig;
        private readonly string ExceptionMessageNotAccess = "Room access error. An uninitialized area.";

        public string RoomName { get; private set; }
        public string RoomDescription { get; private set; }
        public bool Initialized { get; private set; }

        public Room(IServiceProvider serviceProvider)
        {
            ServerConfig = serviceProvider.GetService<Config>();
            EntityObjects = new ConcurrentDictionary<int, Entity>();
        }

        public Room Initialize(string name, string description)
        {
            RoomName = name;
            RoomDescription = description;
            Initialized = true;

            return this;
        }

        public bool IfExistEntity(Entity entity)
        {
            if (!Initialized)
                throw new RoomInitializeException(ExceptionMessageNotAccess);

            return EntityObjects.ContainsKey(entity.Template.Id);
        }

        public Entity GetEntityById(int id)
        {
            if (!Initialized)
                throw new RoomInitializeException(ExceptionMessageNotAccess);

            if (!EntityObjects.ContainsKey(id))
                return null;

            return EntityObjects[id];
        }

        public IEnumerable<Entity> GetAllEntities()
        {
            foreach (KeyValuePair<int, Entity> element in EntityObjects)
                yield return element.Value;
        }

        public bool AddEntity(Entity entity)
        {
            if (!Initialized)
                throw new RoomInitializeException(ExceptionMessageNotAccess);

            if (GetCountEntitesInRoom() >= ServerConfig.MaxCountPlayerForRoom)
                return false;

            return EntityObjects.TryAdd(entity.Template.Id, entity);
        }

        public bool RemoveEntity(Entity entity)
        {
            if (!Initialized)
                throw new RoomInitializeException(ExceptionMessageNotAccess);

            if (!IfExistEntity(entity))
                return false;

            return EntityObjects.TryRemove(entity.Template.Id, out _);
        }

        private int GetCountEntitesInRoom()
        {
            if (!Initialized)
                throw new RoomInitializeException(ExceptionMessageNotAccess);

            return EntityObjects.Count;
        }

        public void RemoveAllEntity()
        {
            if (!Initialized)
                throw new RoomInitializeException(ExceptionMessageNotAccess);

            EntityObjects.Clear();
        }
    }
}