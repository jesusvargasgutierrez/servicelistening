using NUnit.Framework;
using System.Net;
using System.Net.Mail;
using Moq;
using System;
using System.IO;

namespace TestProject1
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //Mock<ISmtpClient> smtpClientMock = null;
            //sendmail Sendmail = null;

            var smtpClientMock = new Mock<ISmtpClient>();
            var Sendmail = new sendmail(smtpClientMock.Object);

            Sendmail.Sendmail();

            //Assert.DoesNotThrow(() =>
            //{
            //    smtpClientMock = new Mock<ISmtpClient>();
            //    Sendmail = new sendmail(smtpClientMock.Object);
            //});

            //Assert.IsNotNull(Sendmail);
            smtpClientMock.Verify(client => client.Send(It.IsAny<MailMessage>()), Times.Once);

            //var smtpClientMock = new Mock<SmtpClient>("sandbox.smtp.mailtrap.io", 587);
            //smtpClientMock.SetupAllProperties();
            //smtpClientMock.Setup(client => client.EnableSsl).Returns(true);
            //smtpClientMock.Setup(client => client.UseDefaultCredentials).Returns(false);
            //smtpClientMock.Setup(client => client.Credentials).Returns(new NetworkCredential("bfa7eea3ac7657", "6e47fae0f79642d"));

            //sendmail();

            //smtpClientMock.Verify(client => client.Send(It.IsAny<MailMessage>()), Times.Once);

        }
        public void sendmail()
        {
            try
            {
                using (SmtpClient client = new SmtpClient("sandbox.smtp.mailtrap.io", 587))
                {
                    client.EnableSsl = false;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("bfa7eea3ac7657", "6e47fae0f79642d");

                    MailMessage message = new MailMessage();
                    message.From = new MailAddress("send@serviciosespeciales.com.mx");
                    message.To.Add("jvargas@exosfera.com");
                    message.Subject = "Test Email";

                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                string pathfileerr = @"c:\messages\errors.txt";
                using (StreamWriter writer = new StreamWriter(pathfileerr))
                {
                    writer.WriteLine(ex.Message.ToString());
                }
            }
        }
    }
}