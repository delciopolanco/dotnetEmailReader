using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using EmailReader.ViewModels;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailReader.Services.MailReader
{
    public class MailReader : IMailReader
    {
        private readonly string mailUser = "";
        private readonly string mailPassword = "";
        private readonly string mailServer = "";
        private readonly int mailPort;
        private readonly bool mailIsSsl;

        private ImapClient imapClient;

        private readonly IOptions<MailSettings> _config;


        public MailReader(IOptions<MailSettings> config)
        {
            _config = config;

            mailUser = _config.Value.google.user;
            mailPassword = _config.Value.google.password;
            mailServer = _config.Value.google.server;
            mailPort = _config.Value.google.port;
            mailIsSsl = _config.Value.google.isSsl;
        }

        public ImapClient Connect()
        {
            if (String.IsNullOrEmpty(mailUser) || String.IsNullOrEmpty(mailPassword) || String.IsNullOrEmpty(mailServer))
                throw new NotImplementedException("You must provide user, password and mailserver to connect.");

            return Connect(mailUser, mailPassword, mailServer, mailPort, mailIsSsl);
        }

        public ImapClient Connect(string user,
                            string password,
                            string server,
                            int port,
                            bool isSsl)
        {
            if (String.IsNullOrEmpty(user) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(server))
                throw new NotImplementedException("You must provide user, password and mailserver to connect.");

            imapClient = new ImapClient();

            imapClient.Connect(server, port, SecureSocketOptions.SslOnConnect);
            imapClient.AuthenticationMechanisms.Remove("XOAUTH2");
            imapClient.Authenticate(user, password);

            return imapClient;
        }

        public async Task<IEnumerable<MimeMessage>> AllGetUnreadsEmails()
        {
            var messages = new List<MimeMessage>();

            using (var client = Connect())
            {

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                var results = inbox.Search(SearchOptions.All, SearchQuery.Not(SearchQuery.Seen));
                foreach (var uniqueId in results.UniqueIds)
                {
                    var message = await inbox.GetMessageAsync(uniqueId);

                    messages.Add(message);

                    //Mark message as read
                    //inbox.AddFlags(uniqueId, MessageFlags.Seen, true);
                }

                client.Disconnect(true);
            }

            return messages;
        }

        public IEnumerable<MimeMessage> GetUnreadsEmails(MailCriteria criteria)
        {
            var messages = new List<MimeMessage>();

            using (var client = Connect())
            {
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                var query = SearchQuery.SubjectContains(criteria.Subject);

                // let's do the same search, but this time sort them in reverse arrival order
                var orderBy = new[] { OrderBy.ReverseDate };

                var uids = client.Inbox.Search(query);
                var mails = client.Inbox.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure);

                foreach (var uid in mails)
                {
                    var message = client.Inbox.GetMessage(uid.Index);

                    messages.Add(message);
                }

                client.Disconnect(true);

                return messages;
            }
        }
    }
}
