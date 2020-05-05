namespace ConquerPlayground.Network
{
    internal class MsgLogin : NetMsg
    {
        public MsgLogin(GameNetwork gameNetwork)
            : base(gameNetwork)
        {
        }

        public override void Process()
        {
            new MsgLoginReply(this.GameNetwork, 1, 1337, "99.99.99.99", 5816).Send();
        }
    }
}
