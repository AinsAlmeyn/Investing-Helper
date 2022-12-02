using GunlukHisseSenediRaporu.API.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Runtime.Intrinsics.X86;

namespace Investing_Helper.Services
{
    public interface IMailService
    {
        public void SendEmail(List<Hisse> data);
    }

    public class MailService : IMailService
    {
        private IConfiguration configuration;
        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private List<string> emails = new()
        {
            "kocakakay@gmail.com"
        };

        public void SendEmail(List<Hisse> hisseler)
        {
            string subject = "Gunluk Borsa Raporu";
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(configuration.GetSection("EmailUsername").Value));
            foreach (var item in emails)
            {
                email.To.Add(MailboxAddress.Parse(item));
                email.Subject = subject;
                BodyBuilder builder = new BodyBuilder();
                builder.HtmlBody = @$"
                                    
<ul>
<h1>VESBE</h1>
    <li>
        <h3>Onceki Kapanis: {hisseler[0].OncekiKapanis}</h3>
    </li>
    <li>
        <h3>Kapanis: {hisseler[0].Kapanis}</h3>
    </li>
    <li>
        <h3>Gunluk Aralik: {hisseler[0].GunlukAralik}</h3>
    </li>
    <li>
        <h3>Yillik Aralik: {hisseler[0].YillikAralik}</h3>
    </li>
    <li>
        <h3>F/K Orani: {hisseler[0].FKOrani}</h3>
    </li>
    <li>
        <h3>Piyasa Degerli: {hisseler[0].PiyasaDegeriTl}</h3>
    </li>
    <li>
        <h3>Temettu Getirisi: {hisseler[0].TemettuGetirisi}</h3>
    </li>
</ul>";
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(configuration.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(configuration.GetSection("EmailUsername").Value, configuration.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}
