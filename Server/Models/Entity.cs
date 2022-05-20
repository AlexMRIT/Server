using System.Linq;
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

        public async virtual Task<DamageResult> TakeDamageAsync(CharacterEntity target)
        {
            return DamageResult.DamageFail;
        }

        public async virtual Task BroadcastPacketAsync(NetworkPacket packet, bool excludeYourself = true)
        {
            await Task.WhenAll(Rooms.GetEntitiesByRoomId(RoomId).Select(async action =>
            {
                if (!excludeYourself)
                {
                    if ((action is CharacterEntity character) && character != this)
                        await character.SendOnlyMe(packet);
                }
                else
                {
                    if (action is CharacterEntity character)
                        await character.SendOnlyMe(packet);
                }
            }));
        }
    }
}