using Server.Utilite;

namespace Server.Network.InnerNetwork
{
    public sealed class ExitAllRoomCmd
    {
        internal static NetworkPacket ToPacket()
        {
            return new NetworkPacket(OpcodeExtension.OpcodeServerExitAllCharacterForRoom);
        }
    }
}