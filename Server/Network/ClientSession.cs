#pragma warning disable CS0660
#pragma warning disable CS0661

namespace Server.Network
{
    public sealed class ClientSession
    {
        public bool SessionClientAuthorizationOk { get; set; }
        public bool SessionCLientGamePlaying { get; set; }

        public ClientSession(bool authorization, bool gamePlaying)
        {
            SessionClientAuthorizationOk = authorization;
            SessionCLientGamePlaying = gamePlaying;
        }

        public override string ToString()
        {
            return $"Authorization: {SessionClientAuthorizationOk}\tGame Playing: {SessionCLientGamePlaying}";
        }

        public static bool operator ==(ClientSession firstArg, ClientSession secondArg)
        {
            return firstArg is object && secondArg is object
                && firstArg.SessionClientAuthorizationOk == secondArg.SessionClientAuthorizationOk
                && firstArg.SessionCLientGamePlaying == secondArg.SessionCLientGamePlaying;
        }

        public static bool operator !=(ClientSession firstArg, ClientSession secondArg)
        {
            return !(firstArg == secondArg);
        }
    }
}