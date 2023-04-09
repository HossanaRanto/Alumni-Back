using Alumni_Back.Models;

namespace Alumni_Back.Serializers
{
    public class MessageSerializer
    {
        public bool IsSender { get; set; }
        public string Message { get; set; }
        public DateTime DateSend { get; set; }

        public MessageSerializer(Message message,User connecteduser)
        {
            this.Message = message.Content;
            this.DateSend = message.DateSend;

            if (message.Sender == connecteduser)
            {
                this.IsSender = true;
            }
            if(message.Receiver== connecteduser)
            {
                this.IsSender = false;
            }
        }
    }
}
