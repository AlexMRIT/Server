using System;
using Server.World;
using Server.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Server
{
    public sealed class GameServer
    {
        private readonly IServiceProvider ServiceProvider;

        public GameServer(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public async void GameServerStart()
        {
            ServiceProvider.GetService<ThreadsRoom>();
            await ServiceProvider.GetService<PreCheckStartServiceRepository>().Initialize();
            await IdFactory.Instance.Initialize(ServiceProvider.GetService<IGetAllIdsInitialise>());
            ServiceProvider.GetService<ServerHandler>().Initialize();
        }
    }
}