using System;
using Server.World;
using Server.Network;
using System.Threading.Tasks;
using System.Collections.Generic;
using Server.Network.InnerNetwork;
using Microsoft.Extensions.DependencyInjection;

namespace Server.RequestPacketHandler
{
    public sealed class RequestCreateServer : NetworkPacketBaseImplement
    {
        private readonly ClientProcessor Client;
        private readonly ThreadsRoom Rooms;
        private readonly Config ServerConfig;

        private readonly string ServerName;
        private readonly string DescriptionName;

        public RequestCreateServer(IServiceProvider serviceProvider, NetworkPacket packet, ClientProcessor client)
        {
            Client = client;
            Rooms = serviceProvider.GetService<ThreadsRoom>();
            ServerConfig = serviceProvider.GetService<Config>();

            ServerName = packet.ReadString(packet.ReadInt());
            DescriptionName = packet.ReadString(packet.ReadInt());
        }

        public async override Task ExecuteImplement()
        {
            Rooms.AddRoom(ServerName, DescriptionName);
            List<Room> rooms = new List<Room>();
            rooms.AddRange(Rooms.GetAllRooms());
            await Client.WriteAsync(SendRoom.ToPacket(rooms, Client.CurrentSession, ServerConfig));

            Console.WriteLine($"Create a new server. Name: {ServerName}, Description: {DescriptionName}");
        }
    }
}