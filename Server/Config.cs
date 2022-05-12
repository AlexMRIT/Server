namespace Server
{
    public sealed class Config
    {
        /// <summary>
        /// The maximum number of room treatments supported by the server. Default <see cref="int"/> 10.
        /// </summary>
        public readonly int MaxRoomGame = 10;

        /// <summary>
        /// The maximum number of connected clients per room. Including whoever created the room. Default <see cref="int"/> 2.
        /// </summary>
        public readonly int MaxCountPlayerForRoom = 2;

        /// <summary>
        /// How many rooms will be initialized by default when the server starts? The default is <see cref="int"/> 0.
        /// </summary>
        public readonly int MaxOpenRoomPreStartServer = 0;

        /// <summary>
        /// Server Port <see cref="int"/> 27015
        /// </summary>
        public readonly int Port = 27015;

        /// <summary>
        /// If the server did not find an account for the user. Will the server automatically create his account?
        /// </summary>
        public readonly bool AutoCreateAccount = true;

        /// <summary>
        /// Unsynchronized jump in motion given milliseconds.
        /// </summary>
        public readonly int MaxSpeedUpPerSecondUnsync = 20;

        /// <summary>
        /// How many ticks to update the position in the movement of the character <see cref="int"/>. Default 1000
        /// </summary>
        public readonly int DelayMillisecondsUpdatingCharacterPositions = 1000;
    }
}