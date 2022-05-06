using System;
using Server.Models;
using Server.Network;
using System.Threading.Tasks;

namespace Server.RequestPacketHandler
{
    public sealed class RequestAttackHandle : NetworkPacketBaseImplement
    {
        private readonly ClientProcessor Client;
        private readonly int[] ObjectIds;

        public RequestAttackHandle(IServiceProvider serviceProvider, NetworkPacket packet, ClientProcessor client)
        {
            Client = client;

            int count = packet.ReadInt();
            ObjectIds = new int[count];
            for (int iterator = 0; iterator < count; iterator++)
                ObjectIds[iterator] = packet.ReadInt();
        }

        public override async Task ExecuteImplement()
        {
            CharacterEntity player = Client.CurrentCharacter;
        }
    }
}