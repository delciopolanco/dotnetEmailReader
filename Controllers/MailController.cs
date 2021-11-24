using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailReader.Services.MailReader;
using EmailReader.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EmailReader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {

        private readonly ILogger<MailController> _logger;
        private readonly IMailReader _mailReader;

        public MailController(ILogger<MailController> logger, IMailReader mailReader)
        {
            _logger = logger;
            _mailReader = mailReader;
        }

        [HttpGet]
        public async Task<IEnumerable<EmailMessage>> Get()
        {
            var unreadMessages = await _mailReader.AllGetUnreadsEmails();

            var messages = unreadMessages.Select(m => new EmailMessage()
            {
                To = m.To.Select(t => t.Name),
                Subject = m.Subject
            });

            return messages;
        }

        [HttpGet("Criteria")]
        public async Task<IEnumerable<EmailMessage>> GetByCriteria(string subject)
        {
            var unreadMessages =  _mailReader.GetUnreadsEmails(new MailCriteria() { Subject = subject });

            var messages = unreadMessages.Select(m => new EmailMessage()
            {
                To = m.To.Select(t => t.Name),
                Subject = m.Subject
            });

            await Task.CompletedTask;

            return messages;
        }
    }
}
