using System;
using System.Net.Sockets;
using System.Collections.Concurrent;

namespace Server.Network
{
    public sealed class ClientHandler
    {
        private readonly IServiceProvider ServiceProvider;

        private readonly ConcurrentDictionary<string, DateTime> FloodClientCollection;
        private readonly ConcurrentDictionary<string, ClientProcessor> ClientSuccessfullyRegister;
        private readonly GamePacketHandler PacketHandler;

        public ClientHandler(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            FloodClientCollection = new ConcurrentDictionary<string, DateTime>();
            ClientSuccessfullyRegister = new ConcurrentDictionary<string, ClientProcessor>();
            PacketHandler = new GamePacketHandler();
        }

        public void AddClient(TcpClient client)
        {

        }
    }
}