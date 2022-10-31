using Application.Interfaces;
using Infrastructure;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendHasloEmail(string to, string createdPassword)
        {
            var body = string.Format(
               "<h2>" +
               "Twoje nowe hasło w systemie to: " +
               "<p style='font - family: Arial, Helvetica, sans - serif; color: #00B2EE;'>" +
               "{0}" +
               "</p>" +
               "</h2>" +
               "</br>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "Klinika PetMed" +
               "</p>", createdPassword);

            var email = CreateEmail(to, "Twoje konto w serwisie PetMed zostało utworzone");
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            email.Body = bodyBuilder.ToMessageBody();

            await Send(email);
        }

        public async Task SendUmowWizytaEmail(string to, string data)
        {
            var body = string.Format(
               "<h2>" +
               "Potwiedzenie rezerwacji w klinice PetMed" +
               "</h2>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "Termin: " +
               "{0}" +
               "</p>" +
               "</br>" +
               "<p style='font - family: Arial, Helvetica, sans - serif;'>" +
               "Klinika PetMed" +
               "</p>", data);

            var email = CreateEmail(to, "Potwierdzenie wizyty");
            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            email.Body = bodyBuilder.ToMessageBody();

            await Send(email);
        }

        private MimeMessage CreateEmail(string to, string subject)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.Add(new MailboxAddress(to));
            emailMessage.Subject = subject;

            return emailMessage;
        }

        private async Task Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    ///await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    //client.AuthenticationMechanisms.Remove("XOAUTH2");

                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.Auto);
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    await client.SendAsync(mailMessage);
                }
                catch (Exception e)
                {
                    throw new Exception(_emailConfig.Password);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}