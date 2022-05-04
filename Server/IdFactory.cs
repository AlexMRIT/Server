namespace Server
{
    public class IdFactory
    {
        public int CurrentId { get; private set; }

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