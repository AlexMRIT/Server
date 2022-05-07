using System;
using Server.Enums;
using Server.World;
using Server.Models;
using Server.Network;
using System.Threading.Tasks;
using Server.Network.InnerNetwork;
using Microsoft.Extensions.DependencyInjection;

namespace Server.RequestPacketHandler
{
    public sealed class RequestAttackHandle : NetworkPacketBaseImplement
    {
        private readonly ClientProcessor Client;
        private readonly ThreadsRoom Rooms;
        private readonly int[] ObjectIds;

        public RequestAttackHandle(IServiceProvider serviceProvider, NetworkPacket packet, ClientProcessor client)
        {
            Rooms = serviceProvider.GetService<ThreadsRoom>();
            Client = client;

            int count = packet.ReadInt();
            ObjectIds = new int[count];
            for (int iterator = 0; iterator < count; iterator++)
                ObjectIds[iterator] = packet.ReadInt();
        }

        public override async Task ExecuteImplement()
        {
            CharacterEntity player = Client.CurrentCharacter;

            for (int iterator = 0; iterator < ObjectIds.Length; iterator++)
            {
                CharacterEntity target = (CharacterEntity)Rooms.GetEntityById(player.RoomId, ObjectIds[iterator]);

                if (target.IsDead)
                    continue;

                DamageResult damageResult = await player.TakeDamage(target);
                await Client.WriteAsync(SendDamageResult.ToPacket(damageResult));
            }
        }
    }
}