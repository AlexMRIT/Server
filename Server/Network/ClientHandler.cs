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
            PacketHandler = new GamePacketHandler(ServiceProvider);
        }

        public void AddClient(TcpClient client)
        {
            string ip = client.Client.RemoteEndPoint.ToString().Split(':')[0];

            if (FloodClientCollection.ContainsKey(ip))
            {
                if (FloodClientCollection[ip].CompareTo(DateTime.UtcNow) == 1)
                {
                    Console.WriteLine($"Active flooder: {ip}");
                    client.Close();
                    return;
                }

                FloodClientCollection.TryRemove(ip, out _);
            }

            FloodClientCollection.AddOrUpdate(ip, DateTime.UtcNow.AddMilliseconds(3000), (a, b) => DateTime.UtcNow.AddMilliseconds(3000));
            ClientProcessor gameClient = new ClientProcessor(this, client, PacketHandler);

            if (!ClientSuccessfullyRegister.TryAdd(client.Client.RemoteEndPoint.ToString(), gameClient))
            {
                Console.WriteLine("Duplicate connection client!");
                gameClient.Disconnect();
                return;
            }
            Console.WriteLine($"{ClientSuccessfullyRegister.Count} active connections");
        }

        public void ClientDisconnectFromRegister(string ip)
        {
            ClientSuccessfullyRegister.TryRemove(ip, out _);
            Console.WriteLine($"{ClientSuccessfullyRegister.Count} active connections");
        }

        public ClientProcessor LoggedAlready(string ip)
        {
            if (!ClientSuccessfullyRegister.ContainsKey(ip))
                return null;

            return ClientSuccessfullyRegister[ip];
        }
    }
}