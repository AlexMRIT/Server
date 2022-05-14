using Server.Models;
using Server.Utilite;

namespace Server.Network.InnerNetwork
{
    public sealed class EnterRoom
    {
        internal static NetworkPacket ToPacket(CharacterEntity character, ClientSession session)
        {
            NetworkPacket packet = new NetworkPacket(OpcodeExtension.OpcodeServerDeleteMe);

            packet.WriteInt(character.CharacterTemplate.Id);
            packet.WriteInt(character.Score);
            packet.WriteString(character.Name);
            packet.WriteDouble(character.CharacterStats.AttackSpeed);
            packet.WriteDouble(character.CharacterStats.MoveSpeed);
            packet.WriteInt(character.CharacterStats.PhysicsAttackMin);
            packet.WriteInt(character.CharacterStats.PhysicsAttackMax);

            packet.InternalWriteBool(session.SessionClientAuthorization);
            packet.InternalWriteBool(session.SessionClientMatchSearch);
            packet.InternalWriteBool(session.SessionClientGamePlaying);

            return packet;
        }
    }
}