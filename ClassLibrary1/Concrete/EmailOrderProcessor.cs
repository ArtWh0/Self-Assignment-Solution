using System.Net;
using System.Net.Mail;
using System.Text;
using TechRent.Domain.Abstract;
using TechRent.Domain.Entities;

namespace TechRent.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "techrent@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"D:\self_assigment\Solution2\Test_Order_Emails";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                    = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod
                        = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("The order is processed!")
                    .AppendLine("---")
                    .AppendLine("Technics:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Tech.Min_Price + (line.Over_Days * line.Tech.Rent_Price);                    
                    body.AppendFormat("{0} x {1} - ({2} euro) and +{3} points ",
                        line.Over_Days, line.Tech.Name, subtotal, line.Tech.Points);
                }

                body.AppendFormat(" Total price: {0} euro and Total points: {1} ", cart.ComputeTotalValue(), cart.ComputeTotalPoints())
                    .AppendLine("---")
                    .AppendLine("Contact details:")
                    .AppendLine(shippingInfo.FirstName)
                    .AppendLine(shippingInfo.LastName)
                    .AppendLine(shippingInfo.EMail)
                    .AppendLine("---");

                MailMessage mailMessage = new MailMessage(
                                       emailSettings.MailFromAddress,	
                                       emailSettings.MailToAddress,		
                                       "Your order on site 'TechRent' is processed",		
                                       body.ToString()); 				

                if (emailSettings.WriteAsFile=true)
                {
                    mailMessage.BodyEncoding = Encoding.UTF8;
                }

                smtpClient.Send(mailMessage);
            }
        }
    }
}