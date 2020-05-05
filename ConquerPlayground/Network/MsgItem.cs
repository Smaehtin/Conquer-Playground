namespace ConquerPlayground.Network
{
    internal enum ItemAction : ushort
    {
        Buy = 0x1,
        Sell = 0x2,
        Drop = 0x3,
        Use = 0x4,
        Equip = 0x5,
        Unequip = 0x6,
        Ping = 0x1B
    }

    internal class MsgItem : NetMsg
    {
        private const int IdOffset = 4;
        private const int DataOffset = 8;
        private const int PosXOffset = 8;
        private const int PosYOffset = 10;
        private const int ActionOffset = 12;
        private const int TimestampOffset = 16;
        private const int AmountOffset = 20;
        private const int SizeOfMessage = AmountOffset + 4;

        public MsgItem(GameNetwork gameNetwork)
            : base(gameNetwork)
        {
        }

        public MsgItem(GameNetwork gameNetwork, uint id, ItemAction action, byte position)
            : base(gameNetwork, SizeOfMessage, MsgType.Item)
        {
            this.Id = id;
            this.Data = position;
            this.Action = action;
        }

        private uint Id
        {
            get { return this.ReadUInt32(IdOffset); }
            set { this.WriteUInt32(IdOffset, value); }
        }

        private uint Data
        {
            get { return this.ReadUInt32(DataOffset); }
            set { this.WriteUInt32(DataOffset, value); }
        }

        private ushort PosX
        {
            get { return this.ReadUInt16(PosXOffset); }
            set { this.WriteUInt16(PosXOffset, value); }
        }

        private ushort PosY
        {
            get { return this.ReadUInt16(PosYOffset); }
            set { this.WriteUInt16(PosYOffset, value); }
        }

        private ItemAction Action
        {
            get { return (ItemAction)this.ReadInt16(ActionOffset); }
            set { this.WriteInt16(ActionOffset, (short)value); }
        }

        private uint Timestamp
        {
            get { return this.ReadUInt32(TimestampOffset); }
            set { this.WriteUInt32(TimestampOffset, value); }
        }

        private uint Amount
        {
            get { return this.ReadUInt32(AmountOffset); }
            set { this.WriteUInt32(AmountOffset, value); }
        }

        public override void Process()
        {
            switch (this.Action)
            {
                case ItemAction.Buy:
                    var shopId = this.Id;
                    var itemType = this.Data;
                    var amount = this.Amount;

                    new MsgItemInfo(this.GameNetwork, itemType).Send();
                    break;

                case ItemAction.Drop:
                    this.Send();
                    break;

                case ItemAction.Use:
                    var itemId = this.Id;
                    var position = (byte)this.Data;

                    if (position > 0)
                    {
                        new MsgItem(this.GameNetwork, itemId, ItemAction.Equip, position).Send();
                    }
                    else
                    {
                        new MsgTalk(this.GameNetwork, "SYSTEM", "Tester", "Unable to use this item", textAttribute: TextAttribute.Normal).Send();
                    }

                    break;

                case ItemAction.Unequip:
                    this.Send();
                    break;

                // Ignore this since the client displays the wrong ping anyways
                case ItemAction.Ping:
                    break;

                default:
                    break;
            }
        }
    }
}
