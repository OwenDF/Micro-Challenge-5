using System.Xml.Linq;
using System.Net.Mail;
using System.Net;

namespace MicroChallenge5
{
    public class MessageEmailer
    {
        private Message _messageToEmail;
        private string _IPAddrSMTPServer = "email.calastone.com";

        public MessageEmailer(Message suppliedMessage)
        {
            _messageToEmail = suppliedMessage;
        }

        public void Send(string senderAddr, string receiverAddr)
        {
            string Subject = "A lovely XML file";//This is the subject and you will like it.
            string _IPAddrSMTPServer = GetIPAddrSMTPServ();
            XDocument validatedFile = _messageToEmail.GetValidatedFile();

            using (SmtpClient Email = new SmtpClient(_IPAddrSMTPServer))
            {
                Email.Credentials = CredentialCache.DefaultNetworkCredentials;
                Email.Send(senderAddr, receiverAddr, Subject, validatedFile.ToString());
                _messageToEmail.IncrementSentMessages();
            }
        }

        public string GetIPAddrSMTPServ() { return _IPAddrSMTPServer; }

        public void SetIPAddrSMTPServ(string addr) { _IPAddrSMTPServer = addr; }
    }
}
