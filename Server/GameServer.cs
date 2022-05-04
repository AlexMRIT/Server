using System;
using Server.World;
using Server.Service;
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

        public async void GameServerStart()
        {
            ServiceProvider.GetService<ThreadsRoom>();
            ServiceProvider.GetService<PreCheckStartServiceRepository>().Initialise();
            IdFactory.Instance.Initialise(ServiceProvider.GetService<IGetAllIdsInitialise>());
        }
    }
}