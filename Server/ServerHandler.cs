using System;
using System.Net;
using Server.Utilite;
using Server.Network;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable CS4014

namespace Server
{
    public sealed class ServerHandler
    {
        private readonly Config ServerConfig;
        private readonly ClientHandler ClientManager;
        private TcpListener TcpServerListener;

        public bool Initialised { get; private set; }

        public ServerHandler(IServiceProvider serviceProvider)
        {
            ServerConfig = serviceProvider.GetService<Config>();
            ClientManager = serviceProvider.GetService<ClientHandler>();
        }

        public void Initialise()
        {
            Debug.Assert(!Initialised, "TCP server is already started!");
            if (Initialised)
                return;

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, ServerConfig.Port);
            TcpServerListener = new TcpListener(endPoint);

            try
            {
                TcpServerListener.Start();
            }
            catch (SocketException exception)
            {
                ExceptionHandler.ExecuteSocketException(exception, nameof(ServerHandler.Initialise));
            }

            Console.WriteLine($"Listening GameServers on port {ServerConfig.Port}");
            Task.Factory.StartNew(WaitForClients);

            Initialised = true;
        }

        private async void WaitForClients()
        {
            while (true)
            {
                try
                {
                    TcpClient client = await TcpServerListener.AcceptTcpClientAsync();
                    Task.Factory.StartNew(() => AcceptClient(client));
                }
                catch (SocketException exception)
                {
                    ExceptionHandler.ExecuteSocketException(exception, nameof(WaitForClients));
                }
                catch (Exception exception)
                {
                    ExceptionHandler.Execute(exception, nameof(WaitForClients));
                }
            }
        }

        private void AcceptClient(TcpClient clientSocket)
        {
            Console.WriteLine($"Received connection request from: {clientSocket.Client.RemoteEndPoint}");
            ClientManager.AddClient(clientSocket);
        }
    }
}