using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripPlanner.Logic.Services.EmailService.Smtp
{
    public class SmtpOptions
    {
        public string Host { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public bool EnableSsl { get; set; } = false;
        public string ServerName { get; set; } = string.Empty;
        public string EncryptionMethod { get; set; } = string.Empty;
    }
}
