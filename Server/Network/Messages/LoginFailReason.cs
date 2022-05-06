namespace Server.Network.Messages
{
    public static class LoginFailReason
    {
        public static readonly byte ReasonUserOrPassWrong = 0x00;
        public static readonly byte ReasonAccountInUse = 0x01;
    }
}