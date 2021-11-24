using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MimeKit;

namespace EmailReader.Services.MailReader
{
    public interface IMailReader
    {
        ImapClient Connect();

        ImapClient Connect(string userName, string passWord, string mailServer, int port, bool isSsl);

        Task<IEnumerable<MimeMessage>> AllGetUnreadsEmails();

        IEnumerable<MimeMessage> GetUnreadsEmails(MailCriteria criteria);
        
    }
}
