using System;
using System.Collections.Generic;

namespace EmailReader.ViewModels
{
    public class EmailMessage
    {
        public IEnumerable<string> To { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public bool HasAttachment { get; set; }
        public dynamic Attachment { get; set; }
    }
}
