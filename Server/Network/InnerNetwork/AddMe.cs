using Server.Models;
using Server.Utilite;

namespace Server.Network.InnerNetwork
{
    public sealed class AddMe
    {
        internal static NetworkPacket ToPacket(CharacterEntity character)
        {
            NetworkPacket packet = new NetworkPacket(OpcodeExtension.OpcodeServerAddMe);

            packet.WriteInt(character.CharacterTemplate.Id);

            return packet;
        }
    }
}