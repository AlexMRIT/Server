using System.Threading.Tasks;

namespace Server.Network
{
    public abstract class NetworkPacketBaseImplement
    {
        public abstract Task ExecuteImplement();
    }
}