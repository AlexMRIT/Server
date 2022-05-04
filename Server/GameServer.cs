using System;
using Server.World;
using Microsoft.Extensions.DependencyInjection;

namespace Server
{
    public sealed class GameServer
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly Config ServerConfig;

        public GameServer(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ServerConfig = serviceProvider.GetService<Config>();
        }

        public void GameServerStart()
        {
            ServiceProvider.GetService<ThreadsRoom>();
            ServiceProvider.GetService<PreCheckStartServiceRepository>().Initialise();
        }
    }
}