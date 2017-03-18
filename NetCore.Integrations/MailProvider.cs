using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using NetCore.Contracts;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NetCore.Integrations
{
    public class MailProvider
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataDictionaryFactory _tempDataFactory;
        private readonly IOptions<MailSettings> _mailOptions;

        protected MailSettings MailSettings
        {
            get
            {
                return _mailOptions.Value;
            }
        }

        public MailProvider(ICompositeViewEngine viewEngine, 
            ITempDataDictionaryFactory tempDataFactory, 
            IOptions<MailSettings> mailOptions)
        {
            _viewEngine = viewEngine;
            _tempDataFactory = tempDataFactory;
            _mailOptions = mailOptions;
        }

        public async Task<string> RenderMailViewAsync(ControllerContext controllerContext, MailTemplate template, object model)
        {
            var viewResult = _viewEngine.FindView(controllerContext, template.ToString(), false);

            if (!viewResult.Success)
            {
                throw new ArgumentOutOfRangeException(nameof(template));
            }

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), controllerContext.ModelState);
            var tempData = _tempDataFactory?.GetTempData(controllerContext.HttpContext);
            using (StringWriter sw = new StringWriter())
            {
                viewData.Model = model;
                ViewContext viewContext = new ViewContext(controllerContext,
                    viewResult.View, viewData, tempData, sw, new HtmlHelperOptions());

                await viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }

        public async Task SendAsync(string to, string subject, string body, bool isHtml = true)
        {
            string message = string.Empty;
            using (var client = new SmtpClient(MailSettings.Host, MailSettings.Port))
            {
                try
                {
                    client.EnableSsl = false;
                    client.UseDefaultCredentials = false;

                    // TODO Password hash
                    client.Credentials = new NetworkCredential(MailSettings.Username, MailSettings.Password);

                    var mailMessage = new MailMessage(MailSettings.From, to, subject, body);
                    mailMessage.IsBodyHtml = isHtml;
                    await client.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    // TODO Log
                    throw ex;
                }
            }
        }
    }
}
