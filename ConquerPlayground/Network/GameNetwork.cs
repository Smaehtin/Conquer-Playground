namespace ConquerPlayground.Network
{
    using System.Collections.Generic;

    internal class GameNetwork
    {
        private Queue<NetMsg> clientMessages;
        private Queue<NetMsg> messages;

        public GameNetwork()
        {
            this.clientMessages = new Queue<NetMsg>();
            this.messages = new Queue<NetMsg>();
        }

        public void AddClientMessage(NetMsg message)
        {
            this.clientMessages.Enqueue(message);
        }

        public void ProcessClientMessages()
        {
            if (this.clientMessages.Count > 0)
            {
                var message = this.clientMessages.Dequeue();
                message.Process();
            }
        }

        public bool ReceiveMsg(out NetMsg message)
        {
            if (this.messages.Count > 0)
            {
                message = this.messages.Dequeue();
                return true;
            }
            else
            {
                message = null;
                return false;
            }
        }

        public void SendMsg(NetMsg message)
        {
            this.messages.Enqueue(message);
        }
    }
}
