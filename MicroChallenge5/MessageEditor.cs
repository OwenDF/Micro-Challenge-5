using System.Xml.Linq;
using System.IO;

namespace MicroChallenge5
{
    public class MessageEditor
    {
        public static XDocument Edit(Message CurrMessage)
        {
            XDocument Initial = CurrMessage.GetValidatedFile();
            return Initial;//This is temporary. Make sure to validate file after editing when we implement.
        }

        public static void Save(XDocument SaveFile, string SaveAs, Message CurrMessage)
        {
            using (StreamWriter NewXmlFile = new StreamWriter(SaveAs, false))
            {
                NewXmlFile.WriteLine(SaveFile);
                NewXmlFile.Close();
                CurrMessage.SetValidatedFile(SaveFile);
            }
        }
    }
}
