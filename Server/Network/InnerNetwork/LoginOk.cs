using Server.Utilite;

namespace Server.Network.InnerNetwork
{
    public sealed class LoginOk
    {
        internal static NetworkPacket ToPacket(ClientProcessor client)
        {
            NetworkPacket packet = new NetworkPacket(OpcodeExtension.OpcodeServerLoginSuccess);

            packet.InternalWriteBool(client.CurrentSession.SessionClientAuthorization);
            packet.InternalWriteBool(client.CurrentSession.SessionClientMatchSearch);
            packet.InternalWriteBool(client.CurrentSession.SessionClientGamePlaying);

            return packet;
        }
    }
}