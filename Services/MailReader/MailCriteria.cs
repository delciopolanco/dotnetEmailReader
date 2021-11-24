using System;
namespace EmailReader.Services.MailReader
{
    public class MailCriteria
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool HasAttachment { get; set; }
    }
}
