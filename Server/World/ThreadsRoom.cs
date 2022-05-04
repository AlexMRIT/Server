using System;
using System.Linq;
using Server.Models;
using Server.Utilite;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Server.World
{
    public sealed class ThreadsRoom
    {
        private readonly Dictionary<int, Room> ServerRooms;
        private readonly IServiceProvider ServiceProvider;
        private readonly Config ServerConfig;

        public ThreadsRoom(IServiceProvider serviceProvider)
        {
            ServerRooms = new Dictionary<int, Room>();
            ServiceProvider = serviceProvider;
            ServerConfig = serviceProvider.GetService<Config>();

            for (int iterator = 0; iterator < ServerConfig.MaxOpenRoomPreStartServer; iterator++)
                ServerRooms.Add(IdFactory.Instance.NextId(), new Room(ServiceProvider));

            Console.WriteLine($"Initialized {ServerConfig.MaxOpenRoomPreStartServer} rooms.");
        }

        public bool IfExistEntityByRoomId(int id, Entity entity)
        {
            return ServerRooms.Where((item) => item.Key.Equals(id)).Any((result) => result.Value.IfExistEntity(entity));
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