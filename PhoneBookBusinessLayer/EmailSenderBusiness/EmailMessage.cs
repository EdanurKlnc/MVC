
namespace PhoneBookBusinessLayer.EmailSenderBusiness
{
    public class EmailMessage
    {
        public string[] To { get; set; } //kime gönderilecek
        public string Subject { get; set; } //konusu
        public string Body { get; set; } //içerik
        public string[] CC { get; set; } 
        public string[] BCC { get; set; } 


    }
}
