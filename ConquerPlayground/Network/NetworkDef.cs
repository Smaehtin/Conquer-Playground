namespace ConquerPlayground.Network
{
    internal enum MsgType : ushort
    {
        UserInfo = 0x3EE,
        Talk = 0x3EC,
        ItemInfo = 0x3F0,
        Item = 0x3F1,
        Action = 0x3F2,
        Player = 0x3F6,
        Login = 0x41B,
        LoginReply = 0x41C
    }
}