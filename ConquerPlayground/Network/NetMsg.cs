namespace ConquerPlayground.Network
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    internal class NetMsg
    {
        private const int MaxMsgSize = 1024;

        private GameNetwork gameNetwork;
        private BinaryReader reader;
        private MemoryStream stream;
        private BinaryWriter writer;

        protected NetMsg(GameNetwork gameNetwork)
        {
            this.gameNetwork = gameNetwork;
            this.stream = new MemoryStream(new byte[MaxMsgSize], 0, MaxMsgSize, true, true);
            this.reader = new BinaryReader(this.stream);
            this.writer = new BinaryWriter(this.stream);
        }

        protected NetMsg(GameNetwork gameNetwork, ushort msgSize, MsgType msgType)
            : this(gameNetwork)
        {
            this.MsgSize = msgSize;
            this.MsgType = msgType;
        }

        public ushort MsgSize
        {
            get { return this.ReadUInt16(0); }
            protected set { this.WriteUInt16(0, value); }
        }

        public MsgType MsgType
        {
            get { return (MsgType)this.ReadInt16(2); }
            protected set { this.WriteInt16(2, (short)value); }
        }

        public byte[] Buffer
        {
            get { return this.stream.GetBuffer(); }
        }

        protected GameNetwork GameNetwork
        {
            get { return this.gameNetwork; }
        }

        public static NetMsg Create(GameNetwork gameNetwork, IntPtr buffer, int size)
        {
            if (buffer == IntPtr.Zero)
            {
                throw new ArgumentException("buffer");
            }

            if (size < 0 || size > MaxMsgSize)
            {
                throw new ArgumentException("size");
            }

            var msgType = (MsgType)Marshal.ReadInt16(buffer, 2);
            NetMsg result;

            switch (msgType)
            {
                case MsgType.Action:
                    result = new MsgAction(gameNetwork);
                    break;

                case MsgType.Item:
                    result = new MsgItem(gameNetwork);
                    break;

                case MsgType.ItemInfo:
                    result = new MsgItemInfo(gameNetwork);
                    break;

                case MsgType.Login:
                    result = new MsgLogin(gameNetwork);
                    break;

                case MsgType.LoginReply:
                    result = new MsgLoginReply(gameNetwork);
                    break;

                case MsgType.Talk:
                    result = new MsgTalk(gameNetwork);
                    break;

                default:
                    result = new NetMsg(gameNetwork);
                    break;
            }

            result.Create(buffer, size);
            return result;
        }

        public virtual void Process()
        {
        }

        public void Send()
        {
            this.gameNetwork.SendMsg(this);
        }

        public override string ToString()
        {
            return string.Format("Size: 0x{0:X}, Type: 0x{1:X}, Data: {2}", this.MsgSize, (short)this.MsgType, BitConverter.ToString(this.Buffer, 0, this.MsgSize).Replace('-', ' '));
        }

        protected byte ReadByte(int offset)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            return this.reader.ReadByte();
        }

        protected byte[] ReadBytes(int offset, int count)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            return this.reader.ReadBytes(count);
        }

        protected string ReadFixedString(int offset, int length)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            var bytes = this.reader.ReadBytes(length);

            return Encoding.ASCII.GetString(bytes).TrimEnd('\0');
        }

        protected short ReadInt16(int offset)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            return this.reader.ReadInt16();
        }

        protected int ReadInt32(int offset)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            return this.reader.ReadInt32();
        }

        protected long ReadInt64(int offset)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            return this.reader.ReadInt64();
        }

        protected string[] ReadStrings(int offset)
        {
            var strAmount = this.ReadByte(offset);
            offset++;

            var result = new string[strAmount];

            for (var i = 0; i < strAmount; i++)
            {
                var strLen = this.ReadByte(offset);
                offset++;

                var strBytes = this.ReadBytes(offset, strLen);
                result[i] = Encoding.ASCII.GetString(strBytes);

                offset += strLen;
            }

            return result;
        }

        protected ushort ReadUInt16(int offset)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            return this.reader.ReadUInt16();
        }

        protected uint ReadUInt32(int offset)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            return this.reader.ReadUInt32();
        }

        protected ulong ReadUInt64(int offset)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            return this.reader.ReadUInt64();
        }

        protected void WriteByte(int offset, byte value)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            this.writer.Write(value);
        }

        protected void WriteBytes(int offset, byte[] bytes)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            this.writer.Write(bytes);
        }

        protected void WriteFixedString(int offset, string value)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            var bytes = Encoding.ASCII.GetBytes(value);

            this.writer.Write(bytes);
        }

        protected void WriteInt16(int offset, short value)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            this.writer.Write(value);
        }

        protected void WriteInt32(int offset, int value)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            this.writer.Write(value);
        }

        protected void WriteInt64(int offset, long value)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            this.writer.Write(value);
        }

        protected ushort WriteStrings(int offset, params string[] strings)
        {
            var originalOffset = offset;

            this.WriteByte(offset, (byte)strings.Length);
            offset++;

            foreach (var str in strings)
            {
                var strBytes = Encoding.ASCII.GetBytes(str);

                this.WriteByte(offset, (byte)strBytes.Length);
                offset++;

                this.WriteBytes(offset, strBytes);
                offset += strBytes.Length;
            }

            return (ushort)(offset - originalOffset);
        }

        protected void WriteUInt16(int offset, ushort value)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            this.writer.Write(value);
        }

        protected void WriteUInt32(int offset, uint value)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            this.writer.Write(value);
        }

        protected void WriteUInt64(int offset, ulong value)
        {
            this.stream.Seek(offset, SeekOrigin.Begin);
            this.writer.Write(value);
        }

        private void Create(IntPtr buffer, int size)
        {
            Marshal.Copy(buffer, this.Buffer, 0, size);
        }
    }
}