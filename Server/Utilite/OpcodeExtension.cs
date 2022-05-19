namespace Server.Utilite
{
    public static class OpcodeExtension
    {
        public static readonly byte OpcodeServerLoginFail = 0x00;
        public static readonly byte OpcodeServerLoginSuccess = 0x01;
        public static readonly byte OpcodeServerSendRoom = 0x02;
        public static readonly byte OpcodeServerDamageResult = 0x03;
        public static readonly byte OpcodeServerCharacterStopMove = 0x04;
        public static readonly byte OpcodeServerCharacterMoveToLocation = 0x05;
        public static readonly byte OpcodeServerCharacterUpdate = 0x06;
        public static readonly byte OpcodeServerCharacterDeath = 0x07;
        public static readonly byte OpcodeServerExitAllCharacterForRoom = 0x08;
        public static readonly byte OpcodeServerDeleteMe = 0x09;
        public static readonly byte OpcodeServerAddMe = 0x0A;
        public static readonly byte OpcodeServerEnterRoom = 0x0B;
    }
}