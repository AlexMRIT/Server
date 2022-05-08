using System;
using Server.Service;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Server
{
    public sealed class IdFactory
    {
        public bool Initialised { get; private set; }

        private int CurrentId;
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
                        _instance = new IdFactory();
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

        public int NextId()
        {
            return Interlocked.Increment(ref CurrentId);
        }
    }
}