using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Models.Email;
using Microsoft.Extensions.Options;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System.Diagnostics;
using System.Net;

namespace HR.LeaveManagement.Infrastructure.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmail(EmailMessage email)
        {
            Configuration.Default.ApiKey.Add("api-key", _emailSettings.ApiKey);

            var apiInstance = new TransactionalEmailsApi();

            SendSmtpEmailSender sender = new() { Email = _emailSettings.FromAddress, Name = _emailSettings.FromName };


            SendSmtpEmail sendSmtpEmail = new()
            {
                Sender = sender,
                To = email.To.Select(s => new SendSmtpEmailTo() { Email = s }).ToList(),
                HtmlContent = email.Body
            };

            try
            {
                // Send a transactional email
                var result = await apiInstance.SendTransacEmailAsyncWithHttpInfo(sendSmtpEmail);
                return result.StatusCode == (int)HttpStatusCode.OK || result.StatusCode == (int)HttpStatusCode.Accepted;
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling TransactionalEmailsApi.SendTransacEmail: " + e.Message);
                return false;
            }


        }
    }
}
