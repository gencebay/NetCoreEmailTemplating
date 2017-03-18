using Microsoft.AspNetCore.Mvc;
using NetCore.Contracts;
using System.Threading.Tasks;

namespace NetCore.Integrations.Controllers
{
    [Route("api/[controller]")]
    public class MailController : Controller
    {
        [HttpPost(nameof(SendAsync))]
        public async Task SendAsync([FromBody]RegistrationEmailViewModel message)
        {
            await Task.CompletedTask;
        }
    }
}
