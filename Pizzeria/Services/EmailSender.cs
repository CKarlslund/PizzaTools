using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pizzeria.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            Console.WriteLine($"Sent email to {email}: {message}");
            Debug.WriteLine($"Sent email to {email}: {message}");
            return Task.CompletedTask;
        }
    }
}
