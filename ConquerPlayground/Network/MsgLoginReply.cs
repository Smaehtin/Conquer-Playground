namespace ConquerPlayground.Network
{
    internal class MsgLoginReply : NetMsg
    {
        private const int AccountIdOffset = 4;
        private const int DataOffset = 8;
        private const int ServerIpOffset = 12;
        private const int ServerPortOffset = 28;
        private const int InfoOffset = 30;
        private const int SizeOfMessage = InfoOffset;

        public MsgLoginReply(GameNetwork gameNetwork)
            : base(gameNetwork)
        {
        }

        public MsgLoginReply(GameNetwork gameNetwork, uint accountId, uint data, string serverIp, ushort serverPort)
            : base(gameNetwork, SizeOfMessage, MsgType.LoginReply)
        {
            this.AccountId = accountId;
            this.Data = data;
            this.ServerIp = serverIp;
            this.ServerPort = serverPort;
        }

        private uint AccountId
        {
            get { return this.ReadUInt32(AccountIdOffset); }
            set { this.WriteUInt32(AccountIdOffset, value); }
        }

        private uint Data
        {
            get { return this.ReadUInt32(DataOffset); }
            set { this.WriteUInt32(DataOffset, value); }
        }

        private string ServerIp
        {
            get { return this.ReadFixedString(ServerIpOffset, 16); }
            set { this.WriteFixedString(ServerIpOffset, value); }
        }

        private ushort ServerPort
        {
            get { return this.ReadUInt16(ServerPortOffset); }
            set { this.WriteUInt16(ServerPortOffset, value); }
        }

        public override void Process()
        {
            if (this.Data == 1337)
            {
                new MsgTalk(this.GameNetwork, "SYSTEM", "ALLUSERS", "ANSWER_OK", textAttribute: TextAttribute.Entrance).Send();
                new MsgTalk(this.GameNetwork, "SYSTEM", "Tester", "Welcome to Conquer Playground").Send();
                new MsgUserInfo(this.GameNetwork, "Tester").Send();
            }
        }
    }
}
