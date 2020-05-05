namespace ConquerPlayground.Network
{
    internal class MsgUserInfo : NetMsg
    {
        private const int IdOffset = 4;
        private const int LookFaceOffset = 8;
        private const int HairOffset = 12;
        private const int MoneyOffset = 14;
        private const int CpsOffset = 18;
        private const int ExperienceOffset = 22;
        private const int StrengthOffset = 46;
        private const int AgilityOffset = 48;
        private const int VitalityOffset = 50;
        private const int SpiritOffset = 52;
        private const int AttributePointsOffset = 54;
        private const int HealthOffset = 56;
        private const int ManaOffset = 58;
        private const int PkPointsOffset = 60;
        private const int LevelOffset = 62;
        private const int ProfessionOffset = 63;
        private const int RebornOffset = 64;
        private const int StringsOffset = 67;
        private const int SizeOfMessage = StringsOffset;

        public MsgUserInfo(GameNetwork gameNetwork)
            : base(gameNetwork)
        {
        }

        public MsgUserInfo(GameNetwork gameNetwork, string name)
            : base(gameNetwork, SizeOfMessage, MsgType.UserInfo)
        {
            this.Id = 1000001;
            this.LookFace = 0x2AFB;
            this.Money = 999999999;
            this.Cps = 999999999;
            this.Strength = 255;
            this.Agility = 255;
            this.Vitality = 255;
            this.Spirit = 255;
            this.Health = 8415;
            this.Mana = 1275;
            this.Level = 130;
            this.Profession = 25;
            this.MsgSize += this.WriteStrings(StringsOffset, name);
        }

        private uint Id
        {
            get { return this.ReadUInt32(IdOffset); }
            set { this.WriteUInt32(IdOffset, value); }
        }

        private uint LookFace
        {
            get { return this.ReadUInt32(LookFaceOffset); }
            set { this.WriteUInt32(LookFaceOffset, value); }
        }

        private short Hair
        {
            get { return this.ReadInt16(HairOffset); }
            set { this.WriteInt16(HairOffset, value); }
        }

        private uint Money
        {
            get { return this.ReadUInt32(MoneyOffset); }
            set { this.WriteUInt32(MoneyOffset, value); }
        }

        private uint Cps
        {
            get { return this.ReadUInt32(CpsOffset); }
            set { this.WriteUInt32(CpsOffset, value); }
        }

        private uint Experience
        {
            get { return this.ReadUInt32(ExperienceOffset); }
            set { this.WriteUInt32(ExperienceOffset, value); }
        }

        private ushort Strength
        {
            get { return this.ReadUInt16(StrengthOffset); }
            set { this.WriteUInt16(StrengthOffset, value); }
        }

        private ushort Agility
        {
            get { return this.ReadUInt16(AgilityOffset); }
            set { this.WriteUInt16(AgilityOffset, value); }
        }

        private ushort Vitality
        {
            get { return this.ReadUInt16(VitalityOffset); }
            set { this.WriteUInt16(VitalityOffset, value); }
        }

        private ushort Spirit
        {
            get { return this.ReadUInt16(SpiritOffset); }
            set { this.WriteUInt16(SpiritOffset, value); }
        }

        private ushort AttributePoints
        {
            get { return this.ReadUInt16(AttributePointsOffset); }
            set { this.WriteUInt16(AttributePointsOffset, value); }
        }

        private ushort Health
        {
            get { return this.ReadUInt16(HealthOffset); }
            set { this.WriteUInt16(HealthOffset, value); }
        }

        private ushort Mana
        {
            get { return this.ReadUInt16(ManaOffset); }
            set { this.WriteUInt16(ManaOffset, value); }
        }

        private ushort PkPoints
        {
            get { return this.ReadUInt16(PkPointsOffset); }
            set { this.WriteUInt16(PkPointsOffset, value); }
        }

        private byte Level
        {
            get { return this.ReadByte(LevelOffset); }
            set { this.WriteByte(LevelOffset, value); }
        }

        private byte Profession
        {
            get { return this.ReadByte(ProfessionOffset); }
            set { this.WriteByte(ProfessionOffset, value); }
        }

        private byte Reborn
        {
            get { return this.ReadByte(RebornOffset); }
            set { this.WriteByte(RebornOffset, value); }
        }
    }
}
