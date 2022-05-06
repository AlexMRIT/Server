using System;
using Server.Service;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Server
{
    public sealed class IdFactory
    {
        public int CurrentId { get; private set; }
        public bool Initialised { get; private set; }

        private static volatile IdFactory _instance;
        private static readonly object SyncRoot = new object();

        public static IdFactory Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new IdFactory(0);
                }

                return _instance;
            }
        }

        public async Task Initialize(IGetAllIdsInitialise getAll)
        {
            Debug.Assert(!Initialised, $"{nameof(IdFactory)} is already Initialise!");
            if (Initialised)
                return;

            CurrentId += (await getAll.GetAccountIdsListAsync()).Count;
            CurrentId += (await getAll.GetPlayerIdsListAsync()).Count;

            Console.WriteLine($"An initialized object indexer. Total {CurrentId}");
            Initialised = true;
        }

        public IdFactory(int defaultId)
        {
            CurrentId = defaultId;
        }

        public int NextId()
        {
            return ++CurrentId;
        }
    }
}