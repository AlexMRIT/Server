using System;
using Server.World;
using Server.Network;
using System.Threading.Tasks;
using System.Collections.Generic;
using Server.Network.InnerNetwork;
using Microsoft.Extensions.DependencyInjection;

namespace Server.RequestPacketHandler
{
    public sealed class RequestGetRoomsWindow : NetworkPacketBaseImplement
    {
        private readonly ClientProcessor Client;
        private readonly ClientSession CurrentClientSession;
        private readonly ThreadsRoom Rooms;

        public RequestGetRoomsWindow(IServiceProvider serviceProvider, NetworkPacket packet, ClientProcessor client)
        {
            Rooms = serviceProvider.GetService<ThreadsRoom>();
            Client = client;

            CurrentClientSession = new ClientSession(packet.InternalReadBool(), packet.InternalReadBool(), packet.InternalReadBool());
        }

        public override async Task ExecuteImplement()
        {
            if (CurrentClientSession != Client.CurrentSession)
            {
                Console.WriteLine($"Invalid SessionKey. AccountId: {Client.CurrectAccountContract.Id}");
                Client.Disconnect();
                return;
            }

            List<Room> roomsCollection = new List<Room>();
            roomsCollection.AddRange(Rooms.GetAllRooms());

            Client.CurrentSession.SessionClientMatchSearch = true;
            await Client.WriteAsync(SendRoom.ToPacket(roomsCollection, Client.CurrentSession));
        }
    }
}