namespace ConquerPlayground.Network
{
    using System;
    using System.Linq;

    internal enum TextAttribute : ushort
    {
        Normal = 2000,
        Whisper = Normal + 1,
        Action = Normal + 2,
        Team = Normal + 3,
        Guild = Normal + 4,
        System = Normal + 5,
        Family = Normal + 6,
        Talk = Normal + 7,
        Friend = Normal + 9,
        Gm = Normal + 11,
        Ghost = Normal + 13,
        World = Normal + 21,
        Qualifier = Normal + 22,
        Entrance = Normal + 101,
        Shop = Normal + 104,
        Map = Normal + 108,
        MapExtra = Normal + 109,
        Broadcast = Normal + 500
    }

    internal class MsgTalk : NetMsg
    {
        private const int ColorOffset = 4;
        private const int TextAttributeOffset = 8;
        private const int TextStyleOffset = 10;
        private const int TimeOffset = 12;
        private const int StringsOffset = 24;
        private const int SizeOfMessage = StringsOffset;

        public MsgTalk(GameNetwork gameNetwork)
            : base(gameNetwork)
        {
        }

        public MsgTalk(GameNetwork gameNetwork, string speaker, string hearer, string words, uint color = 0xFFFFFFFF, TextAttribute textAttribute = TextAttribute.Normal, ushort textStyle = 0)
            : base(gameNetwork, SizeOfMessage, MsgType.Talk)
        {
            this.Color = color;
            this.TextAttribute = textAttribute;
            this.TextStyle = textStyle;
            this.Time = (uint)Environment.TickCount;
            this.MsgSize += this.WriteStrings(StringsOffset, speaker, hearer, string.Empty, words, string.Empty, string.Empty);
        }

        private uint Color
        {
            get { return this.ReadUInt32(ColorOffset); }
            set { this.WriteUInt32(ColorOffset, value); }
        }

        private TextAttribute TextAttribute
        {
            get { return (TextAttribute)this.ReadUInt16(TextAttributeOffset); }
            set { this.WriteUInt16(TextAttributeOffset, (ushort)value); }
        }

        private ushort TextStyle
        {
            get { return this.ReadUInt16(TextStyleOffset); }
            set { this.WriteUInt16(TextStyleOffset, value); }
        }

        private uint Time
        {
            get { return this.ReadUInt32(TimeOffset); }
            set { this.WriteUInt32(TimeOffset, value); }
        }

        public override void Process()
        {
            var strings = this.ReadStrings(StringsOffset);

            // Some very sloppy, basic "command" handling
            if (strings.Length > 3)
            {
                var message = strings[3];

                if (message.StartsWith("/") && message.Length > 1)
                {
                    var parsedMessage = message.TrimStart('/').Split(' ');
                    var command = parsedMessage[0];
                    var parameters = parsedMessage.Skip(1).ToArray();

                    switch (command)
                    {
                        case "announce":
                            if (parameters.Length > 0)
                            {
                                new MsgTalk(this.GameNetwork, "SYSTEM", "ALLUSERS", string.Join(" ", parameters), textAttribute: TextAttribute.Gm).Send();
                            }

                            break;

                        case "goto":
                            if (parameters.Length > 2)
                            {
                                ushort map;
                                ushort destX;
                                ushort destY;

                                if (ushort.TryParse(parameters[0], out map) && ushort.TryParse(parameters[1], out destX) && ushort.TryParse(parameters[2], out destY))
                                {
                                    new MsgAction(this.GameNetwork, 1000001, map, destX, destY, 0, ActionType.ChangeMap).Send();
                                }
                            }

                            break;

                        case "item":
                            if (parameters.Length > 0)
                            {
                                uint item;

                                if (uint.TryParse(parameters[0], out item))
                                {
                                    new MsgItemInfo(this.GameNetwork, item).Send();
                                }
                            }

                            break;
                    }
                }
            }
        }
    }
}
