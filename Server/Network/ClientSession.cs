#pragma warning disable CS0660
#pragma warning disable CS0661

namespace Server.Network
{
    public sealed class ClientSession
    {
        public bool SessionClientAuthorization { get; set; }
        public bool SessionClientMatchSearch { get; set; }
        public bool SessionCLientGamePlaying { get; set; }

        public ClientSession(bool authorization, bool matchSearch, bool gamePlaying)
        {
            SessionClientAuthorization = authorization;
            SessionClientMatchSearch = matchSearch;
            SessionCLientGamePlaying = gamePlaying;
        }

        public override string ToString()
        {
            return $"Authorization: {SessionClientAuthorization}\tMatch Search: {SessionClientMatchSearch}\tGame Playing: {SessionCLientGamePlaying}";
        }

        public static bool operator ==(ClientSession firstArg, ClientSession secondArg)
        {
            return firstArg is object && secondArg is object
                && firstArg.SessionClientAuthorization == secondArg.SessionClientAuthorization
                && firstArg.SessionClientMatchSearch == secondArg.SessionClientMatchSearch
                && firstArg.SessionCLientGamePlaying == secondArg.SessionCLientGamePlaying;
        }

        public static bool operator !=(ClientSession firstArg, ClientSession secondArg)
        {
            return !(firstArg == secondArg);
        }
    }
}