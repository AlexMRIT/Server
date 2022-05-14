using Server.Utilite;

namespace Server.Network.InnerNetwork
{
    public sealed class CharacterDeath
    {
        internal static NetworkPacket ToPacket()
        {
            NetworkPacket packet = new NetworkPacket(OpcodeExtension.OpcodeServerCharacterDeath);

            return packet;
        }
    }
}