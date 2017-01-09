using System.Xml.Linq;
using System.IO;
using System.Xml.XPath;

namespace MicroChallenge5
{
    public class MessageEditor
    {
        private Message _initialMessage;
        private Message _editedMessage;

        public static XDocument Edit(Message currMessage)
        {
            XDocument Initial = currMessage.GetValidatedFile();
            return Initial;//This is temporary. Make sure to validate file after editing when we implement.
        }

        public MessageEditor(Message messageToEdit)
        {
            _initialMessage = messageToEdit;
            _editedMessage = messageToEdit;
        }

        public string View()
        {
            return _editedMessage.ToString();
        }

        public void EditData(string xPathString)
        {
            XPathExpression xPath = XPathExpression.Compile(xPathString);
            EditData(xPath);
        }

        public void EditData(XPathExpression xPath)
        {
            XDocument xmlDocument = _editedMessage.GetValidatedFile();
            XPathNavigator navigator = xmlDocument.CreateNavigator();
            navigator.Select(xPath);
            return;
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
