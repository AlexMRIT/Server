using System;
using Server.World;
using Server.Enums;
using Server.Network;
using Server.Template;
using System.Threading.Tasks;

#pragma warning disable CS1998

namespace Server.Models
{
    public abstract class Entity
    {
        public readonly EntityTemplate Template;
        public readonly ThreadsRoom Rooms;

        public int RoomId { get; set; }

        public Entity(ThreadsRoom rooms, EntityTemplate entityTemplate)
        {
            Template = entityTemplate;
            Rooms = rooms;
        }
        public async virtual Task<DamageResult> TakeDamage(CharacterEntity target)
        {
            return DamageResult.DamageFail;
        }

        public async virtual Task BroadcastPacketAsync(NetworkPacket packet)
        {
            Rooms.GetEntitiesByRoomId(RoomId).ForEach(async action =>
            {
                if (action is CharacterEntity characterEntity)
                    await characterEntity.BroadcastPacketAsync(packet);
            });
        }
    }
}