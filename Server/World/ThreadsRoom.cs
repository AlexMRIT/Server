using System;
using System.Linq;
using Server.Models;
using Server.Utilite;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Server.World
{
    public sealed class ThreadsRoom
    {
        private readonly ConcurrentDictionary<int, Room> ServerRooms;
        private readonly IServiceProvider ServiceProvider;
        private readonly Config ServerConfig;

        public ThreadsRoom(IServiceProvider serviceProvider)
        {
            ServerRooms = new ConcurrentDictionary<int, Room>();
            ServiceProvider = serviceProvider;
            ServerConfig = serviceProvider.GetService<Config>();

            for (int iterator = 0; iterator < ServerConfig.MaxOpenRoomPreStartServer; iterator++)
            {
                int id = IdFactory.Instance.NextId();
                ServerRooms.TryAdd(id, new Room(ServiceProvider, id).Initialize($"Room {iterator}", "Server Room"));
            }

            Console.WriteLine($"Initialized {ServerConfig.MaxOpenRoomPreStartServer} rooms.");
        }

        public bool IfExistEntityByRoomId(int id, Entity entity)
        {
            try
            {
                return ServerRooms[id].IfExistEntity(entity);
            }
            catch (IndexOutOfRangeException exception)
            {
                ExceptionHandler.Execute(exception, nameof(IfExistEntityByRoomId));
                return false;
            }
        }

        public List<Entity> GetEntitiesByRoomId(int id)
        {
            try
            {
                return new List<Entity>(ServerRooms[id].GetAllEntities());
            }
            catch (IndexOutOfRangeException exception)
            {
                ExceptionHandler.Execute(exception, nameof(IfExistEntityByRoomId));
                return new List<Entity>(0);
            }
        }

        public Entity GetEntityById(int idRoom, int id)
        {
            try
            {
                return ServerRooms[idRoom].GetEntityById(id);
            }
            catch (IndexOutOfRangeException exception)
            {
                ExceptionHandler.Execute(exception, nameof(GetEntityById));
                return null;
            }
        }

        public bool AddEntity(int idRoom, Entity entity)
        {
            try
            {
                return ServerRooms[idRoom].AddEntity(entity);
            }
            catch (IndexOutOfRangeException exception)
            {
                ExceptionHandler.Execute(exception, nameof(AddEntity));
                return false;
            }
        }

        public void AddRoom(string name, string description)
        {
            int id = IdFactory.Instance.NextId();
            ServerRooms.TryAdd(id, new Room(ServiceProvider, id).Initialize(name, description));
        }

        public IEnumerable<Room> GetAllRooms()
        {
            foreach (KeyValuePair<int, Room> room in ServerRooms)
                yield return room.Value;
        }

        public bool RemoveEntity(int idRoom, Entity entity)
        {
            try
            {
                return ServerRooms[idRoom].RemoveEntity(entity);
            }
            catch (IndexOutOfRangeException exception)
            {
                ExceptionHandler.Execute(exception, nameof(RemoveEntity));
                return false;
            }
        }

        public void RemoveRoom(int idRoom)
        {
            if (!ServerRooms.ContainsKey(idRoom))
                return;

            RemoveAllEntityInRoom(idRoom);
            ServerRooms[idRoom] = null;
        }

        public void RemoveAllEntityInRoom(int idRoom)
        {
            try
            {
                ServerRooms[idRoom].RemoveAllEntity();
            }
            catch (IndexOutOfRangeException exception)
            {
                ExceptionHandler.Execute(exception, nameof(RemoveEntity));
            }
        }

        public void RemoveAllRoom()
        {
            ServerRooms.ToList().ForEach((action) => action.Value.RemoveAllEntity());
            ServerRooms.Clear();
        }
    }
}