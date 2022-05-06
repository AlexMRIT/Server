using System;
using Server.World;
using Server.Network;
using System.Threading.Tasks;
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

            foreach (Room room in Rooms.GetAllRooms())
            {
                if (!Client.IsDisconnected)
                    await Client.WriteAsync(SendRoom.ToPacket(room));
            }
        }
    }
}