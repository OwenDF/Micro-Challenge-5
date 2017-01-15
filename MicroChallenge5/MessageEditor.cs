using System.Xml.Linq;
using System.IO;
using System.Xml.XPath;

namespace MicroChallenge5
{
    public class MessageEditor
    {
        private Message _messageToEdit;

        public MessageEditor(Message messageToEdit)
        {
            _messageToEdit = messageToEdit;
        }

        public XDocument EditMessage(string xPath, string newText)
        {
            XDocument document = _messageToEdit.GetValidatedFile();
            document.XPathSelectElement(xPath, _messageToEdit.Namespaces).Value = newText;
            return document;
        }

        public static void Save(XDocument SaveFile, string SaveAs, Message currMessage)
        {
            // This is an old function, may want to update it so it's not static or move it to another class.
            using (StreamWriter NewXmlFile = new StreamWriter(SaveAs, false))
            {
                NewXmlFile.WriteLine(SaveFile);
                NewXmlFile.Close();
                currMessage.SetValidatedFile(SaveFile);
            }
        }
    }
}
