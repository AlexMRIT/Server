using System;
using Server.Models;
using Server.Utilite;
using Server.Network;
using System.Threading.Tasks;

namespace Server.RequestPacketHandler
{
    public sealed class RequestMovementAsync : NetworkPacketBaseImplement
    {
        private readonly ClientProcessor Client;

        private readonly Vector3 Direction;
        private readonly bool IsNewDirection;

        public RequestMovementAsync(IServiceProvider serviceProvider, NetworkPacket packet, ClientProcessor client)
        {
            Client = client;

            Direction = new Vector3(packet.ReadInt(), packet.ReadInt(), packet.ReadInt());
            IsNewDirection = packet.InternalReadBool();
        }

        public override async Task ExecuteImplement()
        {
            CharacterEntity player = Client.CurrentCharacter;

            if (IsNewDirection)
                player.CharacterMovement.UpdatePositionDirection(Direction);
            else
                await player.CharacterMovement.MoveTo(Direction);
        }
    }
}