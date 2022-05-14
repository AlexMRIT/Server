using Server.Models;
using Server.Utilite;

namespace Server.Network.InnerNetwork
{
    public sealed class DeleteMe
    {
        internal static NetworkPacket ToPacket(CharacterEntity character, ClientSession session)
        {
            NetworkPacket packet = new NetworkPacket(OpcodeExtension.OpcodeServerDeleteMe);

            packet.WriteInt(character.CharacterTemplate.Id);
            packet.InternalWriteBool(session.SessionClientAuthorization);
            packet.InternalWriteBool(session.SessionClientMatchSearch);
            packet.InternalWriteBool(session.SessionClientGamePlaying);

            return packet;
        }
    }
}