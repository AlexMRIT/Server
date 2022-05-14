using System;
using Server.Models;
using Server.Utilite;
using Server.Network;
using System.Threading.Tasks;

#pragma warning disable CS1998

namespace Server.RequestPacketHandler
{
    public sealed class RequestMovementStopSync : NetworkPacketBaseImplement
    {
        private readonly ClientProcessor Client;

        private readonly Vector3 Direction;

        public RequestMovementStopSync(IServiceProvider serviceProvider, NetworkPacket packet, ClientProcessor client)
        {
            Client = client;

            Direction = new Vector3(packet.ReadInt(), packet.ReadInt(), packet.ReadInt());
        }

        public override async Task ExecuteImplement()
        {
            CharacterEntity player = Client.CurrentCharacter;
            player.CharacterMovement.NotifyStopMoveByPosition(Direction);
        }
    }
}