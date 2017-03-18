using Microsoft.AspNetCore.Mvc;
using NetCore.Contracts;
using System.Threading.Tasks;

namespace NetCore.Integrations.Controllers
{
    [Route("api/[controller]")]
    public class MailController : Controller
    {
        private readonly MailProvider _mailProvider;

        public MailController(MailProvider mailProvider)
        {
            _mailProvider = mailProvider;
        }        

        [HttpPost(nameof(SendAsync))]
        public async Task SendAsync([FromBody]RegistrationEmailViewModel message)
        {
            var messageBody = await _mailProvider.RenderMailViewAsync(ControllerContext, MailTemplate.RegistrationComplete, message);
            await _mailProvider.SendAsync(message.To, message.Subject, messageBody);
        }
    }
}