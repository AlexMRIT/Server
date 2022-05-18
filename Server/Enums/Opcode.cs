namespace Server.Enums
{
    public enum Opcode : byte
    {
        RequestAuthorization = 0x00,
        RequestGetRoomsWindow = 0x01,
        RequestMovementAsync = 0x02,
        RequestAttackHandle = 0x03,
        RequestEnterRoom = 0x04,
        RequestMovementStopSync = 0x05,
        RequestCreateServer = 0x06
    }
}