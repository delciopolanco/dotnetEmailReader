using System;
namespace EmailReader.Services.MailReader
{
    public class MailSettings
    {
        public Google google { get; set; }
    }

    public class Google
    {
        public String server { get; set; }
        public String user { get; set; }
        public String password { get; set; }
        public int port { get; set; }
        public bool isSsl { get; set; }
    }
}
