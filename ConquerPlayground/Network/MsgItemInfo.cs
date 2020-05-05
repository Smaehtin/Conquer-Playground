namespace ConquerPlayground.Network
{
    internal enum ItemInfoAction : byte
    {
        Add = 1,
        Trade = 2,
        Update = 3
    }

    internal class MsgItemInfo : NetMsg
    {
        // Missing: Item color?
        private const int IdOffset = 4;
        private const int TypeOffset = 8;
        private const int AmountOffset = 12;
        private const int AmountLimitOffset = 14;
        private const int ActionOffset = 16;
        private const int StatusOffset = 17;
        private const int PositionOffset = 18;
        private const int Gem1Offset = 24;
        private const int Gem2Offset = 25;
        private const int BonusLevelOffset = 28;
        private const int DamageReductionOffset = 29;
        private const int BonusHealthOffset = 30;
        private const int SizeOfMessage = BonusHealthOffset + 1;

        // Used for simple "state" simulation
        private static uint itemId = 0;

        public MsgItemInfo(GameNetwork gameNetwork)
            : base(gameNetwork)
        {
        }

        public MsgItemInfo(GameNetwork gameNetwork, uint type)
            : base(gameNetwork, SizeOfMessage, MsgType.ItemInfo)
        {
            itemId++;

            this.Id = itemId;
            this.Type = type;
            this.Action = ItemInfoAction.Add;
            this.Position = 0;
            System.Diagnostics.Debug.WriteLine(string.Format("Creating item {0}", type), "[INFO]");
        }

        private uint Id
        {
            get { return this.ReadUInt32(IdOffset); }
            set { this.WriteUInt32(IdOffset, value); }
        }

        private uint Type
        {
            get { return this.ReadUInt32(TypeOffset); }
            set { this.WriteUInt32(TypeOffset, value); }
        }

        private ushort Amount
        {
            get { return this.ReadUInt16(AmountOffset); }
            set { this.WriteUInt16(AmountOffset, value); }
        }

        private ushort AmountLimit
        {
            get { return this.ReadUInt16(AmountLimitOffset); }
            set { this.WriteUInt16(AmountLimitOffset, value); }
        }

        private ItemInfoAction Action
        {
            get { return (ItemInfoAction)this.ReadByte(ActionOffset); }
            set { this.WriteByte(ActionOffset, (byte)value); }
        }

        private byte Status
        {
            get { return this.ReadByte(StatusOffset); }
            set { this.WriteByte(StatusOffset, value); }
        }

        private byte Position
        {
            get { return this.ReadByte(PositionOffset); }
            set { this.WriteByte(PositionOffset, value); }
        }

        private byte Gem1
        {
            get { return this.ReadByte(Gem1Offset); }
            set { this.WriteByte(Gem1Offset, value); }
        }

        private byte Gem2
        {
            get { return this.ReadByte(Gem2Offset); }
            set { this.WriteByte(Gem2Offset, value); }
        }

        private byte BonusLevel
        {
            get { return this.ReadByte(BonusLevelOffset); }
            set { this.WriteByte(BonusLevelOffset, value); }
        }

        private byte DamageReduction
        {
            get { return this.ReadByte(DamageReductionOffset); }
            set { this.WriteByte(DamageReductionOffset, value); }
        }

        private byte BonusHealth
        {
            get { return this.ReadByte(BonusHealthOffset); }
            set { this.WriteByte(BonusHealthOffset, value); }
        }

        public override void Process()
        {
        }
    }
}