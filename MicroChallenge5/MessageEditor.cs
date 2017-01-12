using System.Xml.Linq;
using System.IO;
using System.Xml.XPath;

namespace MicroChallenge5
{
    public class MessageEditor
    {
        private Message _messageToEdit;

        public static XDocument Edit(Message currMessage)
        {
            XDocument Initial = currMessage.GetValidatedFile();
            return Initial;//This is temporary. Make sure to validate file after editing when we implement.
        }

        public MessageEditor(Message messageToEdit)
        {
            _messageToEdit = messageToEdit;
        }

        public string View()
        {
            return _messageToEdit.ToString();
        }

        public XDocument EditMessage(string xPath, string newText)
        {
            XDocument document = _messageToEdit.GetValidatedFile();
            document.XPathSelectElement(xPath, _messageToEdit.GetNamespaces()).Value = newText;
            return document;
        }

        public static void Save(XDocument SaveFile, string SaveAs, Message currMessage)
        {
            using (StreamWriter NewXmlFile = new StreamWriter(SaveAs, false))
            {
                NewXmlFile.WriteLine(SaveFile);
                NewXmlFile.Close();
                currMessage.SetValidatedFile(SaveFile);
            }
        }
    }
}
