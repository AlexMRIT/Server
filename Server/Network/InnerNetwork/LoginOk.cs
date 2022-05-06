namespace Server.Network.InnerNetwork
{
    public sealed class LoginOk
    {
        private const byte Opcode = 0x01;

        internal static NetworkPacket ToPacket(ClientProcessor client)
        {
            NetworkPacket packet = new NetworkPacket(Opcode);

            packet.InternalWriteBool(client.CurrentSession.SessionClientAuthorization);
            packet.InternalWriteBool(client.CurrentSession.SessionClientMatchSearch);
            packet.InternalWriteBool(client.CurrentSession.SessionCLientGamePlaying);

            return packet;
        }
    }
}