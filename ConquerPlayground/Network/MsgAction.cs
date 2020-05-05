namespace ConquerPlayground.Network
{
    using System;

    internal enum ActionType : ushort
    {
        EnterMap = 0x4A,
        GetItems = 0x4B,
        GetFriends = 0x4C,
        GetProficiencies = 0x4D,
        GetMagic = 0x4E,
        RequestChangeMap = 0x55,
        ChangeMap = 0x56,
        SetPkMode = 0x60,
        GetGuildInfo = 0x61,
        DestroyBooth = 0x72,
        GetTutorInfo = 0x82,
        Jump = 0x85,
    }

    internal class MsgAction : NetMsg
    {
        private const int TimestampOffset = 4;
        private const int UserIdOffset = 8;
        private const int DataOffset = 12;
        private const int PosXOffset = 12;
        private const int PosYOffset = 14;
        private const int DataExtraOffset = 16;
        private const int DestXOffset = 16;
        private const int DestYOffset = 18;
        private const int DirectionOffset = 20;
        private const int ActionOffset = 22;
        private const int SizeOfMessage = ActionOffset + 2;

        public MsgAction(GameNetwork gameNetwork)
            : base(gameNetwork)
        {
        }

        public MsgAction(GameNetwork gameNetwork, uint userId, uint data, ushort destX, ushort destY, ushort direction, ActionType action)
            : base(gameNetwork, SizeOfMessage, MsgType.Action)
        {
            this.Timestamp = (uint)Environment.TickCount;
            this.UserId = userId;
            this.Data = data;
            this.DestX = destX;
            this.DestY = destY;
            this.Direction = direction;
            this.Action = action;
        }

        private uint Timestamp
        {
            get { return this.ReadUInt32(TimestampOffset); }
            set { this.WriteUInt32(TimestampOffset, value); }
        }

        private uint UserId
        {
            get { return this.ReadUInt32(UserIdOffset); }
            set { this.WriteUInt32(UserIdOffset, value); }
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

        private uint DataExtra
        {
            get { return this.ReadUInt32(DataExtraOffset); }
            set { this.WriteUInt32(DataExtraOffset, value); }
        }

        private ushort DestX
        {
            get { return this.ReadUInt16(DestXOffset); }
            set { this.WriteUInt16(DestXOffset, value); }
        }

        private ushort DestY
        {
            get { return this.ReadUInt16(DestYOffset); }
            set { this.WriteUInt16(DestYOffset, value); }
        }

        private ushort Direction
        {
            get { return this.ReadUInt16(DirectionOffset); }
            set { this.WriteUInt16(DirectionOffset, value); }
        }

        private ActionType Action
        {
            get { return (ActionType)this.ReadInt16(ActionOffset); }
            set { this.WriteInt16(ActionOffset, (short)value); }
        }

        public override void Process()
        {
            switch (this.Action)
            {
                case ActionType.EnterMap:
                    // Start in Twin City at position (438, 377)
                    new MsgAction(this.GameNetwork, this.UserId, 1002, 438, 377, 0, ActionType.EnterMap).Send();
                    break;

                case ActionType.GetItems:
                case ActionType.GetFriends:
                case ActionType.GetProficiencies:
                case ActionType.GetMagic:
                case ActionType.GetGuildInfo:
                case ActionType.GetTutorInfo:
                    this.Send();
                    break;

                case ActionType.RequestChangeMap:
                    // For now we just send the user back to the starting location
                    new MsgAction(this.GameNetwork, this.UserId, 1002, 438, 377, 0, ActionType.ChangeMap).Send();
                    break;

                case ActionType.SetPkMode:
                    this.Send();
                    break;

                default:
                    break;
            }
        }
    }
}
