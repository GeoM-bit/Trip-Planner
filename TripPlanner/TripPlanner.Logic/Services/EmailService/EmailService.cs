using Microsoft.Extensions.Options;
using System.Net.Mail;
using TripPlanner.Logic.Services.EmailService.Smtp;

namespace TripPlanner.Logic.Services.EmailService
{   

    public class EmailService : IEmailService
    {
        private readonly EmailUser _user;
        private ISmtpClient _smtpClient;
         string template = @"<table class='body-wrap' style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; width: 100%; background-color: #f6f6f6; margin: 0;' bgcolor='#f6f6f6'>
  <tbody>
      <tr style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;'>
          <td style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0;' valign='top'></td>
          <td class='container' width='600' style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; display: block !important; max-width: 600px !important; clear: both !important; margin: 0 auto;'
              valign='top'>
              <div class='content' style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; max-width: 600px; display: block; margin: 0 auto; padding: 20px;'>
                  <table class='main' width='100%' cellpadding='0' cellspacing='0' style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; border-radius: 3px; background-color: #fff; margin: 0; border: 1px solid #e9e9e9;'
                      bgcolor='#fff'>
                      <tbody>
                          <tr style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;'>
                              <td class='' style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 16px; vertical-align: top; color: #fff; font-weight: 500; text-align: center; border-radius: 3px 3px 0 0; background-color: #476DDA; margin: 0; padding: 10px;'
                                  align='center' bgcolor='#71b6f9' valign='top'>
                                  <p style='font-size:28px;color:#fff;'> Business trip request was {0} </p>
                              </td>
                          </tr>
                          <tr style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;'>
                              <td class='content-wrap' style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 20px;' valign='top'>
                                  <table width='100%' cellpadding='0' cellspacing='0' style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;'>
                                      <tbody>
                                          <tr style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;'>
                                              <td class='content-block' style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0; padding: 0 0 20px;' valign='top'>
                                                  Dear <strong>{1}</strong>, <br/> <br/>
                                                  {2} <br /> 
                                                  
                                              </td>
                                          </tr>                                         
                                         
                                      </tbody>
                                  </table>
                              </td>
                          </tr>
                      </tbody>
                  </table>
                  <div class='footer' style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; width: 100%; clear: both; color: #999; margin: 0; padding: 20px;'>
                      <table width='100%' style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;'>
                          <tbody>
                              <tr style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; margin: 0;'>   
                                  <td class='aligncenter content-block' style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 12px; vertical-align: top; color: #999; text-align: center; margin: 0; padding: 0 0 20px;' align='center' valign='top'>
                                      <p style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 12px; color: #999; text-decoration: underline; margin: 0;'>
                                      Nagarro iQuest Technologies S.R.L 
                                      </p> 
                                  </td>
                              </tr>
                          </tbody>
                      </table>
                  </div>
              </div>
          </td>
          <td style='font-family: 'Helvetica Neue',Helvetica,Arial,sans-serif; box-sizing: border-box; font-size: 14px; vertical-align: top; margin: 0;' valign='top'></td>
      </tr>
  </tbody>
</table>";

        public EmailService(ISmtpClient smtpClient, IOptions<EmailUser> user)
        {
            _smtpClient = smtpClient;
            _user = user.Value;
        }

        public async void SendEmail(string status,string email) 
        {
            MailAddress from = new MailAddress(_user.Email);
            MailAddress to = new MailAddress("paul.crasmareanu@nagarro.com");
            MailMessage message = new MailMessage(from, to);
            
            message.IsBodyHtml = true;
            switch (status)
            {
                case "accepted": message.Body = string.Format(template, status, email,"You trip request was aceepted");break;
                case "rejected": message.Body = string.Format(template, status, email,"Your trip request was rejected");break;
                case "created": message.Body = string.Format(template, status, email,"You created a new trip request"); break;
            }
            

            await _smtpClient.SendMailAsync(message);
        }
    }
}
