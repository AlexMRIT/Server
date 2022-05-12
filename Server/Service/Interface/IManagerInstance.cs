namespace Server.Service.Interface
{
    public interface IManagerInstance
    {
        public void SetOnline(int roomId);
        public void SetOffline();
    }
}